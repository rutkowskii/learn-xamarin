using System;
using System.Collections.Generic;

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
    }
}