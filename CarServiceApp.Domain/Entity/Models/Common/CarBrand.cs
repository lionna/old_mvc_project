using CarServiceApp.Domain.Entity.Abstract;
using CarServiceApp.Domain.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarServiceApp.Domain.Entity.Models;

public class CarBrand: ICommonEntity<int>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Column("BrandId")]
    [Display(Name = "Номер")]
    public int Id { get; set; }

    [Column("BrandName")]
    [Display(Name = "Наименование")]
    [NameValidation]
    public string Name { get; set; }

    public byte[] ViewBrand { get; set; }

    public virtual ICollection<CarModel> CarModels { get; set; } = [];
}
