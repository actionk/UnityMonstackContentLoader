using System.Collections.Generic;
using Plugins.Shared.UnityMonstackContentLoader;
using Plugins.Shared.UnityMonstackContentLoader.JSON.Converter;
using Plugins.UnityMonstackCore.Utils;

namespace Plugins.UnityMonstackContentLoader
{
    public interface IContentListRepository<TKey, TEntity> : IContentRepository, IObjectResolver
    {
        FileSourceType FileSource { get; }
        bool ContainsKey(TKey key);
        TEntity GetByKey(TKey key);
        List<TEntity> GetAll();
    }
}