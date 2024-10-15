using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IWorkLoadingService
    {
        Task<TableLoadViewModel> TableLoadAsync(DateTime? currentDate);
        Task<TableLoadPreRecordViewModel> TableLoadPreRecordViewModelAsync(DateTime? currentDate, long? preRecordId);
        Task<CarServiceApp.Domain.Entity.Models.OrderService> GetOrderServiceById(string orderId);
        Task<IEnumerable<CarServiceApp.Domain.Entity.Models.ExecutingService>> ExecutingServicesAsync();
        Task<IEnumerable<CarServiceApp.Domain.Entity.Models.PreRecordService>> PreRecordServiceAsync();
        Task<List<CarServiceApp.Domain.Entity.Models.ExecutingService>> ExecutingServicesAsync(DateTime? date, int? id);
        Task<List<CarServiceApp.Domain.Entity.Models.PreRecordService>> PreRecordServiceAsync(DateTime? date, int? id);
        Task<int> ExecutionProcedureAsync(string nameProcedure, Dictionary<string, object> inputParam);
        Task<int> CancelReservHourForServiceAsync(DateTime? dateTimeReservation, int? personalNumber);
        Task<List<Entity.Models.Service>> GetServicesAsync(string partId, string partName, int takeValue);
        Task<int> PreReservHourForServiceAsync(
            string[] serviceId, string recordId, DateTime? currentDate, int? personnelNumber, int? hourSelect);
        Task<int> ReservHourForServiceAsync(string[] serviceId, string orderId, DateTime? currentDate, int? personnelNumber, int? hourSelect);
    }
}
