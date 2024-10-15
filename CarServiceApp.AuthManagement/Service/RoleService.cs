using CarServiceApp.AuthManagement.Service.Abstract;
using Microsoft.AspNetCore.Identity;

namespace CarServiceApp.AuthManagement.Service
{
    public class RoleService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) : IRoleService
    {
        public IQueryable<IdentityRole> Get()
        {
            return roleManager.Roles;
        }

        public async Task<bool> CreateAsync(string name)
        {
            var role = new IdentityRole { Name = name };
            var result = await roleManager.CreateAsync(role);

            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
                if (usersInRole.Any())
                {
                    return false; // Нельзя удалять роль, у которой есть пользователи.
                }

                var result = await roleManager.DeleteAsync(role);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<IdentityRole> GetByIdAsync(string id)
        {
            return await roleManager.FindByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(string id, string name)
        {
            var existingRole = await roleManager.FindByIdAsync(id);
            if (existingRole != null)
            {
                existingRole.Name = name;

                var result = await roleManager.UpdateAsync(existingRole);
                return result.Succeeded;
            }

            return false;
        }

        public Dictionary<string, string> GetRoles()
        {
            return Get().Select(r => new { r.Id, r.Name }).ToDictionary(r => r.Id, r => r.Name);
        }
    }
}
