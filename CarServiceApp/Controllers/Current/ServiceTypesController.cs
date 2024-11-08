using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;

namespace CarServiceApp.Controllers
{
    public class ServiceTypesController(IGenericServiceAsync<ServiceType, int> service)
      : CommonEntityController<ServiceType, int>(service)
    {
    }
}