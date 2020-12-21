using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Plugins.UnityMonstackCore.Loggers;
using Plugins.UnityMonstackCore.Utils;
using UnityEngine;

namespace Plugins.UnityMonstackContentLoader.JSON
{
    public abstract class
        AbstractJSONContentListRepository<TKey, TEntity> : AbstractContentListRepository<TKey, TEntity>
        where TEntity : class
    {
        protected abstract JsonConverter[] CustomConverters { get; }
        protected JsonSerializerSettings JsonSerializerSettings { get; }

        protected AbstractJSONContentListRepository(string filePath, bool loadImmediately = false) : base(filePath)
        {
            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            JsonSerializerSettings.Formatting = Formatting.Indented;

            if (loadImmediately)
                Reload();
        }

        public override void Save()
        {
            if (!hasPendingChanges)
                return;

            var dataAsJson = JsonConvert.SerializeObject(new JSONDeserializedList<TEntity>
            {
                entries = entries.Values.ToList()
            }, JsonSerializerSettings);

            LocalStorageUtils.SaveBytesToFile(FileSourceType.Resources, FilePath, new UTF8Encoding().GetBytes(dataAsJson));
        }

        public override void Reload()
        {
            try
            {
                var dataAsJson = ReadJson();
                var deserializedList = JsonConvert.DeserializeObject<JSONDeserializedList<TEntity>>(dataAsJson, CustomConverters);
                deserializedList.entries.ForEach(entity =>
                {
                    entries[GetEntityID(entity)] = entity;

                    if (entity is IContentEntity contentEntity) contentEntity.Initialize();
                });
            }
            catch (Exception e)
            {
                UnityLogger.Error($"Failed to load JSON from {FilePath} because", e);
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