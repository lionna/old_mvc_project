using CarServiceApp.Domain.Entity.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarServiceApp.Domain.Validation;

namespace CarServiceApp.Domain.Entity.Models;

public class ServiceType : ICommonEntity<int>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Column("TypeId")]
    [Display(Name = "Номер")]
    public int Id { get; set; }

    [Column("TypeName")]
    [Display(Name = "Наименование")]
    [NameCyrillicOnlyValidation]
    public string Name { get; set; }

    public virtual ICollection<Service> Services { get; set; } = [];

    public virtual ICollection<Post> Posts { get; set; } = [];
}
