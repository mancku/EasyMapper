namespace EasyMapper
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public static class FastMapper
    {
        private static readonly ConcurrentDictionary<(Type SourceType, Type TargetType), Delegate> _mapFuncs = new();

        public static TTarget Map<TSource, TTarget>(TSource source)
        {
            if (source == null)
                return default;

            var key = (SourceType: typeof(TSource), TargetType: typeof(TTarget));
            var mapFunc = _mapFuncs.GetOrAdd(key, _ => BuildMapFunction<TSource, TTarget>().Compile());
            return ((Func<TSource, TTarget>)mapFunc)(source);
        }

        /// <summary>
        /// This function builds an expression tree that represents a function to map a source object to a target object.
        /// This function is then compiled into a delegate and executed to perform the actual mapping operation.
        /// The expression tree represents the code of the function in a data structure that can be manipulated at runtime,
        /// which allows for dynamic generation of the mapping function based on the source and target types.
        /// </summary>
        private static Expression<Func<TSource, TTarget>> BuildMapFunction<TSource, TTarget>()
        {
            // Create a parameter expression for the source object, 
            // which will be the input parameter for the mapping function.
            var sourceParameter = Expression.Parameter(typeof(TSource), "source");

            // Initialize a list to hold the member binding expressions, 
            // which represent the operations of copying the properties from the source object to the target object.
            var memberBindings = new List<MemberBinding>();

            // Iterate over all properties of the target type.
            foreach (var targetProperty in typeof(TTarget).GetProperties())
            {
                // Try to find a property in the source type that has the same name and type as the current target property.
                var sourceProperty = typeof(TSource).GetProperty(targetProperty.Name);
                if (sourceProperty != null && sourceProperty.PropertyType == targetProperty.PropertyType)
                {
                    // Create a property expression for the source property, 
                    // which represents accessing the property of the source object.
                    var sourcePropertyExpression = Expression.Property(sourceParameter, sourceProperty);

                    // Create a member binding expression for the target property, 
                    // which represents assigning the value of the source property to the target property.
                    var memberBinding = Expression.Bind(targetProperty, sourcePropertyExpression);

                    // Add the member binding expression to the list.
                    memberBindings.Add(memberBinding);
                }
            }

            // Create a new expression for the target object, 
            // which represents creating a new instance of the target type.
            var targetNewExpression = Expression.New(typeof(TTarget));

            // Create a member initialization expression, 
            // which represents initializing the new target object with the member bindings.
            var memberInitExpression = Expression.MemberInit(targetNewExpression, memberBindings);

            // Create and return a lambda expression that represents the mapping function, 
            // taking the source object as input and returning the initialized target object.
            return Expression.Lambda<Func<TSource, TTarget>>(memberInitExpression, sourceParameter);
        }
    }
}
