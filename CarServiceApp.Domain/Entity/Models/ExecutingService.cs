namespace CarServiceApp.Domain.Entity.Models;

public partial class ExecutingService
{
    public string ServiceId { get; set; }

    public DateTime? DateCompleting { get; set; }

    public double? TakeTime { get; set; }

    public string OrderId { get; set; } = null!;

    public string Notes { get; set; }

    public decimal? Price { get; set; }

    public int? PersonnelNumber { get; set; }

    public int? TaxAddedValue { get; set; }

    public DateTime? DateStart { get; set; }

    public virtual OrderService OrderService { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual Employee Employee { get; set; }

    public virtual ICollection<UsingCustomSpartMat> UsingCustomSpartMats { get; set; } = [];

    public virtual ICollection<UsingPartMaterial> UsingPartMaterials { get; set; } = [];
}
