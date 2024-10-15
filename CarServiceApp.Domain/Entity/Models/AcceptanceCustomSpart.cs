namespace CarServiceApp.Domain.Entity.Models;

public partial class AcceptanceCustomSpart
{
    public string PartId { get; set; }

    public int AcceptanceId { get; set; }

    public double? Number { get; set; }

    public string StateSpart { get; set; }

    public virtual AcceptanceDocument AcceptDocument { get; set; } = null!;

    public virtual SparePartMaterial SparePartMaterial { get; set; } = null!;

    public virtual ICollection<UsingCustomSpartMat> UsingCustomSpartMats { get; set; } = [];
}
