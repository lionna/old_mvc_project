using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Repository;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace CarServiceApp.Domain.Service
{
    public class PreRecordService(
    IDbConnectionLayer dbConnection,
    ApplicationDbContext context) : IPreRecordService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly IDbConnectionLayer _dbConnection = dbConnection;

        private static Expression<Func<PreRecord, object>>[] GetCarIncludeInfo()
        {
            return
            [
                x => x.Employee,
            ];
        }

        public async Task<GridItem> GetPreRecordsAsync(
            string clientName,
            string regNumberCar,
            string mark,
            long recordIntervalDown,
            long recordIntervalUp,
            DateTime? dateMakingFrom,
            DateTime? dateMakingBefore,
            bool? isRejection,
            int page,
            int pageSize)
        {
            var query = _context.Set<PreRecord>().AsQueryable();

            //query = query.Where(resp => (resp.FullName.Contains(clientName)
            //                 || string.IsNullOrEmpty(resp.FullName)) &&
            //                 (resp.RegNumberCar.Contains(regNumberCar)
            //                 || string.IsNullOrEmpty(resp.RegNumberCar)) &&
            //                 (resp.MarkModelCar.Contains(mark)
            //                 || string.IsNullOrEmpty(resp.MarkModelCar)) &&
            //                 resp.RecordId >= recordIntervalDown && resp.RecordId <= recordIntervalUp
            //                 && resp.DateMakingRecord >= dateMakingFrom
            //                 && resp.DateMakingRecord <= dateMakingBefore
            //                 && (resp.IsRejection == (isRejection ?? false)
            //                 || resp.IsRejection == (isRejection ?? true)));

            //query = query.Where(resp => resp.DateMakingRecord >=  dateMakingFrom
            //                 && resp.DateMakingRecord <= dateMakingBefore);

            var includes = GetCarIncludeInfo();

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

        public async Task DeletePreRecordServiceAsync(Entity.Models.PreRecordService preRecordService)
        {
            _context.Set<Entity.Models.PreRecordService>().Remove(preRecordService);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Entity.Models.PreRecordService>> AddAsync(long? id, string serviceId)
        {
            var entity = new Entity.Models.PreRecordService()
            {
                ServiceId = serviceId,
                RecordId = id.Value
            };

            await _context.Set<Entity.Models.PreRecordService>().AddAsync(entity);
            await _context.SaveChangesAsync();

            var query = await GetPreRecordServiceAsync(id);

            return query;
        }

        public async Task<(int, int)> CreatePreRecordAsync(
            string fullName,
            string phone,
            string markModel,
            int? issueYear,
            string regNumber,
            string employeeId,
            string[] serviceIds)
        {
            var ids = string.Join("|", serviceIds);
            var inputParams = new Dictionary<string, object> {
                { "FullName", fullName},
                { "Phone",phone??(object)DBNull.Value},
                { "MarkModel", markModel },
                { "IssueYear", issueYear??(object)DBNull.Value},
                { "RegisterNumber", regNumber??(object)DBNull.Value},
                { "PersonalNumber", employeeId},
                { "CharArrayServiceId", ids }
            };

            var outputParams = new Dictionary<string, object> { { "RecordId", SqlDbType.BigInt } };
            var statusValue = await _dbConnection.ExecutionProcedureAsync("AddPreRecord", inputParams, outputParams);

            return (statusValue, (int)outputParams["RecordId"]);
        }

        public async Task<Entity.Models.PreRecordService> GetPreRecordServiceAsync(long? id, string serviceId)
        {
            var query = _context.Set<Entity.Models.PreRecordService>()
             .Where(s => s.RecordId == id && s.ServiceId.Contains(serviceId));

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Entity.Models.PreRecordService>> GetPreRecordServiceAsync(long? id)
        {
            var query = _context.Set<Entity.Models.PreRecordService>()
             .Where(s => s.RecordId == id);

            return await query.ToListAsync();
        }

        public async Task<List<Entity.Models.Service>> GetServicesByNameAsync(string id, string name, int takeValye)
        {
            var query = _context.Set<Entity.Models.Service>()
               .Where(s => s.Name.Contains(name) && s.Id.Contains(id) && s.Available)
               .Take(takeValye);

            return await query.ToListAsync();
        }

        public async Task<PreRecord> GetPreRecordByIdAsync(long recordId)
        {
            return await _context.Set<PreRecord>().AsQueryable()
                .Include(pr => pr.PreRecordServices)
                    .ThenInclude(prs => prs.Service)
                        .ThenInclude(s => s.ServiceType)
                .Include(pr => pr.PreRecordServices)
                    .ThenInclude(prs => prs.Employee)
                        .ThenInclude(s => s.PreRecordServices)
                //.Include(pr => pr.PreRecordServices)
                //    .ThenInclude(prs => prs.PreRecord)
                //        .ThenInclude(s => s.Employee)
                .Include(pr => pr.Employee)
                .Include(pr => pr.Employee.Post)
                .FirstOrDefaultAsync(pr => pr.RecordId == recordId);
        }

        public async Task DeletePreRecordAsync(PreRecord preRecord)
        {
            _context.Set<PreRecord>().Remove(preRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Client>> GetClientAsync(string name, string firstName, string subStringName)
        {
            var query = _context.Set<Client>().AsQueryable();

            if (!string.IsNullOrEmpty(subStringName))
            {
                query = query.Where(s => s.FullName.Contains(subStringName));
            }
            else
            {
                if (!string.IsNullOrEmpty(firstName))
                {
                    query = query.Where(s => s.FullName.Contains(firstName));
                }
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(s => s.FullName.Contains(name));
                }
            }

            return await query.ToListAsync();
        }

        public async Task UpdatePreRecordync(PreRecord preRecord)
        {
            ArgumentNullException.ThrowIfNull(preRecord);
            _context.Set<PreRecord>().Update(preRecord);
            await _context.SaveChangesAsync();
        }
    }
}