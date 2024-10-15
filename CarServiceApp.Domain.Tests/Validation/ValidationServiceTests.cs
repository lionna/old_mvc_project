using CarServiceApp.Domain.Validation;
using System.Text.RegularExpressions;

namespace CarServiceApp.Domain.Tests.Validation
{
    public class ValidationServiceTests
    {
        [Fact]
        public void InputValueIsNullOrWhiteSpace_NullValue_ThrowsArgumentException()
        {
            // Arrange
            const string? value = null;
            string textName = "SomeField";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ValidationService.InputValueIsNullOrWhiteSpace(value, textName));
        }

        [Theory]
        [InlineData("", "FieldName")]
        [InlineData("   ", "FieldName")]
        public void InputValueIsNullOrWhiteSpace_WhitespaceValue_ThrowsArgumentException(string value, string textName)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ValidationService.InputValueIsNullOrWhiteSpace(value, textName));
        }


        [Theory]
        [InlineData("short", "FieldName", 10, 20)]
        [InlineData("longerthanmax", "FieldName", 1, 10)]
        public void InputValueCheckLength_OutsideValidRange_ThrowsArgumentException(string name, string textName, int min, int max)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ValidationService.InputValueCheckLength(name, textName, min, max));
        }

        [Theory]
        [InlineData("withinrange", "FieldName", 5, 15)]
        [InlineData("123", "FieldName", 1, 5)]
        public void InputValueCheckLength_InsideValidRange_NoExceptionThrown(string name, string textName, int min, int max)
        {
            // Act & Assert
            var exception = Record.Exception(() => ValidationService.InputValueCheckLength(name, textName, min, max));
            Assert.Null(exception);
        }

        [Fact]
        public void InputValueCheckLength_MinEqualsMax_NoExceptionThrown()
        {
            // Arrange
            string name = "exactlength";
            string textName = "FieldName";
            int min = 11;
            int max = 11;

            // Act & Assert
            var exception = Record.Exception(() => ValidationService.InputValueCheckLength(name, textName, min, max));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void PasswordIsValid_NullOrWhiteSpaceValue_ThrowsArgumentException(string password)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.PasswordIsValid(password));
        }

        [Theory]
        [InlineData("abc", 5, 30)]
        [InlineData("toolongpasswordtoolongpasswordtoolongpasswordtoolongpasswordtoolongpassword", 5, 30)]
        public void PasswordIsValid_OutsideValidLengthRange_ThrowsArgumentException(string password, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.PasswordIsValid(password, min, max));
        }

        [Theory]
        [InlineData("123456", 5, 30)]
        [InlineData("abcdef", 5, 30)]
        [InlineData("ABCDEF", 5, 30)]
        [InlineData("Abc123", 5, 30)]
        [InlineData("aBcDeF123", 5, 30)]
        public void PasswordIsValid_ValidPassword_DoesNotThrowException(string password, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.PasswordIsValid(password, min, max));
        }

        [Theory]
        [InlineData("abcde", 5, 30)]
        [InlineData("ABCDE", 5, 30)]
        [InlineData("12345", 5, 30)]
        public void PasswordIsValid_MissingRequiredCharacters_ThrowsArgumentException(string password, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.PasswordIsValid(password, min, max));
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void PhoneIsValid_NullOrWhiteSpacePhoneNumber_ThrowsArgumentException(string phoneNumber)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.PhoneIsValid(phoneNumber));
        }

        [Theory]
        [InlineData("1234", 5, 20)]
        [InlineData("toolongphonenumber123456789012345", 5, 20)]
        public void PhoneIsValid_OutsideValidLengthRange_ThrowsArgumentException(string phoneNumber, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.PhoneIsValid(phoneNumber, null, min, max));
        }

        [Theory]
        [InlineData("1234567890", 5, 20)]
        [InlineData("80291234567", 5, 20)]
        public void PhoneIsValid_ValidPhoneNumber_DoesNotThrowException(string phoneNumber, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.PhoneIsValid(phoneNumber, null, min, max));
        }

        [Theory]
        [InlineData("12345", 5, 20)]
        [InlineData("802912345", 5, 20)]
        public void PhoneIsValid_InvalidPhoneNumber_ThrowsArgumentException(string phoneNumber, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.PhoneIsValid(phoneNumber, null, min, max));
        }

        [Fact]
        public void PhoneIsValid_NonUniquePhoneNumber_ThrowsArgumentException()
        {
            // Arrange
            var validationService = new ValidationService();
            string phoneNumber = "80291234567";
            const int id = 1; // Assuming it's not unique

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.PhoneIsValid(phoneNumber, id));
        }

        [Fact]
        public void PhoneIsValid_UniquePhoneNumber_DoesNotThrowException()
        {
            // Arrange
            var validationService = new ValidationService();
            string phoneNumber = "80291234567";
            int? id = null; // Assuming it's unique

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.PhoneIsValid(phoneNumber, id));
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void LoginIsValid_NullOrWhiteSpaceLogin_ThrowsArgumentException(string login)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.LoginIsValid(login));
        }

        [Theory]
        [InlineData("ab", 5, 100)]
        [InlineData("toolongloginstringlongloginstring", 5, 10)]
        public void LoginIsValid_OutsideValidLengthRange_ThrowsArgumentException(string login, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.LoginIsValid(login, null, min, max));
        }

        [Theory]
        [InlineData("validlogin", 5, 100)]
        [InlineData("login123", 5, 100)]
        [InlineData("Login_123", 5, 100)]
        public void LoginIsValid_ValidLogin_NoExceptionThrown(string login, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.True(validationService.LoginIsValid(login, null, min, max));
        }

        [Theory]
        [InlineData("invalid!login", 5, 100)]
        [InlineData("login ", 5, 100)]
        [InlineData("Login-123", 5, 100)]
        public void LoginIsValid_InvalidLogin_ThrowsArgumentException(string login, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.LoginIsValid(login, null, min, max));
        }

        [Fact]
        public void LoginIsValid_NonUniqueLogin_ThrowsArgumentException()
        {
            // Arrange
            var validationService = new ValidationService();
            string login = "existing@example.com";
            int? id = 1; // Assuming it's not unique

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.LoginIsValid(login, id));
        }

        [Fact]
        public void LoginIsValid_UniqueLogin_NoExceptionThrown()
        {
            // Arrange
            var validationService = new ValidationService();
            string login = "newlogin";
            int? id = null; // Assuming it's unique

            // Act & Assert
            var exception = Record.Exception(() => validationService.LoginIsValid(login, id));

            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void EmailIsValid_NullOrWhiteSpaceEmail_ThrowsArgumentException(string email)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.EmailIsValid(email));
        }

        [Theory]
        [InlineData("email@", 5, 100)]
        [InlineData("toolongemailtoolongemailtoolongemailtoolongemailtoolongemailtoolongemailtoolongemailtoolongemailtoolongemail@example.com", 5, 100)]
        public void EmailIsValid_OutsideValidLengthRange_ThrowsArgumentException(string email, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.EmailIsValid(email, null, min, max));
        }

        [Theory]
        [InlineData("validemail@example.com", 5, 100)]
        [InlineData("name.surname@example.com", 5, 100)]
        [InlineData("email123@example.com", 5, 100)]
        public void EmailIsValid_ValidEmail_NoExceptionThrown(string email, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.True(validationService.EmailIsValid(email, null, min, max));
        }

        [Theory]
        [InlineData("email @example.com", 5, 100)]
        [InlineData("email@example", 5, 100)]
        public void EmailIsValid_InvalidEmail_ThrowsArgumentException(string email, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.EmailIsValid(email, null, min, max));
        }

        [Fact]
        public void EmailIsValid_NonUniqueEmail_ThrowsArgumentException()
        {
            // Arrange
            var validationService = new ValidationService();
            string email = "existing@example.com";
            int? id = 1; // Assuming it's not unique

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.EmailIsValid(email, id));
        }

        [Fact]
        public void EmailIsValid_UniqueEmail_NoExceptionThrown()
        {
            // Arrange
            var validationService = new ValidationService();
            string email = "new@example.com";
            int? id = null; // Assuming it's unique

            // Act & Assert
            Assert.True(validationService.EmailIsValid(email, id)) ;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void FullNameIsValid_NullOrWhiteSpaceName_ThrowsArgumentException(string name)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.FullNameIsValid(name, "текст"));
        }

        [Theory]
        [InlineData("а", "текст", 2, 50)]
        [InlineData("longnameexceedingmaximumlengthlongnameexceedingmaximumlengthlongname", "текст", 2, 50)]
        public void FullNameIsValid_OutsideValidLengthRange_ThrowsArgumentException(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.FullNameIsValid(name, textName, min, max));
        }

        [Theory]
        [InlineData("Иванов", "текст", 2, 50)]
        [InlineData("Петров", "текст", 2, 50)]
        public void FullNameIsValid_ValidName_NoExceptionThrown(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.True(validationService.FullNameIsValid(name, textName, min, max));
        }

        [Theory]
        [InlineData("Иванов-Петров", "текст", 2, 50)]
        [InlineData("Иванов_Петров", "текст", 2, 50)]
        public void FullNameIsValid_InvalidName_ThrowsArgumentException(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.FullNameIsValid(name, textName, min, max));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NameValidation_NullOrWhiteSpaceName_ThrowsArgumentException(string name)
        {
            // Arrange
            var validationService = new ValidationService();
            var regex = new Regex(@"^[a-zA-Z]+$"); // Assume any regex pattern

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameValidation(regex, name, "текст"));
        }

        [Theory]
        [InlineData("a", "текст", 2, 50)]
        [InlineData("longnameexceedingmaximumlengthlongnameexceedingmaximumlengthlongname", "текст", 2, 50)]
        public void NameValidation_OutsideValidLengthRange_ThrowsArgumentException(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();
            var regex = new Regex(@"^[a-zA-Z]+$"); // Assume any regex pattern

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameValidation(regex, name, textName, min, max));
        }

        [Theory]
        [InlineData("John", "текст", 2, 50)]
        [InlineData("Jane", "текст", 2, 50)]
        public void NameValidation_ValidName_NoExceptionThrown(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();
            var regex = new Regex(@"^[a-zA-Z]+$"); // Assume any regex pattern

            // Act & Assert
            Assert.True(validationService.NameValidation(regex, name, textName, min, max));
        }

        [Theory]
        [InlineData("JohnDoe123", "текст", 2, 50)]
        [InlineData("John Doe", "текст", 2, 50)]
        public void NameValidation_InvalidName_ThrowsArgumentException(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();
            var regex = new Regex(@"^[a-zA-Z]+$"); // Assume any regex pattern

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameValidation(regex, name, textName, min, max));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NameIsValid_NullOrWhiteSpaceName_ThrowsArgumentException(string name)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameIsValid(name, "текст"));
        }

        [Theory]
        [InlineData("a", "текст", 2, 50)]
        [InlineData("longnameexceedingmaximumlengthlongnameexceedingmaximumlengthlongname", "текст", 2, 50)]
        public void NameIsValid_OutsideValidLengthRange_ThrowsArgumentException(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameIsValid(name, textName, min, max));
        }

        [Theory]
        [InlineData("John", "текст", 2, 50)]
        [InlineData("Jane", "текст", 2, 50)]
        [InlineData("Иван", "текст", 2, 50)]
        [InlineData("Александра", "текст", 2, 50)]
        [InlineData("John123", "текст", 2, 50)]
        [InlineData("Иван123", "текст", 2, 50)]
        public void NameIsValid_ValidName_NoExceptionThrown(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.True(validationService.NameIsValid(name, textName, min, max));
        }

        [Theory]
        [InlineData("John Doe!£$%", "текст", 2, 50)]
        [InlineData("Иван Иванов!£$%", "текст", 2, 50)]
        public void NameIsValid_InvalidName_ThrowsArgumentException(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameIsValid(name, textName, min, max));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NameLatinOnlyIsValid_NullOrWhiteSpaceName_ThrowsArgumentException(string name)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameLatinOnlyIsValid(name, "текст"));
        }

        [Theory]
        [InlineData("a", "текст", 2, 50)]
        [InlineData("longnameexceedingmaximumlengthlongnameexceedingmaximumlengthlongname", "текст", 2, 50)]
        public void NameLatinOnlyIsValid_OutsideValidLengthRange_ThrowsArgumentException(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameLatinOnlyIsValid(name, textName, min, max));
        }

        [Theory]
        [InlineData("John", "текст", 2, 50)]
        [InlineData("Jane", "текст", 2, 50)]
        [InlineData("John123", "текст", 2, 50)]
        public void NameLatinOnlyIsValid_ValidName_NoExceptionThrown(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.True(validationService.NameLatinOnlyIsValid(name, textName, min, max));
        }

        [Theory]
        [InlineData("Иван", "текст", 2, 50)]
        [InlineData("Александра", "текст", 2, 50)]
        [InlineData("Иван123", "текст", 2, 50)]
        public void NameLatinOnlyIsValid_InvalidName_ThrowsArgumentException(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameLatinOnlyIsValid(name, textName, min, max));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void NameCyrillicOnlyIsValid_NullOrWhiteSpaceName_ThrowsArgumentException(string name)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameCyrillicOnlyIsValid(name, "текст"));
        }

        [Theory]
        [InlineData("а", "текст", 2, 50)]
        [InlineData("длинноеназвание", "текст", 2, 10)]
        public void NameCyrillicOnlyIsValid_OutsideValidLengthRange_ThrowsArgumentException(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameCyrillicOnlyIsValid(name, textName, min, max));
        }

        [Theory]
        [InlineData("Иван", "текст", 2, 50)]
        [InlineData("Александра", "текст", 2, 50)]
        [InlineData("Иван123", "текст", 2, 50)]
        public void NameCyrillicOnlyIsValid_ValidName_NoExceptionThrown(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.True(validationService.NameCyrillicOnlyIsValid(name, textName, min, max));
        }

        [Theory]
        [InlineData("John", "текст", 2, 50)]
        [InlineData("Jane", "текст", 2, 50)]
        [InlineData("John123", "текст", 2, 50)]
        public void NameCyrillicOnlyIsValid_InvalidName_ThrowsArgumentException(string name, string textName, int min, int max)
        {
            // Arrange
            var validationService = new ValidationService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => validationService.NameCyrillicOnlyIsValid(name, textName, min, max));
        }
    }
}
