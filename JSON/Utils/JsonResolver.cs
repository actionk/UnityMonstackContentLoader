using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugins.UnityMonstackCore.DependencyInjections;

namespace Plugins.UnityMonstackContentLoader.JSON.Utils
{
    public abstract class JsonResolver<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");

        public override bool CanConvert(Type objectType) => objectType == typeof(T);

        public override bool CanWrite => false;

        public T Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        protected abstract bool TryResolveByValue(JToken value, out T output);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var key = JToken.Load(reader);
            if (TryResolveByValue(key, out T output))
                return output;

            throw new ApplicationException($"{GetType()} can't resolve object with key [{key}]");
        }
    }
}