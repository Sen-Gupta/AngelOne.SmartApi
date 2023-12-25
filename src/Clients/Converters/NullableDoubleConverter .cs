using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Converters
{
    public class NullableDoubleConverter : JsonConverter<double?>
    {
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                reader.Read();
                return null;
            }

            if (reader.TryGetDouble(out double value))
            {
                return value;
            }

            if (reader.TryGetInt64(out long longValue))
            {
                return longValue;
            }

            if (reader.TryGetDecimal(out decimal decimalValue))
            {
                return (double)decimalValue;
            }

            throw new JsonException($"Cannot convert {reader.TokenType} to {typeof(double?)}.");
        }
        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
