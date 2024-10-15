using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Models.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле 'Имя пользователя' обязательно для заполнения.")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле 'Электронная почта' обязательно для заполнения.")]
        [EmailAddress(ErrorMessage = "Некорректный формат адреса электронной почты.")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения.")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; }

        // Новое свойство для роли
        [Required(ErrorMessage = "Выберите роль.")]
        [Display(Name = "Роль")]
        public string Role { get; set; }

        // Список ролей для выпадающего списка
        public List<string> AvailableRoles { get; set; }
    }
}
