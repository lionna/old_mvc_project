using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Service.Abstract;
using CarServiceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    public class ContractController(IContractService contractService) : Controller
    {
        private const int TakeValue6 = 6;
        private readonly IContractService _contractService = contractService ?? throw new ArgumentNullException(nameof(contractService));

        [HttpGet]
        public ActionResult CreateContract(int? personnelNumber)
        {
            if (personnelNumber != null)
            {
                var newContract = new NewContractViewModel
                {
                    PersonnelNumber = personnelNumber,
                    RecruitDate = DateOnly.FromDateTime(DateTime.UtcNow)
                };

                return PartialView(newContract);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreateContract(NewContractViewModel newContract)
        {
            if (newContract != null && ModelState.IsValid)
            {
                if (await _contractService.CreateNewContractAsync(newContract.PersonnelNumber ?? 0, newContract.RecruitDate, newContract.Type, newContract.Term ?? 0) > 0)
                {
                    return PartialView("UpdatingTable", await _contractService.GetByIdAsync(newContract.PersonnelNumber ?? 0));
                }

                ModelState.AddModelError(string.Empty, "Невозможно создать контракт так как существует действующий");

                return View();
                // return JavaScript("$('#contents_m').text('Невозможно создать контракт так как существует действующий'); setTimeout(function(){$('#modal_close').click();}, 2000);");
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? contractId)
        {
            if (contractId != null)
            {
                var contract = await _contractService.GetAsync(contractId ?? 0);

                if (contract != null)
                {
                    //  Session[contractId.ToString()] = contract;

                    return PartialView(contract);
                }

                return NotFound();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ContractToEmployee contract)
        {
            if (contract != null && ModelState.IsValid && contract.PersonnelNumber != 0)
            {
                await _contractService.UpdateAsync(contract);

                return PartialView("UpdatingTable", contract);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult> CancelEdit(int? contractId)
        {
            if (contractId != null)
            {
                //if (Session[contractId.ToString()] != null)
                //{
                //    var contract = Session[contractId.ToString()] as ContractToEmployee;

                //    return PartialView("UpdatingTable", contract);
                //}

                var contract = await _contractService.GetAsync(contractId ?? 0);

                return PartialView("UpdatingTable", contract);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? contractId)
        {
            if (contractId != null)
            {
                await _contractService.DeleteAsync(contractId ?? 0);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult> UnLock(int? contractId)
        {
            if (contractId != null)
            {
                if ((await _contractService.UnLockContractAsync(contractId ?? 0)) > 0)
                    return PartialView("UpdatingTable", await _contractService.GetAsync(contractId ?? 0));

                return NotFound();
            }

            return NotFound();
        }

        public ActionResult Search()
        {
            return View(new SearchParmsContractViewModel()
            {
                GridItem = new Domain.Common.GridItem()
                {
                    Data = new List<ContractModel>()
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult> Search(SearchParmsContractViewModel param, int? page)
        {
            if (param != null)
            {
                var searchResults = await _contractService.GeAsync(
                    param.ContractId, param.FullName, param.ContractType, param.Term,
                    param.IsOn, param.DateFrom, param.DateTo, page ?? 1, TakeValue6);

                param.GridItem = searchResults;

                return View(param);
            }

            return BadRequest();
        }
    }
}