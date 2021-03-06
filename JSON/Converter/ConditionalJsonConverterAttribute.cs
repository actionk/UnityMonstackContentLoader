using System;

namespace Plugins.UnityMonstackContentLoader.JSON.Converter
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConditionalJsonConverterAttribute : Attribute
    {
        public Type RootType { get; set; }
        public string PropertyName { get; set; }
        public Type[] ConvertableTypes { get; set; }
        public string[] PropertyValues { get; set; }
    }
}