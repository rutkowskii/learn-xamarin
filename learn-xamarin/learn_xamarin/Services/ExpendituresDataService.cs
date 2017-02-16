using System;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Utils;
using Newtonsoft.Json;
using RestSharp.Portable.Deserializers;

namespace learn_xamarin.Services
{
    class ExpendituresDataService : IExpendituresDataService
    {
        private readonly ILocalDatabase _localDatabase;
        private readonly RestConnection _restConnection;

        public ExpendituresDataService(ILocalDatabase localDatabase, RestConnection restConnection)
        {
            _localDatabase = localDatabase;
            _restConnection = restConnection;
        }

        // todo store things in local db temporarily so we sync when we have the internet. 
        public void Add(Expenditure expenditure) 
        {
            _localDatabase.Insert(expenditure);
            _restConnection.Post("expenditure", expenditure);
        }

        //todo naive implementation - download only the 'delta'
        public async void GetAll(Action<Expenditure[]> callback)
        {
            var task = _restConnection.Get("expenditure");
            await task;
            //todo combine w/ local db. 
            callback(JsonConvert.DeserializeObject<Expenditure[]>(task.Result.Content));
        }
    }
}