using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Plugins.UnityMonstackCore.Loggers;
using Plugins.UnityMonstackCore.Utils;

namespace Plugins.UnityMonstackContentLoader
{
    public abstract class AbstractContentListRepository<TKey, TEntity> : IContentListRepository<TKey, TEntity>, IEnumerable<TEntity> 
        where TEntity : class
    {
        protected bool hasPendingChanges;
        protected readonly Dictionary<TKey, TEntity> entries = new Dictionary<TKey, TEntity>();

        protected AbstractContentListRepository(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }

        public virtual FileSourceType FileSource => FileSourceType.ApplicationPersistentData;
        public int Count => entries.Count;
        
        public virtual int Priority => 0;

        public abstract void Reload();

        public object Resolve(object key)
        {
            return GetByKey((TKey)key);
        }
        
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

        public virtual T GetByKey<T>(TKey key) where T : TEntity
        {
            return (T) GetByKey(key);
        }

        public virtual bool TryGetByKey(TKey key, out TEntity resultEntity)
        {
            return entries.TryGetValue(key, out resultEntity);
        }

        public abstract void Save();

        public virtual void Replace(TEntity newEntity)
        {
            entries[GetEntityID(newEntity)] = newEntity;
            hasPendingChanges = true;
        }

        public virtual bool DeleteByKey(TKey key)
        {
            var isRemoved = entries.Remove(key);
            if (isRemoved)
                hasPendingChanges = true;
            return isRemoved;
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