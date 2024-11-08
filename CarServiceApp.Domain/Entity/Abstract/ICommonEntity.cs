using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarServiceApp.Domain.Entity.Abstract;

/// <summary>
/// Общая модель Entity
/// </summary>
public interface ICommonEntity<TKey>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Display(Name = "Код")]
    TKey Id { set; get; }

    [Display(Name = "Наименование")]
    string Name { set; get; }
}

/// <summary>
/// Общая модель Entity
/// </summary>
public interface ICommonEntityWithDictinary<TKey> : ICommonEntity<TKey>
{
    [NotMapped]
    public Dictionary<object, string> DropdownList { get; set; }
}
