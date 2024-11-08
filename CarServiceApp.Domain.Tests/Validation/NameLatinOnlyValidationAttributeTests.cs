using CarServiceApp.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceApp.Domain.Tests.Validation
{
    public class NameLatinOnlyValidationAttributeTests
    {
        [Theory]
        [InlineData(" ")]
        [InlineData("ValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValid")] // Valid name with maximum length
        [InlineData("")] // Empty name
        [InlineData(null)] // Null name
        [InlineData("Невалидноеимя")] // Non-latin characters in name
        public void IsValid_Returns_Expected_Result(string name)
        {
            // Arrange
            var validationContext = new ValidationContext(new object());
            var attribute = new NameLatinOnlyValidationAttribute();

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
            var attribute = new NameLatinOnlyValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(name, validationContext);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Long_Name()
        {
            // Arrange
            var longName = "ValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLongLengthValidNameWithLong";
            var validationContext = new ValidationContext(new object());
            var attribute = new NameLatinOnlyValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(longName, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Non_Latin_Characters_In_Name()
        {
            // Arrange
            var nonLatinName = "Невалидноеимя";
            var validationContext = new ValidationContext(new object());
            var attribute = new NameLatinOnlyValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(nonLatinName, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Null_Name()
        {
            // Arrange
            string nullName = null;
            var validationContext = new ValidationContext(new object());
            var attribute = new NameLatinOnlyValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(nullName, validationContext);

            // Assert
            Assert.NotNull(result);
        }
    }
}