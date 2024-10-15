using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class NameCyrillicOnlyValidationAttribute : ValidationAttribute
    {
        private readonly ValidationService validationService = new();
        private const string TextName = "Наименование";
        private const int MinValue = 1;
        private const int MaxValue = 100;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value as string;
            try
            {
                validationService.NameCyrillicOnlyIsValid(name, TextName, MinValue, MaxValue);
                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                return new ValidationResult(ex.Message);
            }
        }
    }
}