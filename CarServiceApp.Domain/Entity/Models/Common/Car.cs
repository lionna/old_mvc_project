namespace CarServiceApp.Domain.Entity.Models;

public partial class Car
{
    public string RegistrationNumber { get; set; }

    public int? IssueYear { get; set; }

    public string VinNumber { get; set; } = null!;

    public string OwnerName { get; set; }

    public string DataSheetCar { get; set; }

    public string TransmissionType { get; set; }

    public byte[] ViewCar { get; set; }

    public int? ModificationId { get; set; }

    public int? ColorId { get; set; }

    public virtual CarColor CarColor { get; set; }

    public virtual CarModification CarModification { get; set; }

    public virtual ICollection<OrderService> OrderServices { get; set; } = [];
}
