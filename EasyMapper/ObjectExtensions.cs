using System;
using System.Linq;

namespace EasyMapper
{
    public static class ObjectExtensions
    {
        public static void HydrateFrom<TTarget, TSource>(this TTarget? targetObject, TSource? sourceObject,
            Action<TTarget, TSource>? customAfterMap = null,
            params string[] namesOfPropertiesToIgnore) where TTarget : class
        {
            if (targetObject == null || sourceObject == null)
            {
                return;
            }

            var targetProperties = typeof(TTarget).GetProperties()
                .Where(x => x.CanWrite && !namesOfPropertiesToIgnore.Contains(x.Name));
            foreach (var targetProperty in targetProperties)
            {
                var sourceProperty = sourceObject.GetType().GetProperty(targetProperty.Name);
                if (sourceProperty != null)
                {
                    var sourceValue = sourceProperty.GetValue(sourceObject);
                    if (targetProperty.PropertyType != sourceProperty.PropertyType)
                    {
                        try
                        {
                            sourceValue = Convert.ChangeType(sourceValue, targetProperty.PropertyType);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error converting property {sourceProperty.Name}: {ex.Message}");
                        }
                    }

                    targetProperty.SetValue(targetObject, sourceValue);
                }
            }

            customAfterMap?.Invoke(targetObject, sourceObject);
        }
    }
}
