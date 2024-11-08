using CarServiceApp.AuthManagement.Model;
using CarServiceApp.AuthManagement.Service.Abstract;
using CarServiceApp.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    /// <summary>
    /// Контроллер, обрабатывающий операции, связанные с ролями пользователей.
    /// </summary>
    public class RolesController(IRoleService roleService) : Controller
    {
        /// <summary>
        /// Возвращает представление со списком ролей.
        /// </summary>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sortOrder">Порядок сортировки</param>
        /// <param name="searchString">Строка для поиска по названию роли</param>
        /// <returns>Представление со списком ролей.</returns>
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 5,
            string sortOrder = "",
            string searchString = "")
        {
            // Получение списка ролей.
            var roles = roleService.Get();

            // Фильтрация по строке поиска, если она указана.
            if (!string.IsNullOrEmpty(searchString))
            {
                roles = roles.Where(r => r.Name.Contains(searchString));
            }

            // Сортировка в соответствии с параметрами sortOrder.
            roles = sortOrder switch
            {
                "NameDesc" => roles.OrderByDescending(r => r.Name),
                _ => roles.OrderBy(r => r.Id),
            };

            // Получение данных для текущей страницы с использованием CommonRepositoryAsync.
            var model = await CommonRepositoryAsync.GetAllWithPagesAsync(roles, page, pageSize);

            // Передача данных в представление.
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
            ViewData["CurrentFilter"] = searchString;

            return View(model);
        }

        /// <summary>
        /// Возвращает представление для создания новой роли.
        /// </summary>
        /// <returns>Представление для создания роли.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Обрабатывает POST-запрос для создания новой роли.
        /// </summary>
        /// <param name="model">Модель представления роли.</param>
        /// <returns>Редирект на страницу со списком ролей или возвращает представление с ошибкой.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] RoleViewModel model)
        {
            // Проверка валидности модели перед созданием роли.
            if (ModelState.IsValid)
            {
                // Создание роли с использованием сервиса ролей.
                var result = await roleService.CreateAsync(model.Name);

                // Перенаправление на страницу со списком ролей в случае успеха.
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }

                // В случае ошибки добавление сообщения об ошибке в ModelState.
                ModelState.AddModelError(string.Empty, "Ошибка при создании роли.");
            }

            return View(model);
        }

        /// <summary>
        /// Обрабатывает запрос на удаление роли.
        /// </summary>
        /// <param name="id">Идентификатор роли.</param>
        /// <returns>Редирект на страницу со списком ролей или возвращает JSON с ошибкой удаления.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            // Удаление роли с использованием сервиса ролей.
            var result = await roleService.DeleteAsync(id);

            // В случае ошибки возвращает JSON с информацией об ошибке удаления.
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Нельзя удалять роль, у которой есть пользователи.");
                return Json(new { isNotSuccess = true, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            // В случае успеха перенаправление на страницу со списком ролей.
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Возвращает представление для редактирования роли.
        /// </summary>
        /// <param name="id">Идентификатор роли.</param>
        /// <returns>Представление для редактирования роли.</returns>
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            // Получение роли по идентификатору с использованием сервиса ролей.
            var role = await roleService.GetByIdAsync(id);

            // Проверка наличия роли. В случае отсутствия - возврат NotFound.
            if (role == null)
            {
                return NotFound();
            }

            // Возвращает представление с моделью роли для редактирования.
            return View(new RoleViewModel { Name = role.Name, Id = role.Id });
        }

        /// <summary>
        /// Обрабатывает POST-запрос для редактирования роли.
        /// </summary>
        /// <param name="id">Идентификатор роли.</param>
        /// <param name="role">Модель представления роли.</param>
        /// <returns>Редирект на страницу со списком ролей или возвращает представление с ошибкой.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] RoleViewModel role)
        {
            // Проверка соответствия идентификаторов роли.
            if (id != role.Id)
            {
                return NotFound();
            }

            // Проверка валидности модели перед обновлением роли.
            if (ModelState.IsValid)
            {
                // Обновление роли с использованием сервиса ролей.
                var result = await roleService.UpdateAsync(role.Id, role.Name);

                // Перенаправление на страницу со списком ролей в случае успеха.
                if (result)
                {
                    return RedirectToAction("Index");
                }

                // В случае ошибки добавление сообщения об ошибке в ModelState.
                ModelState.AddModelError(string.Empty, "Ошибка при обновлении роли.");
            }

            return View(role);
        }
    }
}

//using CarServiceApp.Domain.Repository;
//using CarServiceApp.Domain.Service.Abstract;
//using CarServiceApp.Models.User;
//using Microsoft.AspNetCore.Mvc;

//namespace CarServiceApp.Controllers
//{
//    /// <summary>
//    /// Контроллер, обрабатывающий операции, связанные с ролями пользователей.
//    /// </summary>
//    public class RoleController(IRoleService roleService) : Controller
//    {
//        public async Task<IActionResult> Index(int page = 1, int pageSize = 5, string sortOrder = "", string searchString = "")
//        {
//            var roles = roleService.GetRoles();

//            if (!string.IsNullOrEmpty(searchString))
//            {
//                roles = roles.Where(r => r.Name.Contains(searchString));
//            }

