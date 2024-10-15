using CarServiceApp.Domain.Entity.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CarServiceApp.Domain.Validation;

namespace CarServiceApp.Domain.Entity.Models;

public class TypeOfPayment : ICommonEntity<int>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ScaffoldColumn(false)]
    [Column("PaymentId")]
    [Display(Name = "Номер")]
    public int Id { get; set; }

    [Column("PaymentName")]
    [Display(Name = "Наименование")]
    [NameCyrillicOnlyValidation]
    public string Name { get; set; }

    public virtual ICollection<OrderService> OrderServices { get; set; } = [];
}
