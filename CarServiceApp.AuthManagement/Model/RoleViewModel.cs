using CarServiceApp.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.AuthManagement.Model
{
    public class RoleViewModel
    {
        [Display(Name = "Номер")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Поле 'Наименование' обязательно для заполнения.")]
        [Display(Name = "Наименование")]
        [NameLatinOnlyValidation]
        public string Name { get; set; }
    }

}
