using CarServiceApp.Domain.Common;
using CarServiceApp.Domain.Config;
using CarServiceApp.Domain.Entity;
using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace CarServiceApp.Domain.Service
{
    public class AcceptanceInvoiceService(
        ApplicationDbContext context,
        IOptions<ApplicationSettings> settings) : IAcceptanceInvoiceService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        private readonly string connectionString = settings?.Value?.DefaultConnection ?? throw new ArgumentNullException(nameof(settings));

        public async Task<string> GetServiceName(string id)
        {
            return (await _context.Set<Entity.Models.Service>()
             .FirstOrDefaultAsync(s => s.Id == id)).Name;
        }

        public async Task<AcceptanceInvoice> GetByIdAsync(long positionId)
        {
            return await _context.Set<Entity.Models.AcceptanceInvoice>()
             .FirstOrDefaultAsync(s => s.PositionId == positionId);
        }

        public async Task<List<AttachedPartInfo>> GetAttachedPartsAsync(string orderId, string serviceId)
        {
            var attachedParts = await (from sparePart in _context.SparePartMaterial
                                       join acceptance in _context.AcceptanceInvoice
                                       on sparePart.Id equals acceptance.PartId
                                       join usingPart in _context.UsingPartMaterial
                                       on acceptance.PositionId equals usingPart.PositionId
                                       where usingPart.OrderId == orderId && usingPart.ServiceId == serviceId
                                       select new AttachedPartInfo
                                       {
                                           PartId = sparePart.Id,
                                           Name = sparePart.Name,
                                           Manufacture = sparePart.Manufacture,
                                           Unit = sparePart.Unit,
                                           Number = usingPart.Number,
                                           CostPart = usingPart.CostPart,
                                           LotNumber = acceptance.LotNumber,
                                           Price = acceptance.Price,
                                           PositionId = usingPart.PositionId
                                       }).ToListAsync();

            return attachedParts;
        }

        public async Task<List<AttachedCustomPartInfo>> GetAttachedCustomPartsAsync(string orderId, string serviceId)
        {
            var attachedCustomParts = await (from acceptanceCustomPart in _context.AcceptanceCustomSpart
                                             join sparePart in _context.SparePartMaterial
                                             on acceptanceCustomPart.PartId equals sparePart.Id
                                             join usingCustomPart in _context.UsingCustomSpartMat
                                             on sparePart.Id equals usingCustomPart.PartId
                                             where usingCustomPart.OrderId == orderId && usingCustomPart.ServiceId == serviceId
                                             select new AttachedCustomPartInfo
                                             {
                                                 PartId = sparePart.Id,
                                                 Name = sparePart.Name,
                                                 Manufacture = sparePart.Manufacture,
                                                 Unit = sparePart.Unit,
                                                 Number = usingCustomPart.Number,
                                                 StateSPart = acceptanceCustomPart.StateSpart,
                                                 AcceptanceId = acceptanceCustomPart.AcceptanceId
                                             }).ToListAsync();

            return attachedCustomParts;
        }

        public async Task UpdateAsync(AcceptanceInvoice entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Set<AcceptanceInvoice>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<GridItem> FindSparePartsAsync(
            string partId, string name, string manufacture, decimal? priceBefore,
            decimal? priceAfter, int? invoiceId, float? stockPercent, DateTime? dateInvoiceFrom,
            DateTime? dateInvoiceBefore, int? personalNumber, string fieldNameSort, bool? isAsccend,
            int? pageNumber, int pageSize)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "dbo.FindSpareParts";

            command.Parameters.AddWithValue("@PartId", partId);
            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Manufacture", manufacture);
            command.Parameters.AddWithValue("@PriceBefore", priceBefore);
            command.Parameters.AddWithValue("@PriceAfter", priceAfter);
            command.Parameters.AddWithValue("@InvoiceId", invoiceId ?? 0);
            command.Parameters.AddWithValue("@StockPercent", stockPercent);
            command.Parameters.AddWithValue("@DateInviceFrom", dateInvoiceFrom);
            command.Parameters.AddWithValue("@DateInvoiceBefore", dateInvoiceBefore);
            command.Parameters.AddWithValue("@PersonalNumber", personalNumber ?? 0);
            command.Parameters.AddWithValue("@FieldNameSort", fieldNameSort);
            command.Parameters.AddWithValue("@IsAsccend", (isAsccend == null ? 0 : (isAsccend == true ? 1 : 0)));
            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.Add("@CountOfRows", SqlDbType.Int).Direction = ParameterDirection.Output;

            var spareParts = new List<SparePartModel>();

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var sparePart = new SparePartModel
                {
                    PartId = reader.GetString(reader.GetOrdinal("PartId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Unit = reader.GetString(reader.GetOrdinal("Unit")),
                    Manufacture = reader.GetString(reader.GetOrdinal("Manufacture")),
                    LotNumber = reader.GetInt32(reader.GetOrdinal("LotNumber")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                    Cost = reader.GetDecimal(reader.GetOrdinal("Cost")),
                    Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                    TradeIncrease = reader.GetInt32(reader.GetOrdinal("TradeIncrease")),
                    DeliveryDate = reader.IsDBNull(reader.GetOrdinal("DeliveryDate")) ? null : reader.GetDateTime(reader.GetOrdinal("DeliveryDate")),
                    PersonnelNumber = reader.GetInt32(reader.GetOrdinal("PersonnelNumber"))
                };
                spareParts.Add(sparePart);
            }

            int countOfRows = (int)command.Parameters["@CountOfRows"].Value;

            return new GridItem
            {
                Data = spareParts,
                TotalItems = countOfRows,
                CurrentPage = pageNumber ?? 1,
                ItemsPerPage = pageSize
            };

            //return (countOfRows, spareParts);
        }
    }
}