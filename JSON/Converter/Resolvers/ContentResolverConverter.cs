using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugins.UnityMonstackContentLoader.JSON.Converter;
using Plugins.UnityMonstackCore.DependencyInjections;

namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter.Resolvers
{
    [Inject]
    public class ContentResolverConverter : JsonConverter, ICustomJsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var key = (string) JToken.Load(reader);

            var entityType = objectType.GenericTypeArguments[0];
            var repositoryType = objectType.GenericTypeArguments[1];
            var value = ((IObjectResolver) DependencyProvider.ResolveByType(repositoryType)).Resolve(key);

            return Activator.CreateInstance(typeof(ContentResolver<,>).MakeGenericType(entityType, repositoryType), value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(ContentResolver<,>);
        }
    }
}