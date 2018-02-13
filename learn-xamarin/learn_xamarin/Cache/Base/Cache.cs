using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Ninject.Infrastructure.Language;

namespace learn_xamarin.Cache.Base
{
    public class Cache<T> : ICache<T>
    {
        private readonly ObservableCollection<T> _inner;

        public Cache()
        {
            _inner = new ObservableCollection<T>();
            _inner.CollectionChanged += OnInnerCollectionChanged;
        }

        public IEnumerable<T> All()
        {
            return _inner.ToEnumerable();
        }

        public void Add(T item)
        {
            _inner.Add(item);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnInnerCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // do nothing
        }

        private void OnInnerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
            OnInnerCollectionChanged(e);
        }
    }
}