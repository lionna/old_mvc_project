using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Models
{
    public class OrderViewModel
    {
        [Display(Name = "ФИО клиента")]
        public string FullName { get; set; }
        [HiddenInput]
        public string ClientId { get; set; }
        [Display(Name = "Дата оформления")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        public System.DateTime? DateMakingOrder { get; set; }
        public string Descriptions { get; set; }
        [Range(10, 1000000, ErrorMessage = "Пробег вне диапазона!")]
        [Required(ErrorMessage = "Требуеться ввести пробег")]
        public int? CurrentMileageCar { get; set; }
        [HiddenInput]
        [Required(ErrorMessage = "Требуеться выбрать автомобиль по VIN номеру")]
        public string VinNumber { get; set; }
        public SelectList ListEmplyees { get; set; }
        public int PersonnelNumber { get; set; }
        public long? RecordId { set; get; }
        public int[] NumberWheelCaps { get; set; }
        public int[] NumberWipers { get; set; }
        public int[] NumberWipersArms { get; set; }
        public bool IsAntenna { get; set; }
        public bool IsSpareWheel { get; set; }
        public bool IsCoverDecorEngine { get; set; }
        [Range(0, 100, ErrorMessage = "Неверное значение уровня топлива!")]
        [Required(ErrorMessage = "Требуеться указать уровень топлива")]
        public byte? FluelLevelPercent { get; set; }
        public bool IsTuner { get; set; }
    }
}