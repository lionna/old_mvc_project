using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarServiceApp.Domain.Validation;
using CarServiceApp.Domain.Entity.Abstract;

namespace CarServiceApp.Domain.Entity.Models;

public partial class CarModification : ICommonEntityWithDictinary<int>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Column("ModificationId")]
    [Display(Name = "Номер")]
    public int Id { get; set; }

    [Column("ModificationName")]
    [Display(Name = "Наименование")]
    [NameValidation]
    public string Name { get; set; }

    [Display(Name = "Серия")]
    public int SeriesId { get; set; }
    [Display(Name = "Серия")]
    public virtual CarSeries Series { get; set; }

    [NotMapped]
    public Dictionary<object, string> DropdownList { get; set; }

    [Display(Name = "Дата с")]
    public int? StartProductionYear { get; set; }
    [Display(Name = "Дата по")]
    public int? EndProductionYear { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = [];    
}
