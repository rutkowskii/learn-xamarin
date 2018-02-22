using System;
using System.Collections.Generic;
using System.Linq;
using learn_xamarin.Cache;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Utils;
using Newtonsoft.Json;

namespace learn_xamarin.DataServices
{
    class ExpendituresDataService : IExpendituresDataService
    {
        private readonly ILocalDatabase _localDatabase;
        private readonly IRestConnection _restConnection;
        private readonly IExpendituresCache _cache;

        public ExpendituresDataService(
            ILocalDatabase localDatabase, IRestConnection restConnection, IExpendituresCache expendituresCache)
        {
            _localDatabase = localDatabase;
            _restConnection = restConnection;
            _cache = expendituresCache;
        }

        public void Add(Expenditure expenditure)
        {
            AddExpenditureLocally(expenditure);
            AsyncOp.Get(
                asyncOp: () => _restConnection.Post(RestCallsConstants.Expenditure, expenditure.AsArray()),
                onSuccess: x => { },
                onFailure: x => _localDatabase.Insert(new UnsynchronizedItem {Id = expenditure.Id}),
                onCancel: () => _localDatabase.Insert(new UnsynchronizedItem {Id = expenditure.Id})
            ).Run();
        }

        public void TrySynchronize()
        {
            var localItems = _localDatabase.GetAllExpenditures();
            LoadLocalItemsToCache(localItems);

            var itemsToUpload = ResolveUnsynchronizedLocalItems(localItems);

            LoadExpendituresFromServer();
            
            UploadToServer(itemsToUpload);
        }

        private void UploadToServer(Expenditure[] itemsToUpload)
        {
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

        private void LoadExpendituresFromServer()
        {
            AsyncOp.Get(
                asyncOp: () => _restConnection.Get(RestCallsConstants.Expenditure),
                onSuccess: x => JsonConvert.DeserializeObject<Expenditure[]>(x.Content).Foreach(AddExpenditureLocally),
                onFailure: x => { },
                onCancel: () => { }
            ).Run();
        }

        private Expenditure[] ResolveUnsynchronizedLocalItems(Expenditure[] localItems)
        {
            var unsynchronizedIds =
                new HashSet<Guid>(_localDatabase.GetAllUnsynchronizedItems().Select(i => i.Id));
            var itemsToUpload = localItems.Where(exp => unsynchronizedIds.Contains(exp.Id)).ToArray();
            return itemsToUpload;
        }

        private void LoadLocalItemsToCache(Expenditure[] localItems)
        {
            localItems.Foreach(x =>
            {
                if (!_cache.IsStored(x.Id))
                {
                    _cache.Add(x);
                }
            });
        }

        private void AddExpenditureLocally(Expenditure expenditure)
        {
            if (_cache.IsStored(expenditure.Id)) return;
            _cache.Add(expenditure);
            _localDatabase.Insert(expenditure);
        }
    }
}