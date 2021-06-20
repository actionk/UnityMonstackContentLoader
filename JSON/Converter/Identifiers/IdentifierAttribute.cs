using System;

namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter.Identifiers
{
    public class IdentifierAttribute : Attribute
    {
        public string Id { get; }
        public string Label { get; set; }
        public string Tags { get; set; }

        public IdentifierAttribute(string id)
        {
            Id = id;
        }
    }
}