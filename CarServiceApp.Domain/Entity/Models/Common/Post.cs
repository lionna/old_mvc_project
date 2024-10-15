using CarServiceApp.Domain.Entity.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarServiceApp.Domain.Validation;

namespace CarServiceApp.Domain.Entity.Models;

public partial class Post : ICommonEntity<int>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Column("PostId")]
    [Display(Name = "Номер")]
    public int Id { get; set; }

    [Column("NamePost")]
    [Display(Name = "Наименование")]
    [NameCyrillicOnlyValidation]
    public string Name { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = [];

    public virtual ICollection<ServiceType> ServiceTypes { get; set; } = [];
}
