using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class PhoneValidationAttribute : ValidationAttribute
    {
        private readonly ValidationService validationService = new();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phoneNumber = value as string;
            try
            {
                validationService.PhoneIsValid(phoneNumber);
                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                return new ValidationResult(ex.Message);
            }
        }
    }
}