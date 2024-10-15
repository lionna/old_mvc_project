using CarServiceApp.Domain.Entity.Abstract;
using CarServiceApp.Domain.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarServiceApp.Domain.Entity.Models;

public class CarColor: ICommonEntity<int>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Column("ColorId")]
    [Display(Name = "Номер")]
    public int Id { get; set; }

    [Display(Name = "Цвет")]
    public string Hex { get; set; }

    [Display(Name = "Наименование")]
    [NameValidation]
    public string Name { get; set; }

    [Display(Name = "Описание")]
    public string Description { get; set; }

    [Display(Name = "Металик?")]
    public bool IsMetallic { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = [];
}
