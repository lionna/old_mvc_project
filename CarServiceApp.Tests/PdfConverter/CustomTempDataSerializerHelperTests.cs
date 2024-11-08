using CarServiceApp.PdfConverter;
using System.Text;
using System.Text.Json;

namespace CarServiceApp.Tests.PdfConverter
{
    public class CustomTempDataSerializerHelperTests
    {
        [Fact]
        public void Deserialize_NullUnprotectedData_ThrowsArgumentNullException()
        {
            // Arrange
            var serializerHelper = new CustomTempDataSerializerHelper();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => serializerHelper.Deserialize(null));
        }

        [Fact]
        public void Deserialize_ValidUnprotectedData_ReturnsDictionary()
        {
            // Arrange
            var serializerHelper = new CustomTempDataSerializerHelper();
            var testData = new Dictionary<string, object>
            {
                { "key1", "value1" },
                { "key2", 123 },
                { "key3", true }
            };
            var jsonString = JsonSerializer.Serialize(testData);
            var byteData = Encoding.UTF8.GetBytes(jsonString);

            // Act
            var result = serializerHelper.Deserialize(byteData);
            var expectedJsonString = JsonSerializer.Serialize(testData);
            var expectedDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(expectedJsonString);

            // Assert
            Assert.Equal(JsonSerializer.Serialize(expectedDictionary), JsonSerializer.Serialize(result));
        }

        [Fact]
        public void Serialize_ValidValues_ReturnsByteData()
        {
            // Arrange
            var serializerHelper = new CustomTempDataSerializerHelper();
            var testData = new Dictionary<string, object>
            {
                { "key1", "value1" },
                { "key2", 123 },
                { "key3", true }
            };
            var expectedJsonString = JsonSerializer.Serialize(testData);
            var expectedByteData = Encoding.UTF8.GetBytes(expectedJsonString);

            // Act
            var result = serializerHelper.Serialize(testData);

            // Assert
            Assert.Equal(expectedByteData, result);
        }
    }
}