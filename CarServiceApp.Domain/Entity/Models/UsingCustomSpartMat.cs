namespace CarServiceApp.Domain.Entity.Models;

public partial class UsingCustomSpartMat
{
    public int AcceptanceId { get; set; }

    public string PartId { get; set; } = null!;

    public string ServiceId { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public double? Number { get; set; }

    public virtual AcceptanceCustomSpart AcceptanceCustomSpart { get; set; } = null!;

    public virtual ExecutingService ExecutingService { get; set; } = null!;
}
