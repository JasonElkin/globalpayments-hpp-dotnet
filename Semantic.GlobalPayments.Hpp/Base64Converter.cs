using Newtonsoft.Json;
using System;
using System.Text;

namespace Semantic.GlobalPayments.Hpp
{
    public class Base64Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string) ||
                   objectType.IsEnum ||
                   objectType == typeof(bool) ||
                   objectType == typeof(int?);
        }

        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object value = null;
            string stringValue = Encoding.UTF8.GetString(Convert.FromBase64String((string)reader.Value));

            if (objectType == typeof(string))
            {
                return stringValue;
            }

            if (objectType.IsEnum)
            {
                value = Enum.Parse(objectType, stringValue as string);
            }
            else if (objectType == typeof(bool))
            {
                value = bool.Parse(stringValue);
            }

            if (objectType == typeof(int?))
            {
                if (int.TryParse(stringValue, out int parsed))
                {
                    value = parsed;
                }
            }

            return value;
        }

        // This wont currently be called as CanWrite is set to false above.
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueType = value.GetType();
            if (valueType.IsEnum)
            {
                value = ((Enum)value).ToString("F");
            }
            else if (valueType == typeof(bool))
            {
                value = ((bool)value).ToString();
            }
            else if (valueType == typeof(int?))
            {
                value = value.ToString();
            }

            writer.WriteValue(Convert.ToBase64String(Encoding.UTF8.GetBytes((string)value)));
        }
    }
}
