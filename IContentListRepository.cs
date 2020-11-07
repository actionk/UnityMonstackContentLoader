using System.Collections.Generic;
using Plugins.Shared.UnityMonstackCore.Utils;

namespace Plugins.Shared.UnityMonstackContentLoader
{
    public interface IContentListRepository<TKey, TEntity> : IContentRepository
    {
        FileSourceType FileSource { get; }
        bool ContainsKey(TKey key);
        TEntity GetByKey(TKey key);
        List<TEntity> GetAll();
    }
}