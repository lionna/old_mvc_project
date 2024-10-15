namespace CarServiceApp.Domain.Entity.Models;

public partial class FullStockOfSparePart
{
    public string PartId { get; set; } = null!;

    public string Name { get; set; }

    public string Unit { get; set; }

    public string Manufacture { get; set; }

    public decimal? Price { get; set; }

    public int LotNumber { get; set; }

    public decimal? Cost { get; set; }

    public double? Number { get; set; }

    public double? Stock { get; set; }

    public long PositionId { get; set; }

    public byte? TradeIncrease { get; set; }

    public double? StockPercent { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public int? PersonnelNumber { get; set; }

    public string FullName { get; set; }
}
