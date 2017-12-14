using System;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Utils;
using Newtonsoft.Json;
using RestSharp.Portable;

namespace learn_xamarin.DataServices
{
    public class CategoriesDataService : ICategoriesDataService
    {
        private readonly ILocalDatabase _localDb;
        private readonly IRestConnection _restConnection;

        public CategoriesDataService(IRestConnection restConnection, ILocalDatabase localDb)
        {
            _restConnection = restConnection;
            _localDb = localDb;
        }

        public void GetAll(Action<Category[]> onSuccess)
        {
            AsyncOp.Get(
                asyncOp: () => _restConnection.Get(RestCallsConstants.Category),
                onSuccess: restResult => onSuccess(OnCategoriesLoadSuccess(restResult)),
                onCancel: () => onSuccess(LoadStoredLocally()),
                onFailure: e => onSuccess(LoadStoredLocally())
            ).Run();
        }

        private Category[] OnCategoriesLoadSuccess(IRestResponse rawResult)
        {
            System.Diagnostics.Debug.WriteLine(
                $"HTTP CALL RETURNING, Status: {rawResult.StatusCode}, Desc: {rawResult.StatusDescription} ");
            var serverResults = JsonConvert.DeserializeObject<Category[]>(rawResult.Content);
            System.Diagnostics.Debug.WriteLine(
                $"[{serverResults.Length}] categories returned from the server, writing them to the local db");
            _localDb.UpdateCategories(serverResults);
            return serverResults;
        }

        private Category[] LoadStoredLocally()
        {
            var results = _localDb.GetAllCategories();
            System.Diagnostics.Debug.WriteLine($"timeout occured, getting [{results.Length}] categories from the local db");
            return results;
        }
    }
}
