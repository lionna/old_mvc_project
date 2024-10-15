using CarServiceApp.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceApp.Domain.Tests.Validation
{
    public class PasswordValidationAttributeTests
    {
        [Theory]
        [InlineData("validPassword")]
        [InlineData("1nval1dPassw0rd")] // Weak password
        [InlineData("")] // Empty password
        [InlineData(null)] // Null password
        public void IsValid_Returns_Expected_Result(string password)
        {
            // Arrange
            var validationContext = new ValidationContext(new object());
            var attribute = new PasswordValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(password, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Success_For_Valid_Password()
        {
            // Arrange
            var password = "strongPassword123_";
            var validationContext = new ValidationContext(new object());
            var attribute = new PasswordValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(password, validationContext);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Weak_Password()
        {
            // Arrange
            var weakPassword = "weak";
            var validationContext = new ValidationContext(new object());
            var attribute = new PasswordValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(weakPassword, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Null_Password()
        {
            // Arrange
            string nullPassword = null;
            var validationContext = new ValidationContext(new object());
            var attribute = new PasswordValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(nullPassword, validationContext);

            // Assert
            Assert.NotNull(result);
        }
    }
}