using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Extension;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarServiceApp.Domain.Service
{
    public class ClientService(ApplicationDbContext context) : IClientService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task DeleteAsync(Entity.Models.Client clinet)
        {
            _context.Set<Entity.Models.Client>().Remove(clinet);
            await _context.SaveChangesAsync();
        }

        private static Expression<Func<Entity.Models.Client, object>>[] GetInfo()
        {
            return
            [
                x => x.AcceptanceDocuments,
                x => x.OrderServices,
            ];
        }

        public async Task<GridItem> GetAsync(
            string id,
            string name,
            int page,
            int pageSize)
        {
            var query = _context.Set<Entity.Models.Client>().AsQueryable();

            query = !string.IsNullOrWhiteSpace(name)
                 ? query.Where(s => s.FullName.Contains(name))
                 : query.Where(s => s.ClientId.Contains(id));

            var includes = GetInfo();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var offset = (page - 1) * pageSize;
            var data = await query.Skip(offset).Take(pageSize).ToListAsync();

            return new GridItem
            {
                Data = data,
                TotalItems = query.Count(),
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
        }

        public async Task<Dictionary<object, string>> GetDictionaryAsync()
        {
            var entities = await _context.Set<Entity.Models.Client>().ToListAsync();
            return entities.ToDictionary(entity => (object)entity.ClientId, entity => entity.FullName);
        }

        public async Task<SelectList> GetSelectListAsync()
        {
            var entities = await GetDictionaryAsync();
            return entities.ToSelectList("Value", "Text");
        }

        public async Task<Entity.Models.Client> GetAsync(string id)
        {
            return await _context.Set<Entity.Models.Client>().FirstOrDefaultAsync(s => s.ClientId.Contains(id));
        }

        public async Task<List<string>> GetByIdAsync(string id, int takeValue)
        {
            return (await _context.Set<Entity.Models.Client>().Where(s => s.ClientId.Contains(id)).Take(takeValue).ToListAsync())
                .Select(a => a.ClientId).Distinct().ToList();
        }

        public async Task<List<string>> GetByNameAsync(string name, int takeValue)
        {
            return (await _context.Set<Entity.Models.Client>().Where(s => s.FullName.Contains(name)).Take(takeValue).ToListAsync())
                .Select(a => a.FullName).Distinct().ToList();
        }

        public async Task<string> CreateAsync(Entity.Models.Client entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            var clientId = "C" + (entity.NumberDriveLicense + DateTime.Now.Millisecond).ToString();
            entity.ClientId = clientId;

            await _context.Set<Entity.Models.Client>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return clientId;
        }

        public async Task UpdateAsync(Entity.Models.Client entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Set<Entity.Models.Client>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<SelectList> ClientsAsync(int? takeValue)
        {
            return new SelectList(await GatAllAsync(takeValue), "Id", "Name");
        }

        public async Task<IEnumerable<Entity.Models.Client>> SearchClientsAsync(string id, string name, int? valueTake)
        {
            if (valueTake != null)
            {
                var query = _context.Set<Entity.Models.Client>()
                .Where(s => s.ClientId.Contains(id) && s.FullName.Contains(name))
                .Take((int)valueTake);

                return await query.ToListAsync();
            }

            return await _context.Set<Entity.Models.Client>()
                .Where(s => s.ClientId.Contains(id) && s.FullName.Contains(name)).ToListAsync();
        }

        private async Task<IEnumerable<Entity.Models.Client>> GatAllAsync(int? takeValue)
        {
            if (takeValue == null)
            {
                return (await _context.Set<Entity.Models.Client>()
              .ToListAsync());
            }

            return (await _context.Set<Entity.Models.Client>()
                .ToListAsync())
                .Take(takeValue.Value);
        }
    }
}