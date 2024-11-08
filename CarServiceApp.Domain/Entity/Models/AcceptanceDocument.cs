namespace CarServiceApp.Domain.Entity.Models;

public partial class AcceptanceDocument
{
    public int AcceptanceId { get; set; }

    public int PersonnelNumber { get; set; }

    public string ClientId { get; set; } = null!;

    public DateTime? AcceptDate { get; set; }

    public virtual ICollection<AcceptanceCustomSpart> AcceptanceCustomSparts { get; set; } = new List<AcceptanceCustomSpart>();

    public virtual Client Client { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
