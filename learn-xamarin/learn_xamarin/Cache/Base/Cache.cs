using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Ninject.Infrastructure.Language;

namespace learn_xamarin.Cache.Base
{
    public class Cache<T> : ICache<T>
    {
        private readonly List<T> _inner;
        private readonly ReplaySubject<T> _subject;

        public Cache()
        {
            _inner = new List<T>();
            _subject = new ReplaySubject<T>();
        }
    
        public IEnumerable<T> All()
        {
            return _inner.ToEnumerable();
        }

        public IObservable<T> ElementAdded => _subject.AsObservable();

        public void Add(T item)
        {
            _inner.Add(item);
            OnItemAdded(item);
            _subject.OnNext(item);
        }

        protected virtual void OnItemAdded(T item)
        {
            // do nothing
        }
    }
}