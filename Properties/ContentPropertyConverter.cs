using Plugins.Shared.UnityMonstackContentLoader.JSON.Converter.Identifiers;
using Plugins.UnityMonstackContentLoader.JSON.Converter;
using Plugins.UnityMonstackCore.DependencyInjections;

namespace Plugins.Shared.UnityMonstackContentLoader.Properties
{
    [Inject]
    public class ContentPropertyConverter : ACustomJsonIdentifierConverter<ContentProperty>, ICustomJsonConverter
    {
    }
}