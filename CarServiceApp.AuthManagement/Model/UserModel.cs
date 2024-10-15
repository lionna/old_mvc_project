using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.AuthManagement.Model
{
    public class UserModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Поле 'Имя пользователя' обязательно для заполнения.")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения.")]
        public string Password { get; set; }
    }
}
