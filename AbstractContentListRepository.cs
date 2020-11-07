using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Plugins.Shared.UnityMonstackCore.Loggers;
using Plugins.Shared.UnityMonstackCore.Utils;

namespace Plugins.Shared.UnityMonstackContentLoader
{
    public abstract class AbstractContentListRepository<TKey, TEntity> : IContentListRepository<TKey, TEntity>,
        IEnumerable<TEntity> where TEntity : class
    {
        protected readonly Dictionary<TKey, TEntity> entries = new Dictionary<TKey, TEntity>();

        protected AbstractContentListRepository(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }

        public virtual FileSourceType FileSource => FileSourceType.ApplicationPersistentData;
        public int Count => entries.Count;

        public virtual int GetLoadingOrder()
        {
            return 0;
        }

        public abstract void Reload();

        public virtual bool ContainsKey(TKey key)
        {
            return entries.ContainsKey(key);
        }

        public virtual TEntity GetByKey(TKey key)
        {
            if (!entries.ContainsKey(key))
            {
                UnityLogger.Warning("Entity with key [{}] is not found in [{}]", key, this);
                return null;
            }

            return entries[key];
        }

        public virtual List<TEntity> GetAll()
        {
            return entries.Values.ToList();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual IEnumerator<TEntity> GetEnumerator()
        {
            return entries.Values.GetEnumerator();
        }

        protected void AddEntry(TEntity entity)
        {
            entries[GetEntityID(entity)] = entity;
        }

        protected abstract TKey GetEntityID(TEntity entity);
    }
}