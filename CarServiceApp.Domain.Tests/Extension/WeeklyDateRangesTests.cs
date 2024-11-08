using CarServiceApp.Domain.Extension;

namespace CarServiceApp.Domain.Tests.Extension
{
    public class WeeklyDateRangesTests
    {
        [Theory]
        [InlineData("2024-05-01", "2024-04-29")]
        [InlineData("2024-05-05", "2024-04-29")]
        [InlineData("2024-05-06", "2024-05-06")]
        [InlineData("2024-05-07", "2024-05-06")]
        [InlineData("2024-05-08", "2024-05-06")]
        public void GetFirstDayOfWeek_ReturnsCorrectFirstDayOfWeek(string inputDate, string expectedFirstDayOfWeek)
        {
            // Arrange
            DateTime inputDateTime = DateTime.Parse(inputDate);
            DateTime expectedDateTime = DateTime.Parse(expectedFirstDayOfWeek);

            // Act
            DateTime result = inputDateTime.GetFirstDayOfWeek();

            // Assert
            Assert.Equal(expectedDateTime, result);
        }

        [Theory]
        [InlineData("2024-05-01", "2024-05-05 23:59:59")]
        [InlineData("2024-05-05", "2024-05-05 23:59:59")]
        [InlineData("2024-05-06", "2024-05-12 23:59:59")]
        [InlineData("2024-05-07", "2024-05-12 23:59:59")]
        [InlineData("2024-05-08", "2024-05-12 23:59:59")]
        public void GetLastDayOfWeek_ReturnsCorrectLastDayOfWeek(string inputDate, string expectedLastDayOfWeek)
        {
            // Arrange
            DateTime inputDateTime = DateTime.Parse(inputDate);
            DateTime expectedDateTime = DateTime.Parse(expectedLastDayOfWeek);

            // Act
            DateTime result = inputDateTime.GetLastDayOfWeek();

            // Assert
            Assert.Equal(expectedDateTime, result);
        }
    }
}