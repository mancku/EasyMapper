using System;
using System.Collections.Generic;

namespace EasyMapper
{
    public class PropertyMapper<TSource, TTarget> where TTarget : class
    {
        private readonly TSource _source;
        private readonly TTarget _target;
        private Action<TTarget, TSource>? _customAfterMap;
        private readonly List<string> _ignoreProperties;

        public PropertyMapper(TSource source, TTarget target)
        {
            _source = source;
            _target = target;
            _ignoreProperties = new List<string>();
        }

        public static PropertyMapper<TSource, TTarget> Create(TSource source, TTarget target)
        {
            return new PropertyMapper<TSource, TTarget>(source, target);
        }

        public PropertyMapper<TSource, TTarget> WithCustomAfterMap(Action<TTarget, TSource> customAfterMap)
        {
            _customAfterMap = customAfterMap;
            return this;
        }

        public PropertyMapper<TSource, TTarget> IgnoreProperties(IEnumerable<string> ignoreProperties)
        {
            _ignoreProperties.AddRange(ignoreProperties);
            return this;
        }

        public PropertyMapper<TSource, TTarget> IgnoreProperties(params string[] ignoreProperties)
        {
            _ignoreProperties.AddRange(ignoreProperties);
            return this;
        }

        public PropertyMapper<TSource, TTarget> IgnoreProperty(string propertyName)
        {
            _ignoreProperties.Add(propertyName);
            return this;
        }

        public void Map()
        {
            _target.HydrateFrom(_source, _customAfterMap, _ignoreProperties.ToArray());
        }
    }
}
