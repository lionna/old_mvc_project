using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApp.Domain.Service
{
    public class SparePartMaterialService(
        ApplicationDbContext context,
        IGenericRepositoryAsync<SparePartMaterial, string> sparePartMaterialRepository) : ISparePartMaterialService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly IGenericRepositoryAsync<SparePartMaterial, string> _sparePartMaterialRepository = sparePartMaterialRepository ?? throw new ArgumentNullException(nameof(sparePartMaterialRepository));

        public async Task<List<AcceptancedSparePart>> GetAcceptancedSparePartsAsync(int lotNumber)
        {
            var totalMoney = _context.AcceptanceInvoice
                .Where(ai => ai.LotNumber == lotNumber)
                .Sum(ai => ((double)(ai.Price ?? 0) * (ai.Number ?? 0)));

            var acceptedSpareParts = await (from ai in _context.AcceptanceInvoice
                                            join sp in _context.SparePartMaterial
                                            on ai.PartId equals sp.Id
                                            where ai.LotNumber == lotNumber
                                            select new AcceptancedSparePart
                                            {
                                                LotNumber = ai.LotNumber,
                                                PartId = sp.Id,
                                                Number = ai.Number,
                                                Price = ai.Price,
                                                Name = sp.Name,
                                                Unit = sp.Unit,
                                                Manufacture = sp.Manufacture,
                                                PositionId = ai.PositionId,
                                                Total = totalMoney
                                            }).ToListAsync();

            return acceptedSpareParts;
        }

        public async Task<List<SparePartMaterial>> GetAsync(string id, string name, int takeValue)
        {
            var query = _context.Set<SparePartMaterial>()
                .Where(s => s.Id.Contains(id) && s.Name.Contains(name))
                .Take(takeValue);

            return await query.ToListAsync();
        }

        public async Task<List<SparePartMaterial>> GetAsync(string id)
        {
            var query = (await _sparePartMaterialRepository.GetAllAsync()).Where(s => s.Id == id);

            return query.ToList();
        }

        public async Task<List<SparePartMaterial>> GetAsync()
        {
            return (await _sparePartMaterialRepository.GetAllAsync()).ToList();
        }
    }
}