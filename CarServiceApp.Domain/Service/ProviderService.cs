using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApp.Domain.Service
{
    public class ProviderService(
        ApplicationDbContext context,
        IGenericRepositoryAsync<Provider, string> providerRepository) : IProviderService
    {
        private const string ProviderConstName = "Выбрать поставщика...";
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly IGenericRepositoryAsync<Provider, string> _providerRepository = providerRepository ?? throw new ArgumentNullException(nameof(providerRepository));

        public async Task<IEnumerable<Provider>> ProvidersAsync(string id, string name, int? takeValue)
        {
            return takeValue != null
                ? (await GetProvidersAsync())
                    .Where(s => s.Id.Contains(id) && s.Name.Contains(name))
                    .Take((int)takeValue)
                : (await GetProvidersAsync())
                    .Where(s => s.Id.Contains(id) && s.Name.Contains(name));
        }

        public async Task<SelectList> ProvidersAsync()
        {
            var providers = await MostQueriedProvidersAsync();
            providers.Add(new MostQueriedProvider
            {
                Id = "0",
                Name = ProviderConstName
            });

            return new SelectList(providers, "Id", "Name");
        }

        private async Task<List<MostQueriedProvider>> MostQueriedProvidersAsync()
        {
            var topProviders = (from provider in (await GetProvidersAsync())
                                join invoice in (await GetInvoicesAsync()) on provider.Id equals invoice.ProviderId
                                group invoice by new { provider.Name, provider.Id } into g
                                orderby g.Count() descending
                                select new MostQueriedProvider
                                {
                                    Name = g.Key.Name,
                                    Id = g.Key.Id,
                                    CountInvoice = g.Count()
                                }).Take(5).ToList();

            return topProviders;
        }

        private async Task<IEnumerable<Provider>> GetProvidersAsync()
        {
            return await _providerRepository.GetAllAsync();
        }

        private async Task<List<Invoice>> GetInvoicesAsync()
        {
            var query = _context.Set<Invoice>();

            return await query.ToListAsync();
        }

        public async Task<SelectList> ProvidersSearchAsync(int? takeValue = null, bool addDefaultValue = true)
        {
            var providers = takeValue == null
                ? (await GetProvidersAsync()).ToList()
                : (await GetProvidersAsync()).Take(takeValue.Value).ToList();

            if (addDefaultValue)
            {
                providers.Add(new Provider
                {
                    Id = "",
                    Name = ProviderConstName
                });
            }

            return new SelectList(providers, "Id", "Name", "");
        }
    }
}