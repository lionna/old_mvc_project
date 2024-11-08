using CarServiceApp.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Models.User
{
    public class UserSettingsViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Поле 'Имя пользователя' обязательно для заполнения.")]
        [Display(Name = "Имя пользователя")]
        [LoginValidation]      
        public string UserName { get; set; }
        [Required(ErrorMessage = "Поле 'Электронная почта' обязательно для заполнения.")]
        [EmailAddress(ErrorMessage = "Некорректный формат адреса электронной почты.")]
        [Display(Name = "Электронная почта")]
        [EmailValidation]
        [DataType(DataType.EmailAddress, ErrorMessage = "Поле 'Электронная почта' имеет неверный формат.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения.")]
        [DataType(DataType.Password, ErrorMessage = "Поле 'Пароль' оимеет недопустимый формат.")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Поле 'Телефон' обязательно для заполнения.")]
        [Display(Name = "Телефон")]
        [PhoneValidation]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Поле 'Телефон' имеет недопустимый формат.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения.")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}



