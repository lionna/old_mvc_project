using CarServiceApp.Domain.Entity.Abstract;
using CarServiceApp.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Entity.Models;

public partial class Provider : ICommonEntity<string>
{
    public Provider()
    {
        // Генерация Id при создании экземпляра
        Id = $"PR{DateTime.Now:yyyyMMddHHmmss}";
    }

    [Column("ProviderId")]
    [Display(Name = "Номер")]
    public string Id { get; set; }

    [Display(Name = "Наименование")]
    [NameValidation]
    public string Name { get; set; }

    [Display(Name = "Индекс")]
    public int? ZipCode { get; set; }

    [Display(Name = "Адрес")]
    public string Address { get; set; }

    [PhoneValidation]
    [Display(Name = "Телефон")]
    public string Phone { get; set; }

    [Display(Name = "Номер сертификата")]
    public string CertificateNumber { get; set; }

    [EmailValidation]
    [Display(Name = "Эл. адрес")]
    public string Email { get; set; }

    [Display(Name = "Договора")]
    public virtual ICollection<Invoice> Invoices { get; set; } = [];
}
