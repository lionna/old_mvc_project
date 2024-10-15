namespace CarServiceApp.Domain.Entity.Models;

public partial class ContractToEmployee
{
    public int ContractId { get; set; }

    public int PersonnelNumber { get; set; }

    public DateOnly RecruitDate { get; set; }

    public DateOnly? DismissDate { get; set; }

    public string Type { get; set; }

    public int? Term { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
