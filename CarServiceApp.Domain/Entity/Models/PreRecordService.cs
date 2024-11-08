namespace CarServiceApp.Domain.Entity.Models;

public partial class PreRecordService
{
    public long RecordId { get; set; }

    public string ServiceId { get; set; } = null!;

    public int? PersonnelNumber { get; set; }

    public DateTime? DateReservation { get; set; }

    public virtual PreRecord PreRecord { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual Employee Employee { get; set; }
}
