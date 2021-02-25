using System;

namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter.Identifiers
{
    public class IdentifierAttribute : Attribute
    {
        public string Id { get; }

        public IdentifierAttribute(string id)
        {
            Id = id;
        }
    }
}