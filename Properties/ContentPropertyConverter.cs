using Plugins.Shared.UnityMonstackContentLoader.JSON.Converter.Identifiers;
using Plugins.UnityMonstackCore.DependencyInjections;

namespace Plugins.Shared.UnityMonstackContentLoader.Properties
{
    [Inject]
    public class ContentPropertyConverter : ACustomJsonIdentifierConverter<ContentProperty>
    {
        public override string TypeField => "type";

        protected override void OnAfterDeserialize(ContentProperty target)
        {
            target.Initialize();
        }
    }
}