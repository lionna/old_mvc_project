using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Model
{
    public class CarViewModel
    {
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Series { get; set; }
        public string Modification { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Регистрационный номер")]
        public string RegistrationNumber { get; set; }

        public string ColorCode { get; set; }
        public string PaintName { get; set; }
        public string ColorName { get; set; }

        [Display(Name = "Год выпуска")]
        public int? IssueYear { get; set; }

        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN номер состоит из 17 символов")]
        [Display(Name = "VIN номер кузова")]
        public string VinNumber { get; set; }

        [Display(Name = "Ф.И.О. владельца")]
        public string OwnerName { get; set; }

        [Display(Name = "Номер техпаспорта")]
        [Required(ErrorMessage = "Обязательное поле")]
        public string DataSheetCar { get; set; }

        [Display(Name = "Тип КПП")]
        public string TransmissionType { get; set; }

        public byte[] ViewCar { get; set; }
        public int? CountRepare { get; set; }
    }
}