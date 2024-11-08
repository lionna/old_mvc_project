using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;

namespace CarServiceApp.Controllers
{
    public class StreetsController(IGenericServiceAsync<ImportedStreet, int> service)
        : CommonEntityController<ImportedStreet, int>(service)
    {
    }
}