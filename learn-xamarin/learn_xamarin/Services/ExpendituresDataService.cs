using System;
using System.Collections.Generic;
using System.Linq;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Utils;
using Newtonsoft.Json;

namespace learn_xamarin.Services
{
    public class UnsynchronizedItem
    {
        public Guid Id { get; set; }
    }

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

        // todo store things in local db temporarily so we sync when we have the internet. 
        public void Add(Expenditure expenditure) 
        {
            _localDatabase.Insert(expenditure);
            if (_connectionService.IsConnected)
            {
                _restConnection.Post("expenditure", expenditure);
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
            if (!_connectionService.IsConnected)
            {
                return;
            }

            var currentlyCashed = _localDatabase.GetAllExpenditures();

            var task = _restConnection.Get("expenditure", ResolveParameters(currentlyCashed));
            await task;
            var serverExpenditures = JsonConvert.DeserializeObject<Expenditure[]>(task.Result.Content);

            var expendituresMerged = SaveServerExpendituresLocally(currentlyCashed, serverExpenditures);

            PushUnsavedExpenditures();

            callback(expendituresMerged.ToArray());
        }

        private List<Expenditure> SaveServerExpendituresLocally(Expenditure[] currentlyCashed, Expenditure[] serverExpenditures)
        {
            var newExpenditures = FilterServerExpenditures(currentlyCashed, serverExpenditures);

            var expendituresMerged = new List<Expenditure>(currentlyCashed);

            foreach (var newExpenditure in newExpenditures)
            {
                _localDatabase.Insert(newExpenditure);
                expendituresMerged.Add(newExpenditure);
            }
            return expendituresMerged;
        }

        private void PushUnsavedExpenditures()
        {
            var unsynchronized = _localDatabase.GetAllUnsynchronizedItems();
            foreach (var unsynchronizedItem in unsynchronized)
            {
                _restConnection.Post("expenditure", unsynchronizedItem);
            }
            _localDatabase.ClearUnsynchronizedItems();
        }

        private IEnumerable<Expenditure> FilterServerExpenditures(Expenditure[] currentlyCashed, Expenditure[] serverExpenditures)
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
            if(!currentlyCashed.Any()) return new RequestParameter[0];
            var timestamp = currentlyCashed.Max(e => e.Timestamp).AddDays(-1);

            return new []
            {
                new RequestParameter {Key = "ignoreBelow", Value = timestamp.ToString("yyyy-MM-dd")}
            };
        }
    }
}