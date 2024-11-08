namespace CarServiceApp.Domain.Entity.Models;

public partial class AcceptanceInvoice
{
    public double? Number { get; set; }

    public decimal? Price { get; set; }

    public int LotNumber { get; set; }

    public string PartId { get; set; } = null!;

    public byte? TradeIncrease { get; set; }

    public long PositionId { get; set; }

    public virtual SparePartMaterial SparePartMaterial { get; set; } = null!;

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual ICollection<UsingPartMaterial> UsingPartMaterials { get; set; } = new List<UsingPartMaterial>();
}
