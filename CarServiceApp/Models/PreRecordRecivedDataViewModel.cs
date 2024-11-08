using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Models
{
    public class PreRecordRecivedDataViewModel
    {
        [Required]
        [Display(Name = "ФИО клиента")]
        public string FullName { set; get; }
        [Display(Name = "Тел. номер")]
        public string PhoneNumber { set; get; }
        [Display(Name = "Марка/модель авто")]
        [Required]
        public string MarkModelCar { set; get; }
        [Display(Name = "Год выпуска")]
        public int? IssueYear { set; get; }
        [Display(Name = "Рег. номер авто")]
        public string RegNumberCar { set; get; }
        [Required]
        public string[] ServiceIds { set; get; }

    }
}