using CarServiceApp.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarServiceApp.Domain.Tests.Validation
{
    public class EmailValidationAttributeTests
    {
        [Theory]
        [InlineData("test@example")]
        [InlineData("invalidemail")] // Invalid email
        [InlineData("")] // Empty email
        [InlineData(null)] // Null email
        public void IsValid_Returns_Expected_Result(string email)
        {
            // Arrange
            var validationContext = new ValidationContext(new object());
            var attribute = new EmailValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(email, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Success_For_Valid_Email()
        {
            // Arrange
            var email = "test@example.com";
            var validationContext = new ValidationContext(new object());
            var attribute = new EmailValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(email, validationContext);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Invalid_Email()
        {
            // Arrange
            var invalidEmail = "invalidemail";
            var validationContext = new ValidationContext(new object());
            var attribute = new EmailValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(invalidEmail, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Null_Email()
        {
            // Arrange
            string nullEmail = null;
            var validationContext = new ValidationContext(new object());
            var attribute = new EmailValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(nullEmail, validationContext);

            // Assert
            Assert.NotNull(result);
        }
    }
}