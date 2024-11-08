using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IInvoiceService
    {
        Task<int> AddSparePartInvoiceAsync(
            double numberValue,
            decimal priceValue,
            string partId,
            int? lotNumber);

        Task<Invoice> InvoiceAsync(int lotNumber);

        Task<int> AddInvoiceAsync(
            string providerId,
            int? personnelNumber,
            DateOnly? deliveryDate,
            int? lotNumber);

        Task<List<InvoiceInformation>> GetInfoInvoiceByLotNumberAsync(int lotNumber);

        Task<List<Invoice>> GeInvoiceAsync(
            int? lotNumber,
            DateOnly? dateLotStart,
            DateOnly? dateLotEnd,
            string providerId);

        Task<int> DeleteInvoiceAsync(int lotNumber);

        Task DeleteInvoiceAsync(Invoice invoice);

        Task DeleteAcceptanceInvoiceAsync(int? id);

        Task<int> ClearInvoiceAsync(int lotNumber);

        Task<int> DeleteCustomInvoiceAsync(int id);
    }
}