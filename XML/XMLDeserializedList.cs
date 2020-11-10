#region import

using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace Plugins.UnityMonstackContentLoader.XML
{
    [XmlRoot(ElementName = "content")]
    public class XMLDeserializedList<T>
    {
        [XmlElement("entity")] public List<T> Entities { get; set; }
    }
}