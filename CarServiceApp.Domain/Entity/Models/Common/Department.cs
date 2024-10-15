using CarServiceApp.Domain.Entity.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarServiceApp.Domain.Validation;

namespace CarServiceApp.Domain.Entity.Models;

public class Department : ICommonEntity<int>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Column("DepartmentId")]
    [Display(Name = "Номер")]
    public int Id { get; set; }

    [Column("DepartmentName")]
    [Display(Name = "Наименование")]
    [NameCyrillicOnlyValidation]
    public string Name { get; set; }

    public int? ChiefPersonalNumber { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = [];
}
