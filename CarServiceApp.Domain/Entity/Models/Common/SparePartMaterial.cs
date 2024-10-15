using CarServiceApp.Domain.Entity.Abstract;
using CarServiceApp.Domain.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarServiceApp.Domain.Entity.Models;

public partial class SparePartMaterial : ICommonEntity<string>
{
    public SparePartMaterial()
    {
        // Генерация Id при создании экземпляра
        Id = $"SM{DateTime.Now:yyyyMMddHHmmss}";
    }

    [Column("PartId")]
    [Display(Name = "Номер")]
    public string Id { get; set; }

    [Column("Name")]
    [Display(Name = "Наименование")]
    [NameValidation]
    public string Name { get; set; }

    [Display(Name = "Размерность")]
    public string Unit { get; set; }

    [Display(Name = "Производитель")]
    public string Manufacture { get; set; }

    public virtual ICollection<AcceptanceCustomSpart> AcceptanceCustomSparts { get; set; } = [];

    public virtual ICollection<AcceptanceInvoice> AcceptanceInvoices { get; set; } = [];
}
