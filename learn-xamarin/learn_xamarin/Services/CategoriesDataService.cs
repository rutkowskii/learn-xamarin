using System;
using System.IO;
using System.Threading.Tasks;
using learn_xamarin.Model;
using learn_xamarin.Utils;
using Newtonsoft.Json;

namespace learn_xamarin.Services
{
    public class CategoriesDataService : ICategoriesDataService
    {
        private readonly RestConnection _restConnection;

        public CategoriesDataService(RestConnection restConnection)
        {
            _restConnection = restConnection;
        }

        public async Task<Category[]> GetAll()
        {
            var task = _restConnection.Get(RestCallsConstants.Category);
            var rawResult = await task;
            System.Diagnostics.Debug.WriteLine($"HTTP CALL RETURNING, Status: {rawResult.StatusCode}, Desc: {rawResult.StatusDescription} ");
            var results = JsonConvert.DeserializeObject<Category[]>(rawResult.Content);
            System.Diagnostics.Debug.WriteLine($"[{results.Length}] categories returned from the server");
            return results;
        }
    }

    public class LocalDb
    {
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "expenditures.db3");
    }
}