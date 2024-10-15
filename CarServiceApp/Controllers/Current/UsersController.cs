using CarServiceApp.AuthManagement.Model;
using CarServiceApp.AuthManagement.Service.Abstract;
using CarServiceApp.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями
    /// </summary>
    /// <param name="userService">Сервис пользователей</param>
    /// <param name="roleService">Сервис ролей</param>
    public class UsersController(IUserService userService, IRoleService roleService)
        : Controller
    {
        /// <summary>
        /// Действие для отображения списка пользователей
        /// </summary>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sortOrder">Порядок сортировки</param>
        /// <param name="searchString">Строка поиска</param>
        /// <returns>Представление со списком пользователей</returns>
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 5,
            string sortOrder = "",
            string searchString = "")
        {
            var users = userService.Get(page, pageSize, sortOrder, searchString);
            var model = await CommonRepositoryAsync.GetAllWithPagesAsync(users, page, pageSize);

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
            ViewData["CurrentFilter"] = searchString;

            return View(model);
        }

        /// <summary>
        /// Действие для отображения формы создания нового пользователя.
        /// </summary>
        /// <returns>Представление с формой создания пользователя.</returns>
        public IActionResult Create()
        {
            var model = new RegisterViewModel
            {
                AvailableRoles = roleService.GetRoles()
            };
            return View(model);
        }

        /// <summary>
        /// HTTP POST-запрос для создания нового пользователя.
        /// </summary>
        /// <param name="model">Модель представления для регистрации пользователя.</param>
        /// <returns>Редирект на список пользователей в случае успеха, иначе представление с ошибками.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            model.AvailableRoles = roleService.GetRoles();

            if (ModelState.IsValid)
            {
                var result = await userService.CreateAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Действие для отображения формы редактирования пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Представление с формой редактирования пользователя.</returns>
        public async Task<IActionResult> Edit(string id)
        {
            var model = await userService.GetForEditAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        /// <summary>
        /// HTTP POST-запрос для редактирования пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="model">Модель представления для редактирования пользователя.</param>
        /// <returns>Редирект на список пользователей в случае успеха, иначе представление с ошибками.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            model.AvailableRoles = roleService.GetRoles();

            if (ModelState.IsValid)
            {
                var result = await userService.EditAsync(id, model);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        /// <summary>
        /// HTTP DELETE-запрос для удаления пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Редирект на список пользователей в случае успеха, иначе JSON с ошибками.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var result = await userService.DeleteAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Ошибка удаления пользователя.");
                return Json(new { isNotSuccess = true, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

//using CarServiceApp.Domain.Model.User;
//using CarServiceApp.Domain.Repository;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//public class UserController : Controller
//{
//    private readonly UserManager<IdentityUser> _userManager;
//    private readonly RoleManager<IdentityRole> _roleManager;

//    public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
//    {
//        _userManager = userManager;
//        _roleManager = roleManager;
//    }

//    public async Task<IActionResult> Index(int page = 1, int pageSize = 5, string sortOrder = "", string searchString = "")
//    {
//        var users = _userManager.Users;

//        if (!string.IsNullOrEmpty(searchString))
//        {
//            users = users.Where(r => r.UserName.Contains(searchString));
//        }

//        users = sortOrder switch
//        {
//            "NameDesc" => users.OrderByDescending(r => r.UserName),
//            _ => users.OrderBy(r => r.Id),
//        };

//        var model = await CommonRepositoryAsync.GetAllWithPageAsync(users, page, pageSize);

//        ViewData["CurrentSort"] = sortOrder;
//        ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
//        ViewData["CurrentFilter"] = searchString;

//        return View(model);
//    }

//    // GET: User/Create
//    public IActionResult Create()
//    {
//        var model = new RegisterViewModel
//        {
//            AvailableRoles = _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToDictionary(r => r.Id, r => r.Name)
//        };
//        return View(model);
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Create(RegisterViewModel model)
//    {
//        model.AvailableRoles = _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToDictionary(r => r.Id, r => r.Name);

//        if (ModelState.IsValid)
//        {
//            var user = new IdentityUser { UserName = model.UserName, Email = model.Email, EmailConfirmed = true };
//            var result = await _userManager.CreateAsync(user, model.Password);

//            if (result.Succeeded)
//            {
//                // Добавление пользователя к выбранной роли
//                if (!string.IsNullOrEmpty(model.Role))
//                {
//                    var role = GetRole(model.Role);
//                    await _userManager.AddToRoleAsync(user, role);
//                }

//                return RedirectToAction(nameof(Index));
//            }

//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//            }
//        }

//        return View(model);
//    }

//    private string GetRole(string input)
//    {
//        if (string.IsNullOrWhiteSpace(input))
//        {
//            return string.Empty;
//        }

//        // Разделение строки по запятой
//        string[] parts = input.Split(',');

//        return parts[1]?.Trim() ?? string.Empty;
//    }

//    public async Task<IActionResult> Edit(string id)
//    {
//        if (string.IsNullOrWhiteSpace(id))
//        {
//            return NotFound();
//        }

//        var user = await _userManager.FindByIdAsync(id);

//        if (user == null)
//        {
//            return NotFound();
//        }

//        var model = new EditUserViewModel
//        {
//            Id = user.Id,
//            UserName = user.UserName,
//            Email = user.Email,
//            AvailableRoles = _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToDictionary(r => r.Id, r => r.Name)
//        };

//        return View(model);
//    }

//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Edit(string id, EditUserViewModel model)
//    {
//        if (id != model.Id)
//        {
//            return NotFound();
//        }

//        model.AvailableRoles = _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToDictionary(r => r.Id, r => r.Name);

//        if (ModelState.IsValid)
//        {
//            var user = await _userManager.FindByIdAsync(id);

//            if (user == null)
//            {
//                return NotFound();
//            }

//            // Удаление текущих ролей пользователя
//            var userRoles = await _userManager.GetRolesAsync(user);
//            await _userManager.RemoveFromRolesAsync(user, userRoles);

//            var role = GetRole(model.Role);
//            // Добавление новых ролей
//            await _userManager.AddToRolesAsync(user, new List<string> { role });

//            // Обновление остальных свойств пользователя, если необходимо

//            var result = await _userManager.UpdateAsync(user);

//            if (result.Succeeded)
//            {
//                return RedirectToAction(nameof(Index));
//            }

//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//            }
//        }

//        return View(model);
//    }

//    [HttpDelete]
//    public async Task<IActionResult> Delete(string id)
//    {
//        if (string.IsNullOrWhiteSpace(id))
//        {
//            return NotFound();
//        }

//        var user = await _userManager.FindByIdAsync(id);
//        if (user == null)
//        {
//            return NotFound();
//        }

//        var result = await _userManager.DeleteAsync(user);
//        if (!result.Succeeded)
//        {
//            ModelState.AddModelError(string.Empty, "Ошибка удаления пользователя.");
//            return Json(new { isNotSuccess = true, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
//        }

//        return RedirectToAction(nameof(Index));
//    }
//}