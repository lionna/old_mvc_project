using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Models
{
    public class CarEditViewModel
    {
        [Display(Name = "Бренд")]
        public int? BrandId { get; set; }
        [Display(Name = "Бренд")]
        public SelectList CarBrands { get; set; }
        [Display(Name = "Модель")]
        public int? ModelId { get; set; }
        [Display(Name = "Модель")]
        public SelectList ModelCarList { get; set; }
        [Display(Name = "Серия")]
        public int? SeriesId { get; set; }
        [Display(Name = "Серия")]
        //[Required(ErrorMessage = "Укажите серию")]
        public SelectList SeriesCarList { get; set; }
        [Display(Name = "Модификация")]
        //[Required(ErrorMessage = "Укажите модификацию")]
        public int? ModificationId { get; set; }
        [Display(Name = "Модификация")]
        public SelectList CarModifications { get; set; }
        [Required(ErrorMessage = "Укажите регистрационный номер")]
        [Display(Name = "Регистрационный номер")]
        public string RegistrationNumber { get; set; }
        [Display(Name = "Цвет")]
        public int? ColorId { get; set; }
        [Display(Name = "Цвет")]
        public SelectList BodyColorList { get; set; }
        [Required(ErrorMessage = "Укажите год выпуска")]
        [Display(Name = "Год выпуска")]
        public int IssueYear { get; set; }
        [Display(Name = "Год выпуска")]
        public SelectList YearList { get; set; }       
        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN номер состоит из 17 символов")]
        [Display(Name = "VIN номер кузова")]
        [Required(ErrorMessage = "Введите VIN номер")]
        public string VinNumber { get; set; }
        [Required(ErrorMessage = "Укажите владельца")]
        [Display(Name = "Ф.И.О. владельца")]
        public string OwnerName { get; set; }
        [Display(Name = "Номер техпаспорта")]
        [Required(ErrorMessage = "Укажите номер тех. паспорта")]
        public string DataSheetCar { get; set; }
        public SelectList TransmissionTypeList { get; set; }
        [Display(Name = "Тип КПП")]
        public string TransmissionType { get; set; }
    }
}
