namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter
{
    public interface IResolver
    {
        
    }
    public class Resolver<TEntity,TRepository> : IResolver
    {
        public Resolver(TEntity data)
        {
            Data = data;
        }

        public TEntity Data { get; }
    }
}