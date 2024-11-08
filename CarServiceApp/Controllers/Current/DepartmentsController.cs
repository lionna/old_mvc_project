using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;

namespace CarServiceApp.Controllers
{
    public class DepartmentsController(IGenericServiceAsync<Department, int> service)
       : CommonEntityController<Department, int>(service)
    {
    }
}