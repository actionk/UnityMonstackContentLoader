using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugins.UnityMonstackCore.DependencyInjections;
using Plugins.UnityMonstackCore.Loggers;
using UnityEngine;

namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter.Resolvers
{
    [Serializable]
    public class ResourceResolver<T>
    {
        public readonly T resource;

        public ResourceResolver(T resourceToSet)
        {
            resource = resourceToSet;
        }
    }

    [Inject]
    public class ResourceResolverConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var path = (string) JToken.Load(reader);
            var resourceType = objectType.GenericTypeArguments[0];
            var resource = Resources.Load(path, resourceType);
            if (resource == null)
            {
                UnityLogger.Error($"Resource of type {resourceType} can't be loaded from path [{path}]");
                return null;
            }

            var type = typeof(ResourceResolver<>).MakeGenericType(resourceType);
            return Activator.CreateInstance(type, new object[] {resource});
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(ResourceResolver<>);
        }
    }
}