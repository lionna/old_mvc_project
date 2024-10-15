using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity.Models;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IPreRecordService
    {
        Task<(int, int)> CreatePreRecordAsync(string fullName,
            string phone,
            string markModel,
            int? issueYear,
            string regNumber,
            string employeeId,
            string[] serviceIds);

        Task<List<Entity.Models.Service>> GetServicesByNameAsync(string id, string name, int takeValye);

        Task<Entity.Models.PreRecordService> GetPreRecordServiceAsync(long? id, string serviceId);

        Task<List<Entity.Models.PreRecordService>> GetPreRecordServiceAsync(long? id);

        Task<List<Entity.Models.PreRecordService>> AddAsync(long? id, string serviceId);

        Task DeletePreRecordServiceAsync(Entity.Models.PreRecordService preRecordService);

        Task<GridItem> GetPreRecordsAsync(
            string clientName,
            string regNumberCar,
            string mark,
            long recordIntervalDown,
            long recordIntervalUp,
            DateTime? dateMakingFrom,
            DateTime? dateMakingBefore,
            bool? isRejection,
            int page,
            int pageSize);

        Task<PreRecord> GetPreRecordByIdAsync(long recordId);

        Task UpdatePreRecordync(PreRecord preRecord);

        Task DeletePreRecordAsync(PreRecord preRecord);

        Task<List<Client>> GetClientAsync(string name, string firstName, string subStringName);
    }
}