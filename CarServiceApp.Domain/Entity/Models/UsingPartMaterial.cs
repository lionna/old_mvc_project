namespace CarServiceApp.Domain.Entity.Models;

public partial class UsingPartMaterial
{
    public double? Number { get; set; }

    public long PositionId { get; set; }

    public string ServiceId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public decimal? CostPart { get; set; }

    public int? PersonnelNumber { get; set; }

    public virtual ExecutingService ExecutingService { get; set; } = null!;

    public virtual AcceptanceInvoice AcceptanceInvoice { get; set; } = null!;

    public virtual Employee Employee { get; set; }
}
