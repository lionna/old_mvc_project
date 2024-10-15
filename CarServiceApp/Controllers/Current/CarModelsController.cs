using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;
using System.Linq.Expressions;

namespace CarServiceApp.Controllers
{
    public class CarModelsController(
        IGenericServiceAsync<CarModel, int> service,
        IGenericServiceAsync<CarBrand, int> serviceForDictionary)
        : CommonEntityWithListController<CarModel, int>(service)
    {
        private readonly IGenericServiceAsync<CarBrand, int> _serviceForDictionary = serviceForDictionary;

        protected override async Task<Dictionary<object, string>> GetDictionaryAsync()
        {
            return await _serviceForDictionary.GetDictionaryAsync();
        }

        protected override Expression<Func<CarModel, bool>> GetFilters(string searchString)
        {
            return x => string.IsNullOrEmpty(searchString) || (x.Name.Contains(searchString) || x.CarBrand.Name.Contains(searchString));
        }

        protected override Expression<Func<CarModel, object>>[] GetIncludeInfo()
        {
            return
            [
                x => x.CarBrand
            ];
        }
    }
}