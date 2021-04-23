namespace Plugins.Shared.UnityMonstackContentLoader
{
    public interface IContentRepository
    {
        int Count { get; }
        int Priority { get; }
        void Reload();
    }
}