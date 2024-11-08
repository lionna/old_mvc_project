using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;

namespace CarServiceApp.Domain.Service.Abstract
{
    public interface IAcceptanceInvoiceService
    {
        Task<AcceptanceInvoice> GetByIdAsync(long positionId);

        Task UpdateAsync(AcceptanceInvoice entity);

        Task<GridItem> FindSparePartsAsync(
            string partId, string name, string manufacture, decimal? priceBefore,
            decimal? priceAfter, int? invoiceId, float? stockPercent, DateTime? dateInvoiceFrom,
            DateTime? dateInvoiceBefore, int? personalNumber, string fieldNameSort, bool? isAsccend,
            int? pageNumber, int pageSize);

        Task<List<AttachedPartInfo>> GetAttachedPartsAsync(string orderId, string serviceId);

        Task<List<AttachedCustomPartInfo>> GetAttachedCustomPartsAsync(string orderId, string serviceId);

        Task<string> GetServiceName(string id);
    }
}