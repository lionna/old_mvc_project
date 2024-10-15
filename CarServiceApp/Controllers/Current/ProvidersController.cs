using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;
using System.Linq.Expressions;

namespace CarServiceApp.Controllers
{
    //SysAdmin,Manager,Accountant
    public class ProvidersController(IGenericServiceAsync<Provider, string> service)
        : CommonEntityController<Provider, string>(service)
    {
        protected override Expression<Func<Provider, bool>> GetFilters(string searchString)
        {
            return x => string.IsNullOrEmpty(searchString)
            || x.Name.Contains(searchString)
            || x.Address.Contains(searchString)
            || x.Email.Contains(searchString)
            || x.Phone.Contains(searchString)
            || x.CertificateNumber.Contains(searchString);
        }
    }
}