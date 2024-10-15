using CarServiceApp.Domain.Entity.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarServiceApp.Domain.Validation;

namespace CarServiceApp.Domain.Entity.Models;

public class CarModel : ICommonEntityWithDictinary<int>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Column("ModelId")]
    [Display(Name = "Номер")]
    public int Id { get; set; }

    [Column("ModelName")]
    [Display(Name = "Наименование")]
    [NameValidation]
    public string Name { get; set; }

    [Display(Name = "Бренд")]
    public int BrandId { get; set; }

    [Display(Name = "Бренд")]
    public virtual CarBrand CarBrand { get; set; } = null!;

    public virtual ICollection<CarGeneration> CarGenerations { get; set; } = [];

    public virtual ICollection<CarSeries> CarSeries { get; set; } = [];

    [NotMapped]
    public Dictionary<object,string> DropdownList { get; set; }
}
