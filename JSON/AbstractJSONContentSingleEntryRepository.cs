using System;
using System.IO;
using Newtonsoft.Json;
using Plugins.UnityMonstackCore.DependencyInjections;
using Plugins.UnityMonstackCore.Loggers;
using Plugins.UnityMonstackCore.Utils;

namespace Plugins.UnityMonstackContentLoader.JSON
{
    public abstract class AbstractJSONContentSingleEntryRepository<T> : AbstractContentSignleEntryRepository<T>
    {
        protected JsonSerializerSettings JsonSerializerSettings { get; }

        protected AbstractJSONContentSingleEntryRepository(string filePath) : base(filePath)
        {
            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            JsonSerializerSettings.Formatting = Formatting.Indented;
        }

        public override void Reload()
        {
            try
            {
                var dataAsByteArray = LocalStorageUtils.LoadBytesFromFile(FileSource, FilePath);
                var reader = new StreamReader(new MemoryStream(dataAsByteArray));
                var dataAsJson = reader.ReadToEnd();

                Entity = JsonConvert.DeserializeObject<T>(dataAsJson, JsonSerializerSettings);
            }
            catch (Exception e)
            {
                UnityLogger.Error("Failed to load JSON file {} because {}", FilePath, e);
            }
        }
    }
}