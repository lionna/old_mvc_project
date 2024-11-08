using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IOrderService
    {
        Task<SelectList> GetEmployeesAsync(bool usePostFilter = true, int postId = 5, bool userDefaultValue = false);

        Task<SelectList> GetIncludedCarByClientIdAsync(string id);

        Task<Entity.Models.OrderService> GetOrderByIdAsync(string id);

        Task<SelectList> GetServiceTypesAsync();

        Task<SelectList> GetTypeOfPaymentsAsync();

        Task<TotalInfoModel> GetTotalInfoExecServicesByIdAsync(string orderId);

        Task<(decimal, decimal, double, decimal)> GetCalculatedTotalMoneyTimeByOrderAsync(string orderId);

        Task<GridItem> GetServicesAsync(string serviceName, int typeId, int page, int pageSize);

        Task<GridItem> SearchOrdersAsync(
            string clientId,
            string fullName,
            string orderId,
            DateTime? dateFrom,
            DateTime? dateTo,
            bool isClosed,
            bool isReject,
            int page,
            int pageSize,
            string fieldNameSort = "FullName",
            bool isAscend = true);

        Task<Entity.Models.Service> GetServiceByIdAsync(string serviceId);

        Task<SelectList> ListEmployeeAsync(string serviceId);

        Task<int> AddServiceToOrderAsync(string orderId,
            string serviceId,
            DateTime? dateComplete,
            double? takeTime,
            string notes,
            int? personnelNumber,
            int taxAddedValue,
            bool isEdit);

        Task<int> DeleteOrderAsync(string orderId);

        Task<int> DeleteAttachedServiceAsync(string orderId, string serviceId);

        Task<int> CloseOrderAsync(
            string orderId,
            DateTime? dateFactComplete,
            DateTime? completeDate,
            int? paymentTypeId,
            int? personnelNumberCloser,
            bool? isCompleted,
            string rejectionReason);

        Task<List<RemarkToStateCar>> RemarkToStateCarsAsync(string orderId);

        Task<List<UsingCustomSpartMat>> GetUsingCustomSpartMatAsync(string orderId);

        Task<List<AllInfoAttachedPart>> GetAllInfoAttachedPartByIdAsync(string orderId);

        Task<List<TotalInfoServicesByOrder>> GetTotalInfoServicesByOrderAsync(string orderId);

        Task<IEnumerable<string>> GetOrderServiceIdsAsync(string term, string clientId, int takeItems);

        Task<IEnumerable<Entity.Models.OrderService>> GetOrderServicesByClientIdAsync(string clientId);

        Task<(int returnValue, string orderId, string errorMessage)> AddPartOrderAsync(
                    DateTime? dateMaking, string clientId, string vinNumber, string description,
                    int? currentMill, int employeeId, byte numberWheelCaps, byte numberWipers,
                    byte numberWipersArms, bool isAntenna, bool isSpareWheel, bool isCoverDecorEngine,
                    bool isTuner, byte? fluelLevelPercent);

        Task<(int returnValue, string orderId, string errorMessage)> AddPartialOrderForRecordAsync(
            DateTime? dateMaking, string clientId, string vinNumber, string description,
            int? currentMill, int employeeId, long? recordId, byte numberWheelCaps,
            byte numberWipers, byte numberWipersArms, bool isAntenna, bool isSpareWheel,
            bool isCoverDecorEngine, bool isTuner, byte? fluelLevelPercent);

        Task AddRemarkToStateCarAsync(RemarkToStateCar entity);

        Task<IEnumerable<ExecutingService>> GetExecutingServiceByOrderIdAsync(string orderId);

        Task<ExecutingService> GetExecutingServiceAsync(string orderId, string serviceId);

        Task<AcceptanceDocument> GetAcceptDocumentByIdAsync(int acceptanceId);

        Task<IEnumerable<ReportAcceptedCustomParts>> GetReportAcceptedCustomParts(string orderId, string clientId);

        Task<bool> IsClosedOrderAsync(string id);
    }
}