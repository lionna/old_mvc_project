using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApp.Domain.Service
{
    public class InvoiceService(ApplicationDbContext context) : IInvoiceService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
       
        public async Task<int> DeleteCustomInvoiceAsync(int id)
        {
            var acceptDocument = await _context.AcceptanceDocument
                .Include(ad => ad.AcceptanceCustomSparts)
                .FirstOrDefaultAsync(ad => ad.AcceptanceId == id);

            if (acceptDocument != null)
            {
                if (acceptDocument.AcceptanceCustomSparts?.Count == 0)
                {
                    _context.AcceptanceDocument.Remove(acceptDocument);
                    await _context.SaveChangesAsync();
                    return 0;
                }
                else
                {
                    // Материалы данного акта сдачи-приемки уже задействованы. Невозможно удалить документ!
                    return -2;
                }
            }
            else
            {
                // Документ с указанным идентификатором не найден
                return -1;
            }
        }

        public async Task DeleteAcceptanceInvoiceAsync(int? id)
        {
            var deleteValue = await _context.AcceptanceInvoice.FirstOrDefaultAsync(i => i.PositionId == id);
            _context.AcceptanceInvoice.Remove(deleteValue);
            await _context.SaveChangesAsync();
        }

        public async Task<int> ClearInvoiceAsync(int lotNumber)
        {
            var invoiceExists = await _context.Invoice.AnyAsync(i => i.LotNumber == lotNumber);
            if (invoiceExists)
            {
                var usingPartExists = await _context.AcceptanceInvoice
                                                    .Where(ai => ai.LotNumber == lotNumber)
                                                    .Select(ai => ai.PositionId)
                                                    .Distinct()
                                                    .AnyAsync(id => _context.UsingPartMaterial.Any(up => up.PositionId == id));

                if (!usingPartExists)
                {
                    var acceptanceInvoicesToDelete = await _context.AcceptanceInvoice.Where(ai => ai.LotNumber == lotNumber).ToListAsync();
                    _context.AcceptanceInvoice.RemoveRange(acceptanceInvoicesToDelete);
                    await _context.SaveChangesAsync();
                    return 0;
                }
                else
                {
                    //Console.WriteLine("Некоторые материалы данного акта сдачи-приемки уже задействованы. Невозможно исключить все материалы!");
                    return -2;
                }
            }
            else
            {
                return -1;
            }
        }

        public async Task DeleteInvoiceAsync(Invoice invoice)
        {
            var deleteValue = await _context.Invoice.FirstOrDefaultAsync(i => i.LotNumber == invoice.LotNumber);
            _context.Invoice.Remove(deleteValue);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteInvoiceAsync(int lotNumber)
        {
            var invoiceExists = (await _context.Invoice.AnyAsync(i => i.LotNumber == lotNumber));
            if (invoiceExists)
            {
                var usingPartExists = (await _context.AcceptanceInvoice
                                            .Where(ai => ai.LotNumber == lotNumber)
                                            .Select(ai => ai.PositionId)
                                            .Distinct()
                                            .AnyAsync(id => _context.UsingPartMaterial.Any(up => up.PositionId == id)));

                if (!usingPartExists)
                {
                    var deleteValue = await _context.Invoice.FirstOrDefaultAsync(i => i.LotNumber == lotNumber);
                    _context.Invoice.Remove(deleteValue);
                    await _context.SaveChangesAsync();
                    return 0;
                }
                else
                {
                    // Console.WriteLine("Материалы данного акта сдачи-приемки уже задействованы. Невозможно удалить документ!");
                    return -2;
                }
            }
            else
            {
                return -1;
            }
        }

        public async Task<List<Invoice>> GeInvoiceAsync(int? lotNumber, DateOnly? dateLotStart, DateOnly? dateLotEnd, string providerId)
        {
            var lotNumberMin = lotNumber ?? 0;
            var lotNumberMax = lotNumber ?? int.MaxValue;

            var query = _context.Set<Entity.Models.Invoice>()
             .Where(f => f.LotNumber >= lotNumberMin && f.LotNumber <= lotNumberMax &&
                                               f.DeliveryDate >= dateLotStart && f.DeliveryDate <= dateLotEnd &&
                                               f.ProviderId.Contains(providerId));

            return await query.ToListAsync();
        }

        public async Task<List<InvoiceInformation>> GetInfoInvoiceByLotNumberAsync(int lotNumber)
        {
            var query = from invoice in _context.Invoice
                        join provider in _context.Provider on invoice.ProviderId equals provider.Id
                        join employee in _context.Employee on invoice.PersonnelNumber equals employee.PersonnelNumber
                        where invoice.LotNumber == lotNumber
                        select new InvoiceInformation
                        {
                            ProviderName = provider.Name,
                            DeliveryDate = invoice.DeliveryDate,
                            ProviderId = provider.Id,
                            FullName = employee.FullName,
                            PersonnelNumber = employee.PersonnelNumber,
                            LotNumber = invoice.LotNumber
                        };

            return await query.Distinct().ToListAsync();
        }

        private async Task<List<Provider>> GeProviderAsync(string id)
        {
            var query = _context.Set<Provider>()
             .Where(s => s.Id == id);

            return await query.ToListAsync();
        }

        private async Task<List<Employee>> GeEmployeeAsync(int id)
        {
            var query = _context.Set<Entity.Models.Employee>()
             .Where(s => s.PersonnelNumber == id);

            return await query.ToListAsync();
        }

        private async Task<List<Entity.Models.Invoice>> GeInvoiceAsync(int id)
        {
            var query = _context.Set<Entity.Models.Invoice>()
             .Where(s => s.LotNumber == id);

            return await query.ToListAsync();
        }

        public async Task<int> AddInvoiceAsync(string providerId, int? personnelNumber, DateOnly? deliveryDate, int? lotNumber)
        {
            var providerExists = (await GeProviderAsync(providerId)).Count != 0;
            var employeeExists = (await GeEmployeeAsync(personnelNumber ?? 0)).Count != 0;

            if (providerExists && employeeExists)
            {
                var invoiceExists = (await GeInvoiceAsync(lotNumber ?? 0)).Count != 0;
                if (!invoiceExists)
                {
                    if (!deliveryDate.HasValue || deliveryDate > DateOnly.FromDateTime(DateTime.UtcNow))
                    {
                        deliveryDate = DateOnly.FromDateTime(DateTime.UtcNow);
                    }

                    var entity = new Invoice()
                    {
                        LotNumber = lotNumber ?? 0,
                        ProviderId = providerId,
                        DeliveryDate = deliveryDate,
                        PersonnelNumber = personnelNumber
                    };

                    await _context.Set<Invoice>().AddAsync(entity);
                    await _context.SaveChangesAsync();

                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -2;
            }
        }

        public async Task<Invoice> InvoiceAsync(int lotNumber)
        {
            return await _context.Set<Invoice>().FirstOrDefaultAsync(i => i.LotNumber == lotNumber);
        }

        public async Task<int> AddSparePartInvoiceAsync(
            double numberValue, decimal priceValue, string partId, int? lotNumber)
        {
            var existingInvoice = await _context.Set<Invoice>().FirstOrDefaultAsync(i => i.LotNumber == lotNumber);
            if (existingInvoice != null)
            {
                var existingAcceptance = await _context.Set<AcceptanceInvoice>().FirstOrDefaultAsync(a => a.PartId == partId && a.LotNumber == lotNumber && a.Price == priceValue);
                if (existingAcceptance == null)
                {
                    var entity = new AcceptanceInvoice()
                    {
                        LotNumber = lotNumber ?? 0,
                        PartId = partId,
                        Price = priceValue,
                        Number = numberValue,
                        TradeIncrease = 30
                    };

                    await _context.Set<Entity.Models.AcceptanceInvoice>().AddAsync(entity);
                    await _context.SaveChangesAsync();

                    return 0;
                }
            }

            return -1;
        }
    }
}