namespace Plugins.UnityContentLoader
{
    public interface IContentRepository
    {
        int Count { get; }
        int GetLoadingOrder();
        void Reload();
    }
}