using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Plugins.UnityMonstackCore.Loggers;
using Plugins.UnityMonstackCore.Utils;

namespace Plugins.UnityMonstackContentLoader.JSON
{
    public abstract class
        AbstractJSONContentListRepository<TKey, TEntity> : AbstractContentListRepository<TKey, TEntity>
        where TEntity : class
    {
        protected JsonConverter[] CustomConverters { get; set; }
        protected JsonSerializerSettings JsonSerializerSettings { get; }

        protected AbstractJSONContentListRepository(string filePath, bool loadImmediately = false) : base(filePath)
        {
            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            JsonSerializerSettings.Formatting = Formatting.Indented;

            if (loadImmediately)
                Reload();
        }

        public static TRepository CreateAndLoad<TRepository>()
            where TRepository : AbstractContentListRepository<TKey, TEntity>
        {
            var repository = Activator.CreateInstance<TRepository>();
            repository.Reload();
            return repository;
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
                var dataAsByteArray = LocalStorageUtils.LoadBytesFromFile(FileSource, FilePath);
                var reader = new StreamReader(new MemoryStream(dataAsByteArray));
                var dataAsJson = reader.ReadToEnd();

                var deserializedList = JsonConvert.DeserializeObject<JSONDeserializedList<TEntity>>(dataAsJson, CustomConverters);
                deserializedList.entries.ForEach(entity =>
                {
                    entries[GetEntityID(entity)] = entity;

                    if (entity is IContentEntity contentEntity) contentEntity.Initialize();
                });
            }
            catch (Exception e)
            {
                UnityLogger.Error("Failed to load JSON from {} because {}", FilePath, e);
            }
        }
    }
}