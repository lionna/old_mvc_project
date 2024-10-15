using System.ComponentModel.DataAnnotations.Schema;

namespace CarServiceApp.Domain.Entity.Models;

public partial class OrderService
{
    public DateTime? DateMakingOrder { get; set; }

    public DateTime? DateCompleting { get; set; }

    public string OrderId { get; set; } = null!;

    public DateTime? DateFactCompleting { get; set; }

    public string ClientId { get; set; } = null!;

    public int? PaymentId { get; set; }

    public string Descriptions { get; set; }

    public int? CurrentMileageCar { get; set; }

    public string VinNumber { get; set; } = null!;

    public string RejectionReason { get; set; }

    public byte? NumberWheelCaps { get; set; }

    public byte? NumberWipers { get; set; }

    public byte? NumberWipersArms { get; set; }

    public bool? IsAntenna { get; set; }

    public bool? IsSpareWheel { get; set; }

    public bool? IsCoverDecorEngine { get; set; }

    public byte FuelLevelPercent { get; set; }

    public bool? IsTuner { get; set; }

    public virtual ICollection<ExecutingService> ExecutingServices { get; set; } = [];

    public virtual Client Client { get; set; } = null!;

    public virtual TypeOfPayment TypeOfPayment { get; set; }

    public virtual ICollection<RemarkToStateCar> RemarkToStateCars { get; set; } = [];

    public virtual Car Car { get; set; } = null!;

    [NotMapped]
    public string Model => $"[{Car?.CarModification?.Series?.CarModel?.CarBrand?.Name}] {Car?.CarModification?.Series?.Name} {Car?.CarModification?.Name}";
}
