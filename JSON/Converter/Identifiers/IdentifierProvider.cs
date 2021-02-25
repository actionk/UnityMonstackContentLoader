using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.UnityMonstackCore.Loggers;
using Sirenix.Utilities;

namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter.Identifiers
{
    public static class IdentifierProvider
    {
        public struct IdentifierEntry
        {
            public Type type;
            public IdentifierAttribute attribute;
        }

        public static Dictionary<string, Type> GetAllTypesWithIdentifiers<T>()
        {
            var typeToSearchFor = typeof(T);
            return GetAllTypesWithIdentifiers(typeToSearchFor);
        }

        public static Dictionary<string, Type> GetAllTypesWithIdentifiers(Type typeToSearchFor)
        {
            var list = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p != typeToSearchFor && typeToSearchFor.IsAssignableFrom(p) && !p.IsAbstract)
                .Select(x => new IdentifierEntry
                {
                    type = x,
                    attribute = x.GetCustomAttribute<IdentifierAttribute>()
                });

            var map = new Dictionary<string, Type>();
            foreach (var entry in list)
            {
                if (entry.attribute == null)
                {
                    UnityLogger.Error($"Type {entry.type} doesn't have {typeof(IdentifierAttribute)}, but it supposed to. Maybe you forgot to make it abstract or add the attribute?");
                    continue;
                }

                if (map.ContainsKey(entry.attribute.Id))
                {
                    UnityLogger.Error($"Type {entry.type} has the same identifier as {map[entry.attribute.Id]}. Please use another name");
                    continue;
                }

                map[entry.attribute.Id] = entry.type;
            }

            return map;
        }
    }
}