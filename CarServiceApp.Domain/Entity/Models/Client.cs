using Microsoft.AspNetCore.Identity;

namespace CarServiceApp.Domain.Entity.Models;

public partial class Client
{
    public string ClientId { get; set; }

    public int? NumberDriveLicense { get; set; }

    public string Phone { get; set; }

    public string Address { get; set; }

    public DateOnly? ExpirationDateLicense { get; set; }

    public string FullName { get; set; }

    public DateOnly? DateBirth { get; set; }

    public virtual ICollection<AcceptanceDocument> AcceptanceDocuments { get; set; } = [];

    public virtual ICollection<OrderService> OrderServices { get; set; } = [];

    public int? UserId { get; set; }

    //public virtual IdentityUser User { get; set; }
}
