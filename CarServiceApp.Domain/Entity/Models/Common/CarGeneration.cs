using CarServiceApp.Domain.Entity.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarServiceApp.Domain.Validation;

namespace CarServiceApp.Domain.Entity.Models;

public class CarGeneration : ICommonEntityWithDictinary<int>
{
    //public int GenerationId { get; set; }

    //public string GenerationName { get; set; }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Column("GenerationId")]
    [Display(Name = "Номер")]
    public int Id { get; set; }

    [Column("GenerationName")]
    [Display(Name = "Наименование")]
    [NameValidation]
    public string Name { get; set; }

    [Display(Name = "Модель")]
    public int ModelId { get; set; }

    [Display(Name = "Год начала")]
    public int? YearBegin { get; set; }

    [Display(Name = "Год окончания")]
    public int? YearEnd { get; set; }

    [Display(Name = "Серии")]
    public virtual ICollection<CarSeries> CarSeries { get; set; } = [];

    [Display(Name = "Модель")]
    public virtual CarModel CarModel { get; set; } = null!;

    [NotMapped]
    [Display(Name = "Модель")]
    public Dictionary<object, string> DropdownList { get; set; }
}
