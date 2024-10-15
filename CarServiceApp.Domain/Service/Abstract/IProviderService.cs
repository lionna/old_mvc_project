using CarServiceApp.Domain.Entity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IProviderService
    {
        Task<SelectList> ProvidersAsync();

        Task<SelectList> ProvidersSearchAsync(int? takeValue = null, bool addDefaultValue = true);

        Task<IEnumerable<Provider>> ProvidersAsync(string id, string name, int? takeValue);
    }
}