using Microsoft.AspNetCore.Identity;

namespace CarServiceApp.AuthManagement.Service.Abstract
{
    public interface IRoleService
    {
        IQueryable<IdentityRole> Get();
        Task<bool> CreateAsync(string name);
        Task<bool> DeleteAsync(string id);
        Task<IdentityRole> GetByIdAsync(string id);
        Task<bool> UpdateAsync(string id, string name);
        Dictionary<string, string> GetRoles();
    }
}
