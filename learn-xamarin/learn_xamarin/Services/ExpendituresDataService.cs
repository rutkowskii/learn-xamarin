using System;
using System.Collections.Generic;
using System.Linq;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Utils;

namespace learn_xamarin.Services
{
    class ExpendituresDataService : IExpendituresDataService
    {
        private readonly ILocalDatabase _localDatabase;
        private readonly RestConnection _restConnection;

        public ExpendituresDataService(ILocalDatabase localDatabase,
            RestConnection restConnection)
        {
            _localDatabase = localDatabase;
            _restConnection = restConnection;
        }

        public void Add(Expenditure expenditure)
        {
            _localDatabase.Insert(expenditure);
            AsyncOp.Get(
                asyncOp: () => _restConnection.Post(RestCallsConstants.Expenditure, expenditure.AsArray()),
                onSuccess: x => { },
                onFailure: x => _localDatabase.Insert(new UnsynchronizedItem{ Id = expenditure.Id}),
                onCancel: () => _localDatabase.Insert(new UnsynchronizedItem{ Id = expenditure.Id})
            ).Run();
        }
        
        // start with a simple model -> save locally, backend is just a backup dir. 
        // todo piotr [!] general question -> how to sync 2 ways --> leave it for later. 
        
        public void TrySynchronize(Action<Expenditure[]> callback)
        {
            var currentlyCashed = _localDatabase.GetAllExpenditures();
            var unsynchronizedIds = new HashSet<Guid>(_localDatabase.GetAllUnsynchronizedItems().Select(i => i.Id));
            
            UploadExpendituresToServer(currentlyCashed, unsynchronizedIds);
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