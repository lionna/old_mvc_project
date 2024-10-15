using CarServiceApp.Controllers.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;

namespace CarServiceApp.Controllers
{
    public class PostsController(IGenericServiceAsync<Post, int> service)
           : CommonEntityController<Post, int>(service)
    {
    }
}