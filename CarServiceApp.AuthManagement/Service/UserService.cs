using CarServiceApp.AuthManagement.Model;
using CarServiceApp.AuthManagement.Service.Abstract;
using Microsoft.AspNetCore.Identity;

namespace CarServiceApp.AuthManagement.Service
{
    public class UserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        : IUserService
    {
        public IQueryable<IdentityUser> Get(int page, int pageSize, string sortOrder, string searchString)
        {
            var users = userManager.Users;

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(r => r.UserName.Contains(searchString));
            }

            users = sortOrder switch
            {
                "NameDesc" => users.OrderByDescending(r => r.UserName),
                _ => users.OrderBy(r => r.Id),
            };

            return users;
        }

        public async Task<IdentityResult> CreateAsync(RegisterViewModel model)
        {
            var user = new IdentityUser {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumber = model.Phone,
                PhoneNumberConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded && !string.IsNullOrEmpty(model.Role))
            {
                var role = GetRole(model.Role);
                await userManager.AddToRoleAsync(user, role);
            }

            return result;
        }

        public async Task<EditUserViewModel> GetForEditAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            return new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                AvailableRoles = roleManager.Roles.Select(r => new { r.Id, r.Name }).ToDictionary(r => r.Id, r => r.Name)
            };
        }

        public async Task<IdentityResult> EditAsync(string id, EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);

            var role = GetRole(model.Role);
            await userManager.AddToRolesAsync(user, [role]);

            user.PhoneNumber = model.Phone;
            user.Email = model.Email;
            user.UserName = model.UserName;

            var result = await userManager.UpdateAsync(user);

            return result;
        }

        public async Task<IdentityResult> DeleteAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            var result = await userManager.DeleteAsync(user);

            return result;
        }

        private static string GetRole(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            string[] parts = input.Split(',');

            if (parts.Length >= 2) // Check if there are at least two elements in the array
            {
                string modifiedString = parts[1]?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(modifiedString))
                {
                    modifiedString = modifiedString[..^1];
                }

                return modifiedString;
            }

            return string.Empty; // Return an empty string if there are not enough elements
        }

    }
}
