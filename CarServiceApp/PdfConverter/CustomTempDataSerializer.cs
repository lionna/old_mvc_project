using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;
using System.Text.Json;
using System.Text;

namespace CarServiceApp.PdfConverter
{
    public class CustomTempDataSerializerHelper : TempDataSerializer
    {
        public override IDictionary<string, object> Deserialize(byte[] unprotectedData)
        {
            if (unprotectedData == null)
            {
                throw new ArgumentNullException(nameof(unprotectedData));
            }

            var jsonString = Encoding.UTF8.GetString(unprotectedData);

            return JsonSerializer.Deserialize<IDictionary<string, object>>(jsonString);
        }

        public override byte[] Serialize(IDictionary<string, object> values)
        {
            ArgumentNullException.ThrowIfNull(values);

            var jsonString = JsonSerializer.Serialize(values);

            return Encoding.UTF8.GetBytes(jsonString);
        }
    }
}
