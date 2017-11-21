using System;
using System.IO;
using System.Threading.Tasks;
using learn_xamarin.Model;
using learn_xamarin.Utils;
using Newtonsoft.Json;
using learn_xamarin.Storage;
using System.Threading;

namespace learn_xamarin.Services
{
    public class CategoriesDataService : ICategoriesDataService
    {
        private readonly ILocalDatabase _localDb;
        private readonly RestConnection _restConnection;

        public CategoriesDataService(RestConnection restConnection, ILocalDatabase localDb)
        {
            _restConnection = restConnection;
            _localDb = localDb;
        }

        public async Task<Category[]> GetAll()
        {
            var serverCallTask = _restConnection.Get(RestCallsConstants.Category);

            if (await Task.WhenAny(serverCallTask, Task.Delay(TimeSpan.FromSeconds(5))) == serverCallTask) // todo piotr making it reusable?
            {
                // task completed within timeout
                var rawResult = serverCallTask.Result;
                System.Diagnostics.Debug.WriteLine($"HTTP CALL RETURNING, Status: {rawResult.StatusCode}, Desc: {rawResult.StatusDescription} ");
                var serverResults = JsonConvert.DeserializeObject<Category[]>(rawResult.Content);
                System.Diagnostics.Debug.WriteLine($"[{serverResults.Length}] categories returned from the server, writing them to the local db");
                _localDb.UpdateCategories(serverResults);
                return serverResults;
            }

            //timeout logic. 
            var results = _localDb.GetAllCategories();
            System.Diagnostics.Debug.WriteLine($"timeout occured, getting [{results.Length}] categories from the local db");
            return results;
        }
    }
}