using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;

namespace CarServiceApp.Controllers
{
    public class TypeOfPaymentsController(IGenericServiceAsync<TypeOfPayment, int> service)
     : CommonEntityController<TypeOfPayment, int>(service)
    {
    }
}