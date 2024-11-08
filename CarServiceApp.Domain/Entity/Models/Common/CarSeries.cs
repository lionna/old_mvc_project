using CarServiceApp.Domain.Entity.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarServiceApp.Domain.Validation;

namespace CarServiceApp.Domain.Entity.Models;

public partial class CarSeries : ICommonEntityWithDictinary<int>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Column("SeriesId")]
    [Display(Name = "Номер")]
    public int Id { get; set; }

    [Column("SeriesName")]
    [Display(Name = "Наименование")]
    [NameValidation]
    public string Name { get; set; }

    public int ModelId { get; set; }

    public int? GenerationId { get; set; }

    public virtual ICollection<CarModification> CarModifications { get; set; } = [];

    public virtual CarGeneration CarGeneration { get; set; }

    public virtual CarModel CarModel { get; set; } = null!;

    [NotMapped]
    public Dictionary<object, string> DropdownList { get; set; }
}
