using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Models.User
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Поле 'Имя пользователя' обязательно для заполнения.")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле 'Электронная почта' обязательно для заполнения.")]
        [EmailAddress(ErrorMessage = "Некорректный формат адреса электронной почты.")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        // Список ролей для выпадающего списка
        public List<string> AvailableRoles { get; set; }

        // Список текущих ролей пользователя
        public List<string> UserRoles { get; set; }
    }
}
