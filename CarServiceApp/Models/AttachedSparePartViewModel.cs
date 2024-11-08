using CarServiceApp.Domain.Model;
using CarServiceApp.Domain.Service;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Models
{
    public class AttachedPartsByServiceModel
    {
        public string PartId { get; set; }
        public string Name { get; set; }
        public string Manufacture { get; set; }
        public string Unit { get; set; }
        public double? Number { get; set; }
        public decimal? CostPart { get; set; }
        public int LotNumber { get; set; }
        public decimal? Price { get; set; }
        public long PositionId { get; set; }
    }

    public partial class AttachedCustomPartsByServiceModel
    {
        public string PartId { get; set; }
        public string Name { get; set; }
        public string Manufacture { get; set; }
        public string Unit { get; set; }
        public double? Number { get; set; }
        public string StateSPart { get; set; }
        public int AcceptanceId { get; set; }
    }

    public partial class StockOfSparePartModel
    {
        public string PartId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Manufacture { get; set; }
        public decimal? Price { get; set; }
        public int LotNumber { get; set; }
        public decimal? Cost { get; set; }
        public double? Stock { get; set; }
        public long PositionId { get; set; }
        public byte? TradeIncrease { get; set; }
    }

    public partial class StockCustomOfPartModel
    {
        public string PartId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Manufacture { get; set; }
        public int PersonnelNumber { get; set; }
        public string StateSPart { get; set; }
        public double? StockCustom { get; set; }
        public int AcceptanceId { get; set; }
    }

    public class AttachedSparePartViewModel
    {
        public string ServiceName { set; get; }
        public string ClientId { get; set; }
        public string OrderId { get; set; }
        public string ServiceId { get; set; }
        public string PartId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Manufacture { get; set; }
        [Required(ErrorMessage = "!!!")]
        [Range(0.1, 150.01, ErrorMessage = "1...150")]
        public double Number { get; set; }
        public IEnumerable<AttachedPartInfo> AttachedPartsByService { set; get; }
        public IEnumerable<AttachedCustomPartInfo> AttachedCustomPartsByService { set; get; }
        public IQueryable<StockOfSparePartModel> StockOfParts { get; set; }
        public IEnumerable<StockCustomOfPartModel> StockCustomOfParts { get; set; }
    }
}
