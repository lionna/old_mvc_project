using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;

namespace CarServiceApp.Domain.Repository.Abstract
{
    public interface IWorkLoadingRepository
    {
        Task<TableLoadViewModel> TableLoadAsync(DateTime? currentDate);
        Task<TableLoadPreRecordViewModel> TableLoadPreRecordViewModelAsync(DateTime? currentDate, long? preRecordId);
        Task<OrderService> GetOrderServiceById(string orderId);
        Task<IEnumerable<ExecutingService>> ExecutingServicesAsync();
        Task<IEnumerable<PreRecordService>> PreRecordServiceAsync();
        Task<List<ExecutingService>> ExecutingServicesAsync(DateTime? date, int? id);
        Task<List<PreRecordService>> PreRecordServiceAsync(DateTime? date, int? id);
        Task<int> ExecutionProcedureAsync(string nameProcedure, Dictionary<string, object> inputParam);
        Task<int> CancelReservHourForServiceAsync(DateTime? dateTimeReservation, int? personalNumber);
        Task<List<Entity.Models.Service>> GetServicesAsync(string partId, string partName, int takeValue);
    }
}
