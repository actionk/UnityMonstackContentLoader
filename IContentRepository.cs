namespace Plugins.Shared.UnityMonstackContentLoader
{
    public interface IContentRepository
    {
        int Count { get; }
        int GetLoadingOrder();
        void Reload();
    }
}