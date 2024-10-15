using CarServiceApp.Domain.Entity.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarServiceApp.Domain.Validation;

namespace CarServiceApp.Domain.Entity.Models;

public partial class ImportedStreet : ICommonEntity<int>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Display(Name = "Номер")]
    public int Id { get; set; }

    [Display(Name = "Наименование")]
    [NameCyrillicOnlyValidation]
    public string Name { get; set; }
}
