using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;
using System.Linq.Expressions;

namespace CarServiceApp.Controllers
{
    public class CarColorsController(IGenericServiceAsync<CarColor, int> service)
        : CommonEntityController<CarColor, int>(service)
    {
        protected override Expression<Func<CarColor, bool>> GetFilters(string searchString)
        {
            return x => string.IsNullOrEmpty(searchString) || (x.Name.Contains(searchString) || x.Hex.Contains(searchString));
        }
    }
}