//            roles = sortOrder switch
//            {
//                "NameDesc" => roles.OrderByDescending(r => r.Name),
//                _ => roles.OrderBy(r => r.Id),
//            };

//            var model = await CommonRepositoryAsync.GetAllWithPageAsync(roles, page, pageSize);

//            ViewData["CurrentSort"] = sortOrder;
//            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
//            ViewData["CurrentFilter"] = searchString;

//            return View(model);
//        }

//        // GET: Role/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Role/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Name")] RoleViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var result = await roleService.CreateRoleAsync(model.Name);

//                if (result)
//                {
//                    return RedirectToAction(nameof(Index));
//                }

//                ModelState.AddModelError(string.Empty, "Ошибка при создании роли.");
//            }

//            return View(model);
//        }

//        [HttpDelete]
//        public async Task<IActionResult> Delete(string id)
//        {
//            var result = await roleService.DeleteRoleAsync(id);

//            if (!result)
//            {
//                ModelState.AddModelError(string.Empty, "Нельзя удалять роль, у которой есть пользователи.");

//                return Json(new { isNotSuccess = true , errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        public async Task<IActionResult> Edit(string id)
//        {
//            var role = await roleService.GetRoleByIdAsync(id);

//            if (role == null)
//            {
//                return NotFound();
//            }

//            return View(new RoleViewModel { Name = role.Name, Id = role.Id });
//        }

//        [HttpPost]
//        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] RoleViewModel role)
//        {
//            if (id != role.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                var result = await roleService.UpdateRoleAsync(role.Id, role.Name);

//                if (result)
//                {
//                    return RedirectToAction("Index");
//                }

//                ModelState.AddModelError(string.Empty, "Ошибка при обновлении роли.");
//            }

//            return View(role);
//        }
//    }
//}

//using CarServiceApp.Domain.Repository;
//using CarServiceApp.Models.User;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;

//namespace CarServiceApp.Controllers;

//public class RoleController : Controller
//{
//    private readonly RoleManager<IdentityRole> _roleManager;
//    private readonly UserManager<IdentityUser> _userManager;

//    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
//    {
//        _roleManager = roleManager;
//        _userManager = userManager;
//    }

//    public async Task<IActionResult> Index(int page = 1, int pageSize = 5, string sortOrder = "", string searchString = "")
//    {
//        var roles = _roleManager.Roles;

//        if (!string.IsNullOrEmpty(searchString))
//        {
//            roles = roles.Where(r => r.Name.Contains(searchString));
//        }

//        roles = sortOrder switch
//        {
//            "NameDesc" => roles.OrderByDescending(r => r.Name),
//            _ => roles.OrderBy(r => r.Id),
//        };

//        var model = await CommonRepositoryAsync.GetAllWithPageAsync(roles, page, pageSize);

//        ViewData["CurrentSort"] = sortOrder;
//        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
//        ViewData["CurrentFilter"] = searchString;

//        return View(model);
//    }

//    // GET: Role/Create
//    public IActionResult Create()
//    {
//        return View();
//    }

//    // POST: Role/Create
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Create([Bind("Name")] RoleViewModel model)
//    {
//        if (ModelState.IsValid)
//        {
//            var role = new IdentityRole { Name = model.Name };

//            var result = await _roleManager.CreateAsync(role);

//            if (result.Succeeded)
//            {
//                return RedirectToAction(nameof(Index));
//            }

//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//            }
//        }
//        else
//        {
//            foreach (var item in ModelState.Values)
//                ModelState.AddModelError(string.Empty, item.AttemptedValue);
//        }

//        return View(model);
//    }

//    [HttpDelete]
//    public async Task<IActionResult> Delete(string id)
//    {
//        var role = await _roleManager.FindByIdAsync(id);

//        if (role != null)
//        {
//            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

//            if (usersInRole.Any())
//            {
//                ModelState.AddModelError(string.Empty, "Нельзя удалять роль, у которой есть пользователи.");
//                return RedirectToAction(nameof(Index));
//            }

//            var result = await _roleManager.DeleteAsync(role);

//            if (result.Succeeded)
//            {
//                return RedirectToAction(nameof(Index));
//            }

//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//            }
//        }

//        return RedirectToAction(nameof(Index));
//    }

//    public async Task<IActionResult> Edit(string id)
//    {
//        if (string.IsNullOrWhiteSpace(id))
//        {
//            return NotFound();
//        }

//        var role = await _roleManager.FindByIdAsync(id);

//        if (role == null)
//        {
//            return NotFound();
//        }

//        return View(new RoleViewModel {Name = role.Name, Id = role.Id});
//    }

//    [HttpPost]
//    public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] RoleViewModel role)
//    {
//        if (id != role.Id)
//        {
//            return NotFound();
//        }

//        if (ModelState.IsValid)
//        {
//            var existingRole = await _roleManager.FindByIdAsync(id);
//            if (existingRole != null)
//            {
//                existingRole.Name = role.Name;

//                var result = await _roleManager.UpdateAsync(existingRole);

//                if (result.Succeeded)
//                {
//                    return RedirectToAction("Index");
//                }

//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                }
//            }
//        }

//        return View(role);
//    }
//}