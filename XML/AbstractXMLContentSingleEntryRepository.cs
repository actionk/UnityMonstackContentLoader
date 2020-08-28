using System.IO;
using System.Xml.Serialization;

namespace Plugins.UnityMonstackContentLoader.XML
{
    public class AbstractXMLContentSingleEntryRepository<T> : AbstractContentSignleEntryRepository<T>
    {
        protected AbstractXMLContentSingleEntryRepository(string filePath) : base(filePath)
        {
        }

        public override void Reload()
        {
            var serializer = new XmlSerializer(typeof(T));
            TextReader textReader = new StreamReader(FilePath);
            Entity = (T) serializer.Deserialize(textReader);
            textReader.Close();
        }
    }
}