using System;
using System.Collections.Generic;
using System.Linq;

namespace learn_xamarin.Model
{
    public static class EnumerableExtensions
    {
        public static void Foreach<T>(this IEnumerable<T> collection, Action<T> a)
        {
            foreach(var item in collection)
            {
                a(item);
            }
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> collection, Func<T, bool> predicate, T defaultValue) where T : class
        {
            var result = collection.FirstOrDefault(predicate);
            return result ?? defaultValue;
        }
        
        public static T[] AsArray<T>(this T item)
        {
            return new[] {item};
        }
    }
}