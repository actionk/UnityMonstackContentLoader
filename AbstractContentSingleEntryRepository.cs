﻿using Plugins.Shared.UnityMonstackContentLoader;
using Plugins.UnityMonstackCore.Utils;

namespace Plugins.UnityMonstackContentLoader
{
    public abstract class AbstractContentSingleEntryRepository<T> : IContentRepository
    {
        protected AbstractContentSingleEntryRepository(string filePath)
        {
            FilePath = filePath;
        }

        public virtual FileSourceType FileSource => FileSourceType.ApplicationPersistentData;
        public string FilePath { get; }
        public T Entity { get; protected set; }

        public int Count => 1;
        public virtual int Priority => 0;

        public abstract void Reload();
        
        public object GetObjectByKey(object key)
        {
            throw new System.NotImplementedException();
        }
    }
}