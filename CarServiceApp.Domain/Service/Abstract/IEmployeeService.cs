using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IEmployeeService
    {
        //"mngparts"
        SelectList Employees(
            bool usePostFilter = true,
            int postId = 5,
            bool userDefaultValue = false);

        Task<Dictionary<object, string>> GetDictionaryAsync();

        Task<SelectList> GetSelectListAsync();

        Task<Employee> GetByIdAsync(int id);

        Task CreateAsync(Employee entity);

        Task UpdateAsync(Employee entity);

        Task<List<string>> GetAsync(string id);

        Task<List<string>> GetByNameAsync(string name);

        Task<GridItem> GetAsync(
            string id,
            string name,
            int page,
            int pageSize);

        Task DeleteAsync(Employee entity);
    }
}