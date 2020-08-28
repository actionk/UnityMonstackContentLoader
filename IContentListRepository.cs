using System.Collections.Generic;
using Plugins.Framework.Content;

namespace Plugins.UnityContentLoader
{
    public interface IContentListRepository<TKey, TEntity> : IContentRepository
    {
        FileSourceType FileSource { get; }
        bool ContainsKey(TKey key);
        TEntity GetByKey(TKey key);
        List<TEntity> GetAll();
    }
}