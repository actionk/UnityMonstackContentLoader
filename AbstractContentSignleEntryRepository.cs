using Plugins.Shared.UnityMonstackCore.Utils;

namespace Plugins.Shared.UnityMonstackContentLoader
{
    public abstract class AbstractContentSignleEntryRepository<T> : IContentRepository
    {
        protected AbstractContentSignleEntryRepository(string filePath)
        {
            FilePath = filePath;
        }

        public virtual FileSourceType FileSource => FileSourceType.ApplicationPersistentData;
        public string FilePath { get; }
        public T Entity { get; protected set; }

        public int Count => 1;

        public int GetLoadingOrder()
        {
            return 0;
        }

        public abstract void Reload();
    }
}