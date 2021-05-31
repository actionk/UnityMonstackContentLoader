using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugins.UnityMonstackContentLoader.JSON.Converter;
using Plugins.UnityMonstackCore.DependencyInjections;
using Plugins.UnityMonstackCore.Loggers;
using UnityEngine;

namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter.Custom
{
    [Inject]
    public class ColorConverter : JsonConverter<Color>, ICustomJsonConverter
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var colorHash = (string) JToken.Load(reader);

            if (!ColorUtility.TryParseHtmlString(colorHash, out Color color))
            {
                UnityLogger.Error($"Can't parse color from hash [{colorHash}]");
                return default;
            }

            return color;
        }
    }
}