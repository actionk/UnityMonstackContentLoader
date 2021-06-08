using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugins.UnityMonstackContentLoader.JSON.Converter;
using Plugins.UnityMonstackCore.DependencyInjections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter.Resolvers
{
    [Serializable]
    public class ResourceResolver<T> where T : Object
    {
        public readonly string pathToResource;

        private T m_resource;

        public T Resource
        {
            get
            {
                if (m_resource == null)
                    m_resource = Resources.Load<T>(pathToResource);

                return m_resource;
            }
        }

        public ResourceResolver(string pathToResource)
        {
            this.pathToResource = pathToResource;
        }
    }

    [Inject]
    public class ResourceResolverConverter : JsonConverter, ICustomJsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var path = (string) JToken.Load(reader);
            var resourceType = objectType.GenericTypeArguments[0];

            var type = typeof(ResourceResolver<>).MakeGenericType(resourceType);
            return Activator.CreateInstance(type, path);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(ResourceResolver<>);
        }
    }
}