using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using System.Linq.Expressions;

namespace CarServiceApp.Controllers
{
    public class CarModificationsController(
        IGenericServiceAsync<CarModification, int> service,
        IGenericServiceAsync<CarBrand, int> serviceForDictionary)
        : CommonEntityWithListController<CarModification, int>(service)
    {
        private readonly IGenericServiceAsync<CarBrand, int> _serviceForDictionary = serviceForDictionary;

        protected override async Task<Dictionary<object, string>> GetDictionaryAsync()
        {
            return await _serviceForDictionary.GetDictionaryAsync();
        }

        protected override Expression<Func<CarModification, bool>> GetFilters(string searchString)
        {
            return x => string.IsNullOrEmpty(searchString)
            || (x.Name.Contains(searchString)
            || x.Series.Name.Contains(searchString)
            || x.Series.CarModel.Name.Contains(searchString)
            || x.Series.CarModel.CarBrand.Name.Contains(searchString));
        }

        protected override Expression<Func<CarModification, object>>[] GetIncludeInfo()
        {
            return
            [
                x => x.Series,
                x => x.Series.CarModel,
                x => x.Series.CarModel.CarBrand,
            ];
        }
    }
}