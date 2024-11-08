using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class LoginValidationAttribute : ValidationAttribute
    {
        private readonly ValidationService validationService = new();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var login = value as string;
            try
            {
                validationService.LoginIsValid(login);
                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                return new ValidationResult(ex.Message);
            }
        }
    }
}