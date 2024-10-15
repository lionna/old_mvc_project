using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace CarServiceApp.Controllers
{
    public class CarGenerationsController(
       IGenericServiceAsync<CarGeneration, int> service,
       IGenericServiceAsync<CarModel, int> serviceForDictionary)
       : CommonEntityWithListController<CarGeneration, int>(service)
    {
        private readonly IGenericServiceAsync<CarModel, int> _serviceForDictionary = serviceForDictionary;

        protected override async Task<Dictionary<object, string>> GetDictionaryAsync()
        {
            return await _serviceForDictionary.GetDictionaryAsync();
        }

        protected override Expression<Func<CarGeneration, bool>> GetFilters(string searchString)
        {
            return x => string.IsNullOrEmpty(searchString)
            || (x.Name.Contains(searchString)
            || x.CarModel.Name.Contains(searchString));
        }

        protected override Expression<Func<CarGeneration, object>>[] GetIncludeInfo()
        {
            return
            [
                x => x.CarModel
            ];
        }

        public IActionResult GetYearsJson()
        {
            var currentYear = DateTime.Now.Year;
            var startYear = 1920;

            var years = new List<int>();
            for (int year = startYear; year <= currentYear; year++)
            {
                years.Add(year);
            }

            return Json(new { YearBegin = startYear, YearEnd = currentYear });
        }
    }
}