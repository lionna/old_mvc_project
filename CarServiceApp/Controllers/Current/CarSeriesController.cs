using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;
using System.Linq.Expressions;

namespace CarServiceApp.Controllers
{
    public class CarSeriesController(
    IGenericServiceAsync<CarSeries, int> service,
    IGenericServiceAsync<CarModel, int> serviceForDictionary)
        : CommonEntityWithListController<CarSeries, int>(service)
    {
        private readonly IGenericServiceAsync<CarModel, int> _serviceForDictionary = serviceForDictionary;

        protected override async Task<Dictionary<object, string>> GetDictionaryAsync()
        {
            return await _serviceForDictionary.GetDictionaryAsync();
        }

        protected override Expression<Func<CarSeries, bool>> GetFilters(string searchString)
        {
            return x => string.IsNullOrEmpty(searchString) || (x.Name.Contains(searchString) || x.CarModel.Name.Contains(searchString));
        }

        protected override Expression<Func<CarSeries, object>>[] GetIncludeInfo()
        {
            return
            [
                x => x.CarModel
            ];
        }
    }
}