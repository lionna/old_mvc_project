using CarServiceApp.Domain.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IClientService
    {
        Task<SelectList> ClientsAsync(int? takeValue);

        Task<IEnumerable<Entity.Models.Client>> SearchClientsAsync(string id, string name, int? valueTake);

        Task<Dictionary<object, string>> GetDictionaryAsync();

        Task<SelectList> GetSelectListAsync();

        Task<Entity.Models.Client> GetAsync(string id);

        Task<List<string>> GetByIdAsync(string id, int takeValue);

        Task<List<string>> GetByNameAsync(string name, int takeValue);

        Task<string> CreateAsync(Entity.Models.Client entity);

        Task UpdateAsync(Entity.Models.Client entity);

        Task<GridItem> GetAsync(
            string id,
            string name,
            int page,
            int pageSize);

        Task DeleteAsync(Entity.Models.Client clinet);
    }
}