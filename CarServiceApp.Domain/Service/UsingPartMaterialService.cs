using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApp.Domain.Service
{
    public class UsingPartMaterialService(
        ApplicationDbContext context) : IUsingPartMaterialService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<List<UsingPartMaterial>> UsingPartMaterialsAsync(int? positionId)
        {
            var query = _context.Set<UsingPartMaterial>().Where(s => s.PositionId == positionId);

            return await query.ToListAsync();
        }
    }
}