using CarServiceApp.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Tests.Validation
{
    public class PhoneValidationAttributeTests
    {
        [Theory]
        [InlineData("1234567890")] // Valid phone number
        [InlineData("")] // Empty phone number
        [InlineData(null)] // Null phone number
        [InlineData("invalidPhoneNumber")] // Invalid phone number
        public void IsValid_Returns_Expected_Result(string phoneNumber)
        {
            // Arrange
            var validationContext = new ValidationContext(new object());
            var attribute = new PhoneValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(phoneNumber, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Success_For_Valid_Phone_Number()
        {
            // Arrange
            var phoneNumber = "+375291234567";
            var validationContext = new ValidationContext(new object());
            var attribute = new PhoneValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(phoneNumber, validationContext);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Invalid_Phone_Number()
        {
            // Arrange
            var invalidPhoneNumber = "invalidPhoneNumber";
            var validationContext = new ValidationContext(new object());
            var attribute = new PhoneValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(invalidPhoneNumber, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Null_Phone_Number()
        {
            // Arrange
            string nullPhoneNumber = null;
            var validationContext = new ValidationContext(new object());
            var attribute = new PhoneValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(nullPhoneNumber, validationContext);

            // Assert
            Assert.NotNull(result);
        }
    }
}