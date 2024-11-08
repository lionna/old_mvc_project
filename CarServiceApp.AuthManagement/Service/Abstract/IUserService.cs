using CarServiceApp.AuthManagement.Model;
using Microsoft.AspNetCore.Identity;

namespace CarServiceApp.AuthManagement.Service.Abstract
{
    public interface IUserService
    {
        IQueryable<IdentityUser> Get(int page, int pageSize, string sortOrder, string searchString);
        Task<IdentityResult> CreateAsync(RegisterViewModel model);
        Task<EditUserViewModel> GetForEditAsync(string id);
        Task<IdentityResult> EditAsync(string id, EditUserViewModel model);
        Task<IdentityResult> DeleteAsync(string id);
    }
}
