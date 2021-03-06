﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Plugins.UnityMonstackContentLoader;

namespace Plugins.Shared.UnityMonstackContentLoader.JSON.Converter.Identifiers
{
    public abstract class ACustomJsonIdentifierConverter<T> : CustomCreationConverter<T>
    {
        private readonly Dictionary<string, Type> m_types;

        public virtual string TypeField => "type";

        protected virtual void OnAfterDeserialize(T target)
        {
        }

        protected ACustomJsonIdentifierConverter()
        {
            m_types = IdentifierProvider.GetAllTypesWithIdentifiers<T>();
        }

        public override T Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        private T Create(Type objectType, JObject jObject)
        {
            var type = (string) jObject.Property(TypeField);
            if (type != null && m_types.ContainsKey(type))
            {
                var entry = (T) Activator.CreateInstance(m_types[type]);
                return entry;
            }

            throw new ApplicationException($"The type with identifier [{type}] is not supported!");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var target = Create(objectType, jObject);
            serializer.Populate(jObject.CreateReader(), target);
            
            if (target is IContentEntity contentEntity)
                contentEntity.Initialize();
            
            OnAfterDeserialize(target);
            return target;
        }
    }
}