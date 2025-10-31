namespace EasyMapper
{
    using System;
    using System.Collections.Generic;

    public class EasyMapper<TSource, TTarget> where TTarget : class
    {
        private readonly TSource _source;
        private readonly TTarget _target;
        private Action<TTarget, TSource>? _customAfterMap;
        private readonly List<string> _ignoreProperties;

        public EasyMapper(TSource source, TTarget target)
        {
            _source = source;
            _target = target;
            _ignoreProperties = new List<string>();
        }

        public static EasyMapper<TSource, TTarget> Create(TSource source, TTarget target)
        {
            return new EasyMapper<TSource, TTarget>(source, target);
        }

        public EasyMapper<TSource, TTarget> WithCustomAfterMap(Action<TTarget, TSource> customAfterMap)
        {
            _customAfterMap = customAfterMap;
            return this;
        }

        public EasyMapper<TSource, TTarget> IgnoreProperties(IEnumerable<string> ignoreProperties)
        {
            _ignoreProperties.AddRange(ignoreProperties);
            return this;
        }

        public EasyMapper<TSource, TTarget> IgnoreProperties(params string[] ignoreProperties)
        {
            _ignoreProperties.AddRange(ignoreProperties);
            return this;
        }

        public EasyMapper<TSource, TTarget> IgnoreProperty(string propertyName)
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
