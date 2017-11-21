using System;
using System.Collections.Generic;
using System.Linq;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Utils;
using Newtonsoft.Json;

namespace learn_xamarin.Services
{
    class ExpendituresDataService : IExpendituresDataService
    {
        private readonly ILocalDatabase _localDatabase;
        private readonly RestConnection _restConnection;
        private readonly IConnectionService _connectionService;

        public ExpendituresDataService(ILocalDatabase localDatabase,
            RestConnection restConnection,
            IConnectionService connectionService)
        {
            _localDatabase = localDatabase;
            _restConnection = restConnection;
            _connectionService = connectionService;
        }

        public void Add(Expenditure expenditure)
        {
            //_localDatabase.Insert(expenditure);
            if (_connectionService.IsConnected)
            {
                _restConnection.Post(RestCallsConstants.Expenditure, expenditure);
            }
            else
            {
                MarkForSyncLater(expenditure);
            }
        }

        private void MarkForSyncLater(Expenditure expenditure)
        {
            var syncToken = new UnsynchronizedItem {Id = expenditure.Id};
            _localDatabase.Insert(syncToken);
        }

        public async void TrySynchronize(Action<Expenditure[]> callback)
        {
            var currentlyCashed = _localDatabase.GetAllExpenditures();

            if (!_connectionService.IsConnected)
            {
                callback(currentlyCashed);
                return;
            }

            var task = _restConnection.Get(RestCallsConstants.Expenditure, ResolveParameters(currentlyCashed));
            await task;
            var serverExpenditures = JsonConvert.DeserializeObject<Expenditure[]>(task.Result.Content);

            var expendituresMerged = SaveServerExpendituresLocally(currentlyCashed, serverExpenditures);

            PushUnsavedExpenditures();

            callback(expendituresMerged.ToArray());
        }

        private List<Expenditure> SaveServerExpendituresLocally(Expenditure[] currentlyCashed,
            Expenditure[] serverExpenditures)
        {
            var newExpenditures = FilterServerExpenditures(currentlyCashed, serverExpenditures);

            var expendituresMerged = new List<Expenditure>(currentlyCashed);

            foreach (var newExpenditure in newExpenditures)
            {
                //_localDatabase.Insert(newExpenditure); todo piotr review it completely 
                expendituresMerged.Add(newExpenditure);
            }
            return expendituresMerged;
        }

        private void PushUnsavedExpenditures()
        {
            var unsynchronized = _localDatabase.GetAllUnsynchronizedItems();
            foreach (var unsynchronizedItem in unsynchronized)
            {
                var expenditure = _localDatabase.GetAllExpenditures().Single(e => e.Id == unsynchronizedItem.Id); //todo perf
                _restConnection.Post(RestCallsConstants.Expenditure, expenditure);
            }
            _localDatabase.ClearUnsynchronizedItems();
        }

        private IEnumerable<Expenditure> FilterServerExpenditures(Expenditure[] currentlyCashed,
            Expenditure[] serverExpenditures)
        {
            var ids = new HashSet<Guid>(currentlyCashed.Select(e => e.Id));
            foreach (var serverExpenditure in serverExpenditures)
            {
                if (!ids.Contains(serverExpenditure.Id))
                {
                    yield return serverExpenditure;
                }
            }
        }

        private RequestParameter[] ResolveParameters(Expenditure[] currentlyCashed)
        {
            if (!currentlyCashed.Any()) return new RequestParameter[0];
            var timestamp = currentlyCashed.Max(e => e.Timestamp).AddDays(-1);

            return new[]
            {
                new RequestParameter
                {
                    Key = RestCallsConstants.IgnoreBelow,
                    Value = timestamp.ToString(RestCallsConstants.DateFormat)
                }
            };
        }
    }
}