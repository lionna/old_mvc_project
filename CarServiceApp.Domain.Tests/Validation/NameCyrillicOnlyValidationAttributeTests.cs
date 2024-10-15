using CarServiceApp.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServiceApp.Domain.Tests.Validation
{
    public class NameCyrillicOnlyValidationAttributeTests
    {
        [Theory]
        [InlineData("£")]
        [InlineData("ВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлиной")] // Valid name with maximum length
        [InlineData("")] // Empty name
        [InlineData(null)] // Null name
        [InlineData("V")] // Non-cyrillic characters in name
        public void IsValid_Returns_Expected_Result(string name)
        {
            // Arrange
            var validationContext = new ValidationContext(new object());
            var attribute = new NameCyrillicOnlyValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(name, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Success_For_Valid_Name()
        {
            // Arrange
            var name = "ВалидноеИмя";
            var validationContext = new ValidationContext(new object());
            var attribute = new NameCyrillicOnlyValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(name, validationContext);

            // Assert
            Assert.Equal(ValidationResult.Success, result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Long_Name()
        {
            // Arrange
            var longName = "ВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлинойВалидноеИмяСДлиннойДлиной";
            var validationContext = new ValidationContext(new object());
            var attribute = new NameCyrillicOnlyValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(longName, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Non_Cyrillic_Characters_In_Name()
        {
            // Arrange
            var nonCyrillicName = "ValidName";
            var validationContext = new ValidationContext(new object());
            var attribute = new NameCyrillicOnlyValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(nonCyrillicName, validationContext);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IsValid_Returns_Validation_Error_For_Null_Name()
        {
            // Arrange
            string nullName = null;
            var validationContext = new ValidationContext(new object());
            var attribute = new NameCyrillicOnlyValidationAttribute();

            // Act
            var result = attribute.GetValidationResult(nullName, validationContext);

            // Assert
            Assert.NotNull(result);
        }
    }
}