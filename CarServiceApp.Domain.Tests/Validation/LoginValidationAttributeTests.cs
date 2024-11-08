using CarServiceApp.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceApp.Domain.Tests.Validation
{
    public class LoginValidationAttributeTests
    {
        [Theory]
        [InlineData("l")]
        [InlineData("1")] // Weak login
        [InlineData("")] // Empty login
        [InlineData(null)] // Null login
        [InlineData("invalid*login")] // Invalid login with special characters
        public void IsValid_Returns_Expected_Result(string login)
        {
            // Arrange
            var validationContext = new ValidationContext(new object());
            var attribute = new LoginValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(login, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Success_For_Valid_Login()
        {
            // Arrange
            var login = "valid_login";
            var validationContext = new ValidationContext(new object());
            var attribute = new LoginValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(login, validationContext);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Weak_Login()
        {
            // Arrange
            var weakLogin = "12";
            var validationContext = new ValidationContext(new object());
            var attribute = new LoginValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(weakLogin, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Invalid_Characters_In_Login()
        {
            // Arrange
            var invalidLogin = "invalid*login";
            var validationContext = new ValidationContext(new object());
            var attribute = new LoginValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(invalidLogin, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Null_Login()
        {
            // Arrange
            string nullLogin = null;
            var validationContext = new ValidationContext(new object());
            var attribute = new LoginValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(nullLogin, validationContext);

            // Assert
            Assert.NotNull(result);
        }
    }
}