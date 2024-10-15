using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Repository.Abstract;
using CarServiceApp.Domain.Service.Abstract;
 

namespace CarServiceApp.Domain.Service
{
    public class WorkLoadingService(IWorkLoadingRepository workLoadingRepository) : IWorkLoadingService
    {
        private readonly IWorkLoadingRepository _workLoadingRepository = workLoadingRepository;

        public async Task<int> CancelReservHourForServiceAsync(
            DateTime? dateTimeReservation, int? personalNumber)
        {
            return await _workLoadingRepository.CancelReservHourForServiceAsync(dateTimeReservation, personalNumber);
        }

        public async Task<IEnumerable<ExecutingService>> ExecutingServicesAsync()
        {
            return await _workLoadingRepository.ExecutingServicesAsync();
        }

        public async Task<List<ExecutingService>> ExecutingServicesAsync(DateTime? date, int? id)
        {
            return await _workLoadingRepository.ExecutingServicesAsync(date, id);
        }

        public async Task<int> ExecutionProcedureAsync(string nameProcedure, Dictionary<string, object> inputParam)
        {
            return await _workLoadingRepository.ExecutionProcedureAsync(nameProcedure, inputParam);
        }

        public async Task<Entity.Models.OrderService> GetOrderServiceById(string orderId)
        {
            return await _workLoadingRepository.GetOrderServiceById(orderId);
        }

        public async Task<List<Entity.Models.Service>> GetServicesAsync(
            string partId, string partName, int takeValue)
        {
            return await _workLoadingRepository.GetServicesAsync(partId, partName, takeValue);
        }

        public async Task<IEnumerable<Entity.Models.PreRecordService>> PreRecordServiceAsync()
        {
            return await _workLoadingRepository.PreRecordServiceAsync();
        }

        public async Task<List<Entity.Models.PreRecordService>> PreRecordServiceAsync(DateTime? date, int? id)
        {
            return await _workLoadingRepository.PreRecordServiceAsync(date, id);
        }

        public async Task<int> PreReservHourForServiceAsync(
            string[] serviceId, string recordId, DateTime? currentDate, int? personnelNumber, 
            int? hourSelect)
        {
            DateTime? date = new DateTime(currentDate.Value.Year, currentDate.Value.Month, 
                currentDate.Value.Day, hourSelect.Value, 0, 0);
            var serviceIdWithJoin = string.Join("|", serviceId);
            var inputParams = new Dictionary<string, object> {
                                                                    { "recordId", recordId },
                                                                    { "dateTimeReserv", date  },
                                                                    { "persNumber", personnelNumber },
                                                                    { "charArrayID_services", serviceIdWithJoin }
                                                                 };
            return await  ExecutionProcedureAsync("PreReservHourForService", inputParams);
        }

        public async Task<int> ReservHourForServiceAsync(
            string[] serviceId, string orderId, DateTime? currentDate, int? personnelNumber, int? hourSelect)
        {
            DateTime? date = new DateTime(currentDate.Value.Year, currentDate.Value.Month, currentDate.Value.Day, hourSelect.Value, 0, 0);
            var stringServiceId = string.Join("|", serviceId);
            var inputParams = new Dictionary<string, object> {
                                                                    { "ID_order", orderId },
                                                                    { "dateTimeReserv", date  },
                                                                    { "persNumber", personnelNumber },
                                                                    { "charArrayID_services", stringServiceId }
                                                                 };
            return await ExecutionProcedureAsync("ReservHourForService", inputParams);
        }

        public async Task<TableLoadViewModel> TableLoadAsync(DateTime? currentDate)
        {
            return await _workLoadingRepository.TableLoadAsync(currentDate);
        }

        public async Task<TableLoadPreRecordViewModel> TableLoadPreRecordViewModelAsync(
            DateTime? currentDate, long? preRecordId)
        {
            return await _workLoadingRepository.TableLoadPreRecordViewModelAsync(currentDate, preRecordId);
        }
    }
}
