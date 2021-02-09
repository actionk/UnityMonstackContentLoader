using Plugins.UnityMonstackCore.DependencyInjections;
using UnityEngine;

namespace Plugins.UnityMonstackContentLoader
{
    public static class ContentRepositoryUtils
    {
        public static T GetEntityByKey<T, TRepository, TKey>(TKey key)
            where TRepository : AbstractContentListRepository<TKey, T>, new()
            where T : class
        {
            var isApplicationPlaying = Application.isPlaying;
            var repository = isApplicationPlaying ? DependencyProvider.Resolve<TRepository>() : new TRepository();
            if (!isApplicationPlaying)
                repository.Reload();

            return repository.GetByKey(key);
        }
    }
}