using CarServiceApp.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceApp.Domain.Tests.Validation
{
    public class NameValidationAttributeTests
    {
        [Theory]
        [InlineData(" ")]
        [InlineData("ValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLen")] // Valid name with maximum length
        [InlineData("")] // Empty name
        [InlineData(null)] // Null name
        [InlineData("Invalid*Name")] // Invalid name with special characters
        public void IsValid_Returns_Expected_Result(string name)
        {
            // Arrange
            var validationContext = new ValidationContext(new object());
            var attribute = new NameValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(name, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Success_For_Valid_Name()
        {
            // Arrange
            var name = "ValidName";
            var validationContext = new ValidationContext(new object());
            var attribute = new NameValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(name, validationContext);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Long_Name()
        {
            // Arrange
            var longName = "ValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLen";
            var validationContext = new ValidationContext(new object());
            var attribute = new NameValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(longName, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Invalid_Characters_In_Name()
        {
            // Arrange
            var invalidName = "Invalid*Name";
            var validationContext = new ValidationContext(new object());
            var attribute = new NameValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(invalidName, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Null_Name()
        {
            // Arrange
            string nullName = null;
            var validationContext = new ValidationContext(new object());
            var attribute = new NameValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(nullName, validationContext);

            // Assert
            Assert.NotNull(result);
        }
    }
}