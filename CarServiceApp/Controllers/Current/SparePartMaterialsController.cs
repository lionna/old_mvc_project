using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;
using System.Linq.Expressions;

namespace CarServiceApp.Controllers
{
    //SysAdmin, Manager
    public class SparePartMaterialsController(IGenericServiceAsync<SparePartMaterial, string> service)
        : CommonEntityController<SparePartMaterial, string>(service)
    {
        protected override Expression<Func<SparePartMaterial, bool>> GetFilters(string searchString)
        {
            return x => string.IsNullOrEmpty(searchString)
            || (x.Name.Contains(searchString)
            || x.Unit.Contains(searchString)
            || x.Manufacture.Contains(searchString));
        }
    }
}