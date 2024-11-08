using CarServiceApp.Domain.Extension;

namespace CarServiceApp.Domain.Tests.Extension
{
    public class EnumExtensionsTests
    {
        public enum SampleEnum
        {
            Value1,
            Value2,
            Value3
        }

        [Fact]
        public void GetEnumNamesAndValues_ReturnsCorrectValues()
        {
            // Arrange
            string[] expected = { "Value1", "Value2", "Value3" };

            // Act
            string[] result = EnumExtensions.GetEnumNamesAndValues<SampleEnum>();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDictionary_ReturnsCorrectDictionary()
        {
            // Arrange
            Dictionary<int, string> expected = new Dictionary<int, string>
            {
                { 0, "Value1" },
                { 1, "Value2" },
                { 2, "Value3" }
            };

            // Act
            Dictionary<int, string> result = EnumExtensions.ToDictionary<SampleEnum>();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}