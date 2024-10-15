using CarServiceApp.AuthManagement.Enums;
using CarServiceApp.AuthManagement.Model;
using CarServiceApp.Domain.Extension;
using CarServiceApp.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    public class AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager) : Controller
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;

        private readonly SignInManager<IdentityUser> _signInManager = signInManager;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> UserSettings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserSettingsViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber
            };

            ViewBag.ModelMessage = string.Empty;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserSettings(UserSettingsViewModel model)
        {
            ViewBag.ModelMessage = string.Empty;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;

            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            await _signInManager.SignOutAsync();

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Ошибка входа.");
                return View(model);
            }

            ViewBag.ModelMessage = "Данные успешно изменены";

            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(UserModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.UserName);

        //    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var roles = await _userManager.GetRolesAsync(user);

        //        if (roles.Contains(UserRoles.SysAdmin.ToString()))
        //        {
        //            return RedirectToAction("SysAdmin", "Account");
        //        }
        //        if (roles.Contains(UserRoles.Manager.ToString()))
        //        {
        //            return RedirectToAction("Manager", "Account");
        //        }
        //        if (roles.Contains(UserRoles.Employee.ToString()))
        //        {
        //            return RedirectToAction("Employee", "Account");
        //        }
        //        if (roles.Contains(UserRoles.Client.ToString()))
        //        {
        //            return RedirectToAction("Client", "Account");
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }

        //    ModelState.AddModelError(string.Empty, "Не верный логин или пароль");
        //    return View(model);
        //}

        [HttpPost]
        public async Task<IActionResult> Login(UserModel model, string returnUrl = null)
        {
            //var user = await _userManager.FindByNameAsync(model.UserName);
            //var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, lockoutOnFailure: false);

            //if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            //{
            //    // Получаем словарь ролей и соответствующих действий
            //    var roleActions = EnumExtensions.ToDictionary<UserRoles>();

            //    // Проверяем, есть ли у пользователя какая-либо из ролей
            //    foreach (var role in roleActions)
            //    {
            //        if (roleActions.ContainsValue(role.Value))
            //        {
            //            // Добавляем клейм с ролью
            //            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role.Value));

            //            return RedirectToAction(role.Value, "Account");
            //        }
            //    }

            //    // Если ни одной из ролей не найдено, перенаправляем на домашнюю страницу
            //    return RedirectToAction("Index", "Home");
            //}

            //ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
            //return View(model);

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Получаем пользователя по email
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    // Получаем список ролей пользователя
                    var roles = await _userManager.GetRolesAsync(user);

                    // Вход пользователя
                    await _signInManager.SignInAsync(user, isPersistent: true);

                    // Получаем словарь ролей и соответствующих действий
                    var rolesItems = EnumExtensions.ToDictionary<UserRoles>();

                    var role = roles.FirstOrDefault();
                    if (rolesItems.ContainsValue(role))
                    {
                        return RedirectToAction(role, "Account");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Проблема входа. Проверьте введенные данные");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Проблема входа. Проверьте введенные данные");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult SysAdmin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Manager()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Accountant()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Employee()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Client()
        {
            return View();
        }
    }
}