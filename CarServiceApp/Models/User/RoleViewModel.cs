using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Models.User
{
    public class RoleViewModel
    {
        [Display(Name = "Номер")]
        public string Id { get; set; }
        //[Required(ErrorMessage = "Поле 'Наименование' обязательно для заполнения.")]
        [Display(Name = "Наименование")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина поля 'Наименование' должна быть от 1 до 50 символов.")]
        public string Name { get; set; }
    }

}
