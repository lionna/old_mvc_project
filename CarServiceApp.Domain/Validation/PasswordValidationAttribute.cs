using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class PasswordValidationAttribute : ValidationAttribute
    {
        private readonly ValidationService validationService = new();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;
            try
            {
                validationService.PasswordIsValid(password);
                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                return new ValidationResult(ex.Message);
            }
        }
    }
}