using CarServiceApp.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceApp.Domain.Tests.Validation
{
    public class FullNameValidationAttributeTests
    {
        [Theory]
        [InlineData("John Doe")] // Valid full name
        [InlineData("Jane Smith-Johnson")] // Valid full name with hyphen
        [InlineData("")] // Empty full name
        [InlineData(null)] // Null full name
        [InlineData("John")] // Too short full name
        [InlineData("Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters")] // Too long full name
        [InlineData("1")] // Number
        public void IsValid_Returns_Expected_Result(string fullName)
        {
            // Arrange
            var validationContext = new ValidationContext(new object());
            var attribute = new FullNameValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(fullName, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Success_For_Valid_Full_Name()
        {
            // Arrange
            var fullName = "Иван Иванов";
            var validationContext = new ValidationContext(new object());
            var attribute = new FullNameValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(fullName, validationContext);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Short_Full_Name()
        {
            // Arrange
            var shortFullName = "John";
            var validationContext = new ValidationContext(new object());
            var attribute = new FullNameValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(shortFullName, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Long_Full_Name()
        {
            // Arrange
            var longFullName = "Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters Very Long Full Name With Many Words And Characters";
            var validationContext = new ValidationContext(new object());
            var attribute = new FullNameValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(longFullName, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Null_Full_Name()
        {
            // Arrange
            string nullFullName = null;
            var validationContext = new ValidationContext(new object());
            var attribute = new FullNameValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(nullFullName, validationContext);

            // Assert
            Assert.NotNull(result);
        }
    }
}