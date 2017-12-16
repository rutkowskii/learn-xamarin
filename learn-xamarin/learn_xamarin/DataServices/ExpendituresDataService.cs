using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Utils;
using Ninject.Infrastructure.Language;

namespace learn_xamarin.DataServices
{
    class ExpendituresDataService : IExpendituresDataService
    {
        private readonly ILocalDatabase _localDatabase;
        private readonly IRestConnection _restConnection;

        private readonly ExpendituresCache _cache; 

        public ExpendituresDataService(ILocalDatabase localDatabase, IRestConnection restConnection, IDateTimeProvider dateTimeProvider)
        {
            _localDatabase = localDatabase;
            _restConnection = restConnection;
            _cache = new ExpendituresCache(dateTimeProvider);
            _localDatabase.GetAllExpenditures().Foreach(_cache.Add);
        }

        public IExpendituresCache GetCache()
        {
            return _cache;
        }

        public void Add(Expenditure expenditure)
        {
            _localDatabase.Insert(expenditure);
            _cache.Add(expenditure);
            AsyncOp.Get(
                asyncOp: () => _restConnection.Post(RestCallsConstants.Expenditure, expenditure.AsArray()),
                onSuccess: x => { },
                onFailure: x => _localDatabase.Insert(new UnsynchronizedItem{ Id = expenditure.Id}),
                onCancel: () => _localDatabase.Insert(new UnsynchronizedItem{ Id = expenditure.Id})
            ).Run();
        }
        
        public void TrySynchronize(Action<Expenditure[]> callback) // just do this when the welcome screen appears?
        {
            var currentlyCashed = _localDatabase.GetAllExpenditures();
            var unsynchronizedIds = new HashSet<Guid>(_localDatabase.GetAllUnsynchronizedItems().Select(i => i.Id));
            
            UploadExpendituresToServer(currentlyCashed, unsynchronizedIds); //  todo piotr this is naive, 1-way so far. 
            callback(currentlyCashed);
        }

        private void UploadExpendituresToServer(Expenditure[] currentlyCashed, HashSet<Guid> unsynchronizedIds)
        {
            var itemsToUpload = currentlyCashed.Where(exp => unsynchronizedIds.Contains(exp.Id)).ToArray();
            if (itemsToUpload.Any())
            {
                AsyncOp.Get(
                    asyncOp: () => _restConnection.Post(RestCallsConstants.Expenditure, itemsToUpload),
                    onSuccess: x => { _localDatabase.ClearUnsynchronizedItems(); },
                    onFailure: x => { },
                    onCancel: () => { }
                ).Run();
            }
        }
    }

    public interface IReadOnlyCache<T> : INotifyCollectionChanged
    {
        IEnumerable<T> All();
    }

    public interface ICache<T> : IReadOnlyCache<T>
    {
        void Add(T items);
    }

    public interface IExpendituresCache : IReadOnlyCache<Expenditure>
    {
        decimal Sum { get; }
        decimal SumThisWeek { get; }
        decimal SumThisMonth { get; }
    }
    
    public class Cache<T> : ICache<T>, IReadOnlyCache<T>
    {
        private readonly ObservableCollection<T> _inner;

        public Cache()
        {
            _inner = new ObservableCollection<T>();
            _inner.CollectionChanged += OnInnerCollectionChanged;
        }

        private void OnInnerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnInnerCollectionChanged(e);
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
        }
    }

    public class ExpendituresCache : Cache<Expenditure>, IExpendituresCache
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public ExpendituresCache(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            Sum = 0;
        }
        
        public decimal Sum { get; private set; }
        public decimal SumThisWeek { get; private set; } // todo piotr what about the currencies here?
        public decimal SumThisMonth { get; private set; } // todo piotr what about the currencies here?

        protected override void OnInnerCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnInnerCollectionChanged(e);
            e.OldItems?.Cast<Expenditure>().Foreach(SubtractForDeletedItems);
            e.NewItems?.Cast<Expenditure>().Foreach(AddForNewItems);
        }

        private void AddForNewItems(Expenditure exp)
        {
            Sum += exp.Sum;
            if (exp.Timestamp > WeekStart(_dateTimeProvider.Now))
            {
                SumThisWeek += exp.Sum;
            }
            if (exp.Timestamp > MonthStart(_dateTimeProvider.Now))
            {
                SumThisMonth += exp.Sum;
            }
        }

        private void SubtractForDeletedItems(Expenditure exp)
        {
            Sum -= exp.Sum;
            if (exp.Timestamp > WeekStart(_dateTimeProvider.Now))
            {
                SumThisWeek -= exp.Sum;
            }
            if (exp.Timestamp > MonthStart(_dateTimeProvider.Now))
            {
                SumThisMonth -= exp.Sum;
            }
        }
        
        private DateTime MonthStart(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        private DateTime WeekStart(DateTime dt)
        {
            for (var i = dt;; i = i.AddDays(-1))
            {
                if (i.DayOfWeek == DayOfWeek.Monday) return i.Date;
            }
        }
    }
}
