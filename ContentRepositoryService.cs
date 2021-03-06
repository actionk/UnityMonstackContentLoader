﻿#region import

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Plugins.Shared.UnityMonstackContentLoader;
using Plugins.UnityMonstackCore.DependencyInjections;
using Plugins.UnityMonstackCore.Extensions.Collections;
using Plugins.UnityMonstackCore.Loggers;
using Sirenix.Utilities;

#endregion

namespace Plugins.UnityMonstackContentLoader
{
    [Inject]
    public class ContentRepositoryService
    {
        private readonly Dictionary<Type, IContentRepository> m_contentRepositories =
            new Dictionary<Type, IContentRepository>();

        public static ContentRepositoryService Instance => DependencyProvider.Resolve<ContentRepositoryService>();

        public static T GetRepository<T>() where T : class, IContentRepository
        {
            return Instance.Get<T>();
        }

        public virtual T Get<T>() where T : class, IContentRepository
        {
            var type = typeof(T);
            if (!m_contentRepositories.ContainsKey(type))
                throw new InvalidOperationException("Type [" + type + "] doesn't have [Inject] attribute");

            return m_contentRepositories[type] as T;
        }

        public void Reload()
        {
            m_contentRepositories.Clear();
            var listOfRepositories = DependencyProvider.ResolveList<IContentRepository>();
            foreach (var loader in listOfRepositories)
                m_contentRepositories[loader.GetType()] = loader;

            var loaders = new List<IContentRepository>();
            m_contentRepositories.ForEachValue(loader => loaders.Add(loader));

            loaders
                .OrderByDescending(x => x.Priority)
                .ForEach(ReloadRepository);
        }

        private static void ReloadRepository(IContentRepository loader)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            loader.Reload();
            stopwatch.Stop();
            UnityLogger.Info("Repository [" + loader + "] is reloaded with [" + loader.Count + $"] entries in [{stopwatch.ElapsedMilliseconds} ms]");
        }
    }
}