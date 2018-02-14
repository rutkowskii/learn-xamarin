using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace learn_xamarin.Cache.Base
{
    public interface IReadOnlyCache<T>
    {
        IEnumerable<T> All();
        IObservable<T> ElementAdded { get; }
    }
}