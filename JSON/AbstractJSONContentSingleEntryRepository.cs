using System;
using System.IO;
using Newtonsoft.Json;
using Plugins.UnityMonstackCore.DependencyInjections;
using Plugins.UnityMonstackCore.Loggers;
using Plugins.UnityMonstackCore.Utils;
using UnityEngine;

namespace Plugins.UnityMonstackContentLoader.JSON
{
    public abstract class AbstractJSONContentSingleEntryRepository<T> : AbstractContentSignleEntryRepository<T>
    {
        protected JsonSerializerSettings JsonSerializerSettings { get; }

        public string Path { get; }
        
        protected AbstractJSONContentSingleEntryRepository(string filePath) : base(filePath)
        {
            Path = filePath;
            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            JsonSerializerSettings.Formatting = Formatting.Indented;
        }

        public override void Reload()
        {
            try
            {
                var dataAsJson = ReadJson();
                Entity = JsonConvert.DeserializeObject<T>(dataAsJson, JsonSerializerSettings);

                if (Entity is IContentEntity contentEntity) contentEntity.Initialize();
            }
            catch (Exception e)
            {
                UnityLogger.Error("Failed to load JSON file {} because {}", FilePath, e);
            }
        }

        private string ReadJson()
        {
            switch (FileSource)
            {
                case FileSourceType.Resources:
                    return Resources.Load<TextAsset>(FilePath).text;

                case FileSourceType.ApplicationPersistentData:
                    var dataAsByteArray = LocalStorageUtils.LoadBytesFromFile(FileSource, FilePath);
                    var reader = new StreamReader(new MemoryStream(dataAsByteArray));
                    var dataAsJson = reader.ReadToEnd();
                    return dataAsJson;
            }

            throw new NotImplementedException();
        }
    }
}