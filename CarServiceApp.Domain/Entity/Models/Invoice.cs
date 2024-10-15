namespace CarServiceApp.Domain.Entity.Models;

public partial class Invoice
{
    public int LotNumber { get; set; }

    public string ProviderId { get; set; } = null!;

    public DateOnly? DeliveryDate { get; set; }

    public int? PersonnelNumber { get; set; }

    public virtual ICollection<AcceptanceInvoice> AcceptanceInvoices { get; set; } = [];

    public virtual Provider Provider { get; set; } = null!;

    public virtual Employee Employee { get; set; }
}
