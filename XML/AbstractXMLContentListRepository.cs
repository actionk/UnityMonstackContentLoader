#region import

using System.IO;
using System.Xml.Serialization;

#endregion

namespace Plugins.Framework.Content.XML
{
    public abstract class AbstractXMLContentListRepository<TKey, TEntity> : AbstractContentListRepository<TKey, TEntity>
        where TEntity : class
    {
        protected AbstractXMLContentListRepository(string filePath) : base(filePath)
        {
        }

        public override void Reload()
        {
            var serializer = new XmlSerializer(typeof(XMLDeserializedList<TEntity>));
            TextReader textReader = new StreamReader(FilePath);
            var deserializedList = serializer.Deserialize(textReader) as XMLDeserializedList<TEntity>;

            deserializedList.Entities.ForEach(entity => { entries[GetEntityID(entity)] = entity; });
            textReader.Close();
        }
    }
}