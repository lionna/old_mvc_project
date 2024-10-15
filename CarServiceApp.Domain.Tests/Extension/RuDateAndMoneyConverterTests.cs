using CarServiceApp.Domain.Extension;

namespace CarServiceApp.Domain.Tests.Extension
{
    public class RuDateAndMoneyConverterTests
    {
        [Theory]
        [InlineData(0, "Ноль рублей 00 копеек")]
        [InlineData(1, "Один рубль 00 копеек")]
        [InlineData(10, "Десять рублей 00 копеек")]
        [InlineData(21, "Двадцать один рубль 00 копеек")]
        [InlineData(12345.67, "Двенадцать тысяч триста сорок пять рублей 67 копеек")]
        [InlineData(9876543210.98, "Девять миллиардов восемьсот семьдесят шесть миллионов пятьсот сорок три тысячи двести десять рублей 98 копеек")]
        public void CurrencyToTxt_ReturnsCorrectText(double amount, string expected)
        {
            // Act
            string result = RuDateAndMoneyConverter.CurrencyToTxt(amount, firstCapital: true);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, "ноль")]
        [InlineData(1, "один")]
        [InlineData(10, "десять")]
        [InlineData(21, "двадцать один")]
        [InlineData(12345, "двенадцать тысяч триста сорок пять")]
        [InlineData(9876543210, "девять миллиардов восемьсот семьдесят шесть миллионов пятьсот сорок три тысячи двести десять")]
        public void NumeralsToTxt_Nominative_ReturnsCorrectText(long number, string expected)
        {
            // Act
            string result = RuDateAndMoneyConverter.NumeralsToTxt(number, RuDateAndMoneyConverter.TextCase.Nominative, isMale: true, firstCapital: false);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Иванов Иван Иванович", "Иванов И. И.")]
        [InlineData("Петров Петр", "Петров П.")]
        [InlineData("Сидоров", "Сидоров")]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void ConvertToShortName_ReturnsCorrectShortName(string fullName, string expected)
        {
            // Act
            string result = fullName.ConvertToShortName();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Иванов Иван Иванович", new string[] { "Иванов", "Иван", "Иванович" })]
        [InlineData("Петров Петр", new string[] { "Петров", "Петр", null })]
        [InlineData("Сидоров", new string[] { "Сидоров", null, null })]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void ConvertToStringNames_ReturnsCorrectStringNames(string fullName, string[] expected)
        {
            // Act
            string[] result = fullName.ConvertToStringNames();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}