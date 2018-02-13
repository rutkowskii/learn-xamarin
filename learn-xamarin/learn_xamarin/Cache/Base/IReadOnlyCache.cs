using System.Collections.Generic;
using System.Collections.Specialized;

namespace learn_xamarin.Cache.Base
{
    public interface IReadOnlyCache<T> : INotifyCollectionChanged
    {
        IEnumerable<T> All();
    }
}