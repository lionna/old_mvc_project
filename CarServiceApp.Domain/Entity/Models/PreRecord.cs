using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Entity.Models;

public partial class PreRecord
{
    [Display(Name = "Номер")]
    public long RecordId { get; set; }
    [Display(Name = "ФИО клиента")]
    public string FullName { get; set; }
    [Display(Name = "Марка/модель авто")]
    public string MarkModelCar { get; set; }
    [Display(Name = "Год выпуска")]
    public int IssueYear { get; set; }
    [Display(Name = "Дата заявки")]
    public DateTime DateMakingRecord { get; set; }
    [Display(Name = "Номер")]
    public int PersonnelNumber { get; set; }
    [Display(Name = "Рег. номер авто")]
    public string RegNumberCar { get; set; }
    [Display(Name = "Номер телефона")]
    public string PhoneNumber { get; set; }
    [Display(Name = "Статус заявки")]
    public bool? IsRejection { get; set; }
    [Display(Name = "Сотрудник")]
    public virtual Employee Employee { get; set; }
    [Display(Name = "Записи")]
    public virtual ICollection<PreRecordService> PreRecordServices { get; set; } = [];
}
