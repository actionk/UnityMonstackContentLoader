using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Plugins.Shared.UnityMonstackCore.Utils;
using Plugins.UnityMonstackCore.DependencyInjections;

namespace Plugins.UnityMonstackContentLoader.JSON.Converter
{
    [Inject]
    public class CustomConverterManager
    {
        private List<JsonConverter> m_converters;

        public List<JsonConverter> CustomConverters
        {
            get
            {
                if (m_converters == null)
                    m_converters = LoadConverters();

                return m_converters;
            }
        }

        private List<JsonConverter> LoadConverters()
        {
            return ReflectionUtils.GetAllDerivedTypes<JsonConverter>()
                .Where(x => typeof(ICustomJsonConverter).IsAssignableFrom(x))
                .Select(x => (JsonConverter) DependencyProvider.ResolveByType(x))
                .ToList();
        }
    }
}