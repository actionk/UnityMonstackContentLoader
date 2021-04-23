using System.Collections.Generic;
using Plugins.Shared.UnityMonstackContentLoader;
using Plugins.UnityMonstackCore.Utils;

namespace Plugins.UnityMonstackContentLoader
{
    public interface IContentListRepository<TKey, TEntity> : IContentRepository
    {
        FileSourceType FileSource { get; }
        bool ContainsKey(TKey key);
        TEntity GetByKey(TKey key);
        List<TEntity> GetAll();
    }
}