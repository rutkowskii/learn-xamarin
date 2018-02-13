using System;
using System.Collections.Generic;
using System.Linq;
using learn_xamarin.Cache;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Utils;

namespace learn_xamarin.DataServices
{
    class ExpendituresDataService : IExpendituresDataService
    {
        private readonly ILocalDatabase _localDatabase;
        private readonly IRestConnection _restConnection;
        private readonly IExpendituresCache _cache; 

        public ExpendituresDataService(ILocalDatabase localDatabase, IRestConnection restConnection, IExpendituresCache expendituresCache)
        {
            _localDatabase = localDatabase;
            _restConnection = restConnection;
            _cache = expendituresCache;
            _localDatabase.GetAllExpenditures().Foreach(_cache.Add);
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
}
