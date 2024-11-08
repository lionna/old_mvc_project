using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Entity.Models;

public partial class Employee
{
    [Display(Name = "Номер")]
    public int PersonnelNumber { get; set; }
    [Display(Name = "Адрес")]
    public string ResidentialAddress { get; set; }
    [Display(Name = "Телефон")]
    public string Phone { get; set; }
    [Display(Name = "ФИО сотрудника")]
    public string FullName { get; set; }
    [Display(Name = "Дата рождения")]
    public DateOnly? DateBirthday { get; set; }
    [Display(Name = "Идентификационный номер паспорта")]
    public string PassportPrivateNumber { get; set; }
    [Display(Name = "Серия паспорта")]
    public string PassportSeries { get; set; }
    [Display(Name = "Номер паспорта")]
    public int? PassportNumber { get; set; }

    public string PassportIssuedBy { get; set; }

    public DateTime? PassportIssuedDate { get; set; }

    public DateTime? PassportValidityDate { get; set; }

    public int? DepartmentId { get; set; }

    public int PostId { get; set; }

    public virtual ICollection<AcceptanceDocument> AcceptanceDocuments { get; set; } = [];

    public virtual ICollection<ContractToEmployee> ContractToEmployees { get; set; } = [];

    public virtual ICollection<ExecutingService> ExecutingServices { get; set; } = [];

    public virtual Department Department { get; set; }

    public virtual Post Post { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = [];

    public virtual ICollection<PreRecordService> PreRecordServices { get; set; } = [];

    public virtual ICollection<PreRecord> PreRecords { get; set; } = [];

    public virtual ICollection<UsingPartMaterial> UsingPartMaterials { get; set; } = [];

    public int? UserId { get; set; }

    //public virtual IdentityUser User { get; set; }
}
