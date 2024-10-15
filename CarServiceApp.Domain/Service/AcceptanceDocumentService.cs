using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CarServiceApp.Domain.Service
{
    public class AcceptanceDocumentService(ApplicationDbContext context) : IAcceptanceDocumentService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<int> ClearCustomInvoiceAsync(int id)
        {
            var acceptDocumentExists = _context.AcceptanceDocument.Any(ad => ad.AcceptanceId == id);
            if (acceptDocumentExists)
            {
                var usingCustomSPartMatExists = _context.UsingCustomSpartMat.Any(us => us.AcceptanceId == id);
                if (!usingCustomSPartMatExists)
                {
                    var customSPartsToDelete = _context.AcceptanceCustomSpart.Where(acsp => acsp.AcceptanceId == id);
                    _context.AcceptanceCustomSpart.RemoveRange(customSPartsToDelete);
                    await _context.SaveChangesAsync();
                    return 0;
                }
                else
                {
                    return -2;
                }
            }
            else
            {
                return -1;
            }
        }

        public async Task<List<AcceptanceCustomSpartModel>> GetAcceptedCustomPartsAsync(int id)
        {
            var result = await _context.AcceptanceCustomSpart
                .Where(acsp => acsp.AcceptanceId == id)
                .Include(acsp => acsp.SparePartMaterial)
                .Select(acsp => new AcceptanceCustomSpartModel
                {
                    AcceptanceId = acsp.AcceptanceId,
                    PartId = acsp.PartId,
                    Number = acsp.Number,
                    Name = acsp.SparePartMaterial.Name,
                    Unit = acsp.SparePartMaterial.Unit,
                    Manufacture = acsp.SparePartMaterial.Manufacture,
                    StateSPart = acsp.StateSpart
                })
                .ToListAsync();

            return result;
        }

        public async Task CreateAsync(AcceptanceDocument entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _context.Set<AcceptanceDocument>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<CustomInvoiceInfo> GetInfoCustomInvoiceByIdAsync(int? id)
        {
            var query = from employee in _context.Employee
                        join acceptDocument in _context.AcceptanceDocument on employee.PersonnelNumber equals acceptDocument.PersonnelNumber
                        join client in _context.Client on acceptDocument.ClientId equals client.ClientId
                        where acceptDocument.AcceptanceId == id
                        select new CustomInvoiceInfo
                        {
                            ClientName = client.FullName,
                            Date = acceptDocument.AcceptDate,
                            ClientId = client.ClientId,
                            EmployeeName = employee.FullName,
                            PersonnelNumber = employee.PersonnelNumber,
                            AcceptanceId = acceptDocument.AcceptanceId
                        };

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> DeletePartAsync(string partId, int? acceptanceId)
        {
            var value = await _context.AcceptanceCustomSpart
                .Include(p => p.UsingCustomSpartMats)
                .FirstOrDefaultAsync(p => p.PartId == partId && p.AcceptanceId == acceptanceId);

            if (value != null && value.UsingCustomSpartMats?.Count == 0)
            {
                _context.AcceptanceCustomSpart.Remove(value);
                await _context.SaveChangesAsync();
                return 0;
            }

            return -1;
        }

        public async Task<int> AddCustomInvoiceAsync(string clientId, int? personnelNumber, DateTime? date)
        {
            int id = -1;

            var clientExists = _context.Client.Any(c => c.ClientId == clientId);
            var employeeExists = _context.Employee.Any(e => e.PersonnelNumber == personnelNumber);

            if (clientExists && employeeExists)
            {
                if (date == null || date > DateTime.UtcNow)
                {
                    date = DateTime.UtcNow;
                }

                var entity = new AcceptanceDocument
                {
                    ClientId = clientId,
                    AcceptDate = date.Value,
                    PersonnelNumber = personnelNumber ?? 0
                };

                _context.AcceptanceDocument.Add(entity);
                await _context.SaveChangesAsync();

                id = entity.AcceptanceId;
                return 1;
            }

            return -1;
        }

        public async Task<AcceptanceDocument> AcceptanceDocumentAsync(int id)
        {
            return await _context.Set<AcceptanceDocument>().AsQueryable()
                 .FirstOrDefaultAsync(a => a.AcceptanceId == id);
        }

        public async Task<int> AddCustomPartInvoiceAsync(double number, string state, string partId, int id)
        {
            var acceptDocumentExists = _context.Set<AcceptanceDocument>().AsQueryable()
                .Where(a => a.AcceptanceId == id);

            if (acceptDocumentExists.Any())
            {
                var exists = _context.Set<AcceptanceCustomSpart>().AsQueryable()
                    .Any(ac => ac.PartId == partId && ac.AcceptanceId == id);

                if (!exists)
                {
                    var entity = new AcceptanceCustomSpart
                    {
                        AcceptanceId = id,
                        PartId = partId,
                        StateSpart = state,
                        Number = number
                    };

                    _context.AcceptanceCustomSpart.Add(entity);
                    await _context.SaveChangesAsync();

                    return 0;
                }
                else
                {
                    // Если запись уже существует, вернуть -2
                    return -2;
                }
            }
            else
            {
                // Если не найден соответствующий документ приема, вернуть -1
                return -1;
            }
        }
    }
}