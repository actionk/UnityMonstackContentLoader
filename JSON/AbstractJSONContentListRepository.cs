using System;
using System.IO;
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

        protected AbstractJSONContentListRepository(string filePath) : base(filePath)
        {
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