﻿using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Plugins.Shared.UnityMonstackCore.Utils;
using Plugins.UnityMonstackContentLoader.JSON.Converter;
using Plugins.UnityMonstackCore.DependencyInjections;
using Plugins.UnityMonstackCore.Loggers;
using Sirenix.Utilities;
using UnityEngine;

#if !UNITY_EDITOR
using Plugins.UnityMonstackCore.Providers;
using Plugins.UnityMonstackCore.Utils;
#endif

namespace Plugins.UnityMonstackContentLoader.JSON
{
    public abstract class AJSONContentListDifferentFilesRepository<TKey, TEntity> : AbstractContentListRepository<TKey, TEntity>, IJSONContentListSingleFileRepository
        where TEntity : class
    {
        public string Path { get; }

        protected JsonSerializerSettings JsonSerializerSettings { get; }

        protected AJSONContentListDifferentFilesRepository(string filePath, bool loadImmediately = false) : base(filePath)
        {
            Path = FileUtils.GetApplicationDirectory() + "/Assets/Resources/" + FilePath;
            JsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };
            OnPrepareJsonSerializer(JsonSerializerSettings);

            if (IsCustomConvertersEnabled)
            {
                var customConverters = DependencyProvider.Resolve<CustomConverterManager>().CustomConverters;
                JsonSerializerSettings.Converters.AddRange(customConverters);
            }

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
#if UNITY_EDITOR
                var filesInPath = Directory.EnumerateFiles(Path, "*.json", SearchOption.AllDirectories)
                    .Select(x => x.Replace(Path + "\\", "").Replace(".json", ""));
#else
                var jsonData = ResourceProvider.GetJSON(FilePath + "/files");
                var filesInPath = LocalStorageUtils.LoadJSONSerializedObjectFromData<ContentFileIndex>(jsonData).files;
#endif
                foreach (var file in filesInPath)
                {
                    var filePath = file.Replace(Path + "\\", "").Replace(".json", "");
                    try
                    {
                        var json = Resources.Load<TextAsset>(FilePath + "/" + filePath).text;
                        var entity = JsonConvert.DeserializeObject<TEntity>(json, JsonSerializerSettings);

                        entries[GetEntityID(entity)] = entity;
                        if (entity is IContentEntity contentEntity)
                        {
                            contentEntity.Initialize();
                        }
                    }
                    catch (Exception e)
                    {
                        UnityLogger.Error($"Failed to read/deserialize JSON from [{filePath}] for repository [{GetType()}] because", e);
                    }
                }
            }
            catch (Exception e)
            {
                UnityLogger.Error($"Failed to initialize repository [{GetType()}] from [{FilePath}] because", e);
            }
        }
    }
}