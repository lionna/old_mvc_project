using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class FullNameValidationAttribute : ValidationAttribute
    {
        private readonly ValidationService validationService = new();
        private const string TextName = "Ф.И.О";
        private const int MinValue = 3;
        private const int MaxValue = 150;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fullName = value as string;
            try
            {
                validationService.FullNameIsValid(fullName, TextName, MinValue, MaxValue);
                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                return new ValidationResult(ex.Message);
            }
        }
    }
}