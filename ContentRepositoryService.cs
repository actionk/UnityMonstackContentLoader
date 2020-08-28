#region import

using System;
using System.Collections.Generic;
using Plugins.UnityMonstackCore.DependencyInjections;
using Plugins.UnityMonstackCore.Extensions.Collections;
using UnityEngine;

#endregion

namespace Plugins.UnityContentLoader
{
    [Inject]
    public class ContentRepositoryService
    {
        private readonly Dictionary<Type, IContentRepository> m_contentRepositories =
            new Dictionary<Type, IContentRepository>();

        public ContentRepositoryService()
        {
            var listOfRepositories = DependencyProvider.ResolveList<IContentRepository>();
            foreach (var loader in listOfRepositories)
                m_contentRepositories[loader.GetType()] = loader;
        }

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
            var loaders = new List<IContentRepository>();
            m_contentRepositories.ForEachValue(loader => loaders.Add(loader));
            loaders.Sort((x, y) => x.GetLoadingOrder().CompareTo(y.GetLoadingOrder()));
            loaders.ForEach(loader => ReloadRepository(loader));
        }

        private static void ReloadRepository(IContentRepository loader)
        {
            loader.Reload();
            Debug.Log("Repository [" + loader + "] is reloaded with [" + loader.Count + "] entries");
        }
    }
}