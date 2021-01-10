using System;
using System.IO;
using Newtonsoft.Json;
using Plugins.UnityMonstackCore.Loggers;
using UnityEngine;

namespace Plugins.UnityMonstackContentLoader.JSON
{
    public abstract class AbstractJSONContentListSingleFileRepository<TKey, TEntity> : AbstractContentListRepository<TKey, TEntity>
        where TEntity : class
    {
        protected JsonSerializerSettings JsonSerializerSettings { get; }

        protected AbstractJSONContentListSingleFileRepository(string filePath, bool loadImmediately = false) : base(filePath)
        {
            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            JsonSerializerSettings.Formatting = Formatting.Indented;

            if (loadImmediately)
                Reload();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Reload()
        {
            try
            {
                var path = Application.dataPath + "/Resources/" + FilePath;
                var directory = Directory.EnumerateFiles(path, "*.json", SearchOption.AllDirectories);
                foreach (var file in directory)
                {
                    var filePath = file.Replace(path + "\\", "").Replace(".json", "");
                    var json = Resources.Load<TextAsset>(FilePath + "/" + filePath).text;
                    var entity = JsonConvert.DeserializeObject<TEntity>(json, JsonSerializerSettings);

                    entries[GetEntityID(entity)] = entity;
                    if (entity is IContentEntity contentEntity) contentEntity.Initialize();
                }
            }
            catch (Exception e)
            {
                UnityLogger.Error($"Failed to load JSON from {FilePath} because", e);
            }
        }
    }
}