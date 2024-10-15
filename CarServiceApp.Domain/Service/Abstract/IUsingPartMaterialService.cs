using CarServiceApp.Domain.Entity.Models;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IUsingPartMaterialService
    {
        Task<List<UsingPartMaterial>> UsingPartMaterialsAsync(int? positionId);
    }
}