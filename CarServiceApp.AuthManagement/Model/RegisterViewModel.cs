using CarServiceApp.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.AuthManagement.Model
{
    public class RegisterViewModel
    {
        [LoginValidation]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [EmailValidation]
        [EmailAddress(ErrorMessage = "Некорректный формат адреса электронной почты.")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [PasswordValidation]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [PasswordValidation]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; }

        [PhoneValidation]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Роль")]
        public string Role { get; set; }

        public Dictionary<string, string> AvailableRoles { get; set; }
    }
}
