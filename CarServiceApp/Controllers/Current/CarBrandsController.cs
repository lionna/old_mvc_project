using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;

namespace CarServiceApp.Controllers
{
    public class CarBrandsController(IGenericServiceAsync<CarBrand, int> service)
        : CommonEntityController<CarBrand, int>(service)
    {
    }
}