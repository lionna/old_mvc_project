using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Extension;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using CarServiceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    public class EmployeeController(
        IEmployeeService employeeService,
        IGenericRepositoryAsync<Post, int> postRepository,
        IGenericRepositoryAsync<Department, int> departmentRepository) : Controller
    {
        private const int PageSize = 5;

        private readonly IEmployeeService _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        private readonly IGenericRepositoryAsync<Post, int> _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        private readonly IGenericRepositoryAsync<Department, int> _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));

        public ActionResult Search()
        {
            return View();
        }

        public async Task<ActionResult> AutocompleteSearch(string term)
        {
            var isContainsDigit = false;
            foreach (var currentChar in term.ToCharArray())
            {
                if (char.IsDigit(currentChar))
                    isContainsDigit = true;
            }
            if (isContainsDigit)
            {
                var nameClient = await _employeeService.GetAsync(term);

                return Json(nameClient);
            }
            else
            {
                var nameClient = _employeeService.GetByNameAsync(term);

                return Json(nameClient);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Search(string name, string id, int? page)
        {
            var employees = await _employeeService.GetAsync(id, name, page ?? 0, PageSize);

            if (employees.TotalItems == 0)
            {
                return NotFound();
            }

            return PartialView("ResultsSearch", employees);
        }

        [HttpGet]
        public async Task<ActionResult> ShowDetails(int? personnelNumber)
        {
            Employee employee = null;
            if (personnelNumber != null)
                employee = await _employeeService.GetByIdAsync(personnelNumber ?? 0);

            if (employee != null)
            {
                ViewBag.ShortName = employee.FullName.ConvertToShortName();

                return View(employee);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? personnelNumber)
        {
            var employees = await _employeeService.GetByIdAsync(personnelNumber ?? 0);

            if (personnelNumber != null)
            {
                ViewBag.ShortName = employees.FullName.ConvertToShortName();
                ViewBag.ListPost = await _postRepository.GetSelectListAsync();
                ViewBag.ListDepart = await _departmentRepository.GetSelectListAsync();

                return View(employees);
            }

            return NotFound();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var existingEmployee = await _employeeService.GetByIdAsync(employee.PersonnelNumber);
                if (existingEmployee != null)
                {
                    await _employeeService.UpdateAsync(employee);

                    return RedirectToAction("ShowDetails", new { employee.PersonnelNumber });
                }
            }

            ViewBag.ListPost = await _postRepository.GetSelectListAsync();
            ViewBag.ListDepart = await _departmentRepository.GetSelectListAsync();

            return View(employee);
        }

        public async Task<ActionResult> ShowContracts(int? personnelNumber)
        {
            if (personnelNumber != null)
            {
                var employees = await _employeeService.GetByIdAsync(personnelNumber ?? 0);
                IEnumerable<ContractToEmployee> contractsByEmployees = employees.ContractToEmployees;
                var contracts = new ContractsViewModel
                {
                    Contracts = contractsByEmployees,
                    FullName = employees.FullName,
                    PersonnelNumber = employees.PersonnelNumber
                };
                ViewBag.ShortName = employees?.FullName?.ConvertToShortName();

                return View(contracts);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewBag.ListPost = await _postRepository.GetSelectListAsync();
            ViewBag.ListDepart = await _departmentRepository.GetSelectListAsync();

            return View(new Employee());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee employee)
        {
            if (employee != null && ModelState.IsValid)
            {
                await _employeeService.CreateAsync(employee);

                return RedirectToAction("ShowDetails", employee);
            }
            if (!ModelState.IsValid)
            {
                ViewBag.ListPost = await _postRepository.GetSelectListAsync();
                ViewBag.ListDepart = await _departmentRepository.GetSelectListAsync();

                return View(employee);
            }

            return NotFound();
        }

        public async Task<ActionResult> Delete(int? personnelNumber)
        {
            var employees = await _employeeService.GetByIdAsync(personnelNumber ?? 0);
            if (employees != null)
            {
                try
                {
                    await _employeeService.DeleteAsync(employees);
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, $"Данный сотрудник не может быть удален!");
                    return View();
                    // return JavaScript("$('#popErorr').html('Данный сотрудник не может быть удален!').slideDown(500).delay(3000).slideUp(500); ");
                }
                ModelState.AddModelError(string.Empty, $"Сотрудник, {employees.FullName}, удален!");
                return View();
                //return JavaScript("$('#popSuccess').html('Сотрудник, " + employees.FullName + ", удален!').slideDown(500).delay(3000).slideUp(500); setInterval(function () {window.location.href='/Employee/Search';},4000)");
            }

            ModelState.AddModelError(string.Empty, $"Сотрудник не найден!");
            return View();
        }
    }
}