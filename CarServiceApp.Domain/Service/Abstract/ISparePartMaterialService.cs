using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface ISparePartMaterialService
    {
        Task<List<SparePartMaterial>> GetAsync(string id, string name, int takeValue);

        Task<List<AcceptancedSparePart>> GetAcceptancedSparePartsAsync(int lotNumber);
    }
}