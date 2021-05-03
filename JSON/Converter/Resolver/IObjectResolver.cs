namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter
{
    public interface IObjectResolver
    {
        object Resolve(object key);
    }
}