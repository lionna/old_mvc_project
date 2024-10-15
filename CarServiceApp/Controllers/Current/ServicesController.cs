using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace CarServiceApp.Controllers
{
    //SysAdmin, Manager, Accountant
    public class ServicesController(
        IGenericServiceAsync<Service, string> service,
        IGenericServiceAsync<ServiceType, int> serviceForDictionary)
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
        : CommonEntityWithListController<Service, string>(service: service)
#pragma warning restore CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    {
        private readonly IGenericServiceAsync<ServiceType, int> _serviceForDictionary = serviceForDictionary;

        protected override async Task<Dictionary<object, string>> GetDictionaryAsync()
        {
            return await _serviceForDictionary.GetDictionaryAsync();
        }

        protected override Expression<Func<Service, bool>> GetFilters(string searchString)
        {
            return x => string.IsNullOrEmpty(searchString)
            || (x.Name.Contains(searchString) || x.ServiceType.Name.Contains(searchString));
        }

        protected override Expression<Func<Service, object>>[] GetIncludeInfo()
        {
            return
            [
                x => x.ServiceType
            ];
        }

        [HttpDelete]
        public async Task<IActionResult> Activate(string id)
        {
            try
            {
                var entity = await service.GetByIdAsync(id);
                entity.Available = !entity.Available;

                await service.UpdateAsync(entity);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Проблема активации или диактивации.");
                return Json(new { isNotSuccess = true, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}