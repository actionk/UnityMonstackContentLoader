namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter.Resolvers
{
    public interface IResolver
    {
        
    }
    public class ContentResolver<TEntity,TRepository> : IResolver
    {
        public ContentResolver(TEntity data)
        {
            Data = data;
        }

        public TEntity Data { get; }
    }
}