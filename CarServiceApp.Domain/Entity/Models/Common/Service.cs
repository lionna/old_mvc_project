using CarServiceApp.Domain.Entity.Abstract;
using CarServiceApp.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Entity.Models;

public partial class Service : ICommonEntityWithDictinary<string>
{
    public Service()
    {
        // Генерация Id при создании экземпляра
        Id = $"SR{DateTime.Now:yyyyMMddHHmmss}";
    }

    [Key]
    [Column("ServiceId")]
    [Display(Name = "Номер")]
    public string Id { get; set; }

    [Column("ServiceName")]
    [Display(Name = "Наименование")]
    [NameValidation]
    public string Name { get; set; }

    [Display(Name = "Время")]
    public double? RateTime { get; set; }

    [Display(Name = "Цена")]
    public decimal? PriceHour { get; set; }

    [Display(Name = "Тип")]
    public int? TypeId { get; set; }

    [Display(Name = "Доступность")]
    public bool Available { get; set; }

    public virtual ICollection<ExecutingService> ExecutingServices { get; set; } = [];

    [Display(Name = "Тип")]
    public virtual ServiceType ServiceType { get; set; }

    public virtual ICollection<PreRecordService> PreRecordServices { get; set; } = [];

    [NotMapped]
    [Display(Name = "Тип")]
    public Dictionary<object, string> DropdownList { get; set; }
}
