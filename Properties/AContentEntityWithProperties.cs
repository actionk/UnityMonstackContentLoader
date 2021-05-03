using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.Shared.UnityMonstackCore.Utils;
using Plugins.UnityMonstackContentLoader;
using Plugins.UnityMonstackCore.Loggers;
using UnityEngine;

namespace Plugins.Shared.UnityMonstackContentLoader.Properties
{
    public abstract class AContentEntityWithProperties : IContentEntity
    {
        public virtual void Initialize()
        {
            InitializeProperties();
        }

        public readonly List<ContentProperty> properties = new List<ContentProperty>();

        private MultiValueDictionary<Type, ContentProperty> m_propertiesByType;

        public T GetProperty<T>() where T : ContentProperty
        {
            try
            {
                return (T) m_propertiesByType[typeof(T)].FirstOrDefault();
            }
            catch (KeyNotFoundException)
            {
                UnityLogger.Error($"Content entity {this} has no property of type {typeof(T)}. Available properties: {string.Join(",\n", m_propertiesByType.Keys)}");
                return null;
            }
        }

        public List<T> GetPropertyList<T>() where T : ContentProperty
        {
            try
            {
                return m_propertiesByType[typeof(T)]
                    .OfType<T>()
                    .ToList();
            }
            catch (KeyNotFoundException)
            {
                UnityLogger.Error($"Content entity {this} has no property of type {typeof(T)}. Available properties: {string.Join(",\n", m_propertiesByType.Keys)}");
                return null;
            }
        }

        public bool TryGetProperty<T>(out T value) where T : ContentProperty
        {
            var key = typeof(T);
            if (m_propertiesByType.ContainsKey(key))
            {
                value = (T) m_propertiesByType[key].FirstOrDefault();
                return true;
            }

            value = default;
            return false;
        }

        public bool HasProperty<T>() where T : ContentProperty
        {
            return m_propertiesByType.ContainsKey(typeof(T));
        }

        private void InitializeProperties()
        {
            if (Application.isPlaying)
            {
                m_propertiesByType = new MultiValueDictionary<Type, ContentProperty>();
                foreach (var property in properties)
                    m_propertiesByType.Add(property.GetType(), property);
            }
        }
    }
}