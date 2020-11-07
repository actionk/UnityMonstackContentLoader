using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter
{
    public class ConditionalJsonConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var attribute =
                (ConditionalJsonConverterAttribute) Attribute.GetCustomAttribute(objectType,
                    typeof(ConditionalJsonConverterAttribute), false);
            if (attribute == null)
            {
                var instance = Activator.CreateInstance(objectType);
                serializer.Populate(reader, instance);
                return instance;
            }

            var jObject = JObject.Load(reader);
            var propertyValue = (string) jObject[attribute.PropertyName];
            var typeToCastTo = FindConvertObjectType(attribute, propertyValue);

            return jObject.ToObject(typeToCastTo);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        private Type FindConvertObjectType(ConditionalJsonConverterAttribute attribute, string propertyValue)
        {
            for (var i = 0; i < attribute.PropertyValues.Length; i++)
                if (attribute.PropertyValues[i] == propertyValue)
                    return attribute.ConvertableTypes[i];

            throw new NotSupportedException($"Property value {propertyValue} is not defined in {attribute}");
        }
    }
}