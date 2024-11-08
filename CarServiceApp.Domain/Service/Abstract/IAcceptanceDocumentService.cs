using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IAcceptanceDocumentService
    {
        Task<int> AddCustomPartInvoiceAsync(double number, string state, string partId, int id);

        Task<AcceptanceDocument> AcceptanceDocumentAsync(int id);

        Task<int> AddCustomInvoiceAsync(string clientId, int? personnelNumber, DateTime? date);

        Task<CustomInvoiceInfo> GetInfoCustomInvoiceByIdAsync(int? id);

        Task<int> DeletePartAsync(string partId, int? acceptanceId);

        Task CreateAsync(AcceptanceDocument entity);

        Task<List<AcceptanceCustomSpartModel>> GetAcceptedCustomPartsAsync(int id);

        Task<int> ClearCustomInvoiceAsync(int id);
    }
}