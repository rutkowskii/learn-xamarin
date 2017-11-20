using System;
using System.Threading.Tasks;
using learn_xamarin.Model;
using learn_xamarin.Utils;
using learn_xamarin.Vm;
using Newtonsoft.Json;

namespace learn_xamarin.Services
{
    public class CategoriesDataService : ICategoriesDataService //todo piotr name 
    {
        private readonly RestConnection _restConnection;

        public CategoriesDataService(RestConnection restConnection)
        {
            _restConnection = restConnection;
        }

        public async Task<Category[]> GetAll()
        {
            //return new[] //todo get from the backend!!
            //{
            //    new Category {Id = new Guid("B1EE74CD-A750-4352-967B-C7A443782634"), Name = "Tesco"},
            //    new Category {Id = new Guid("1733106D-36E9-491B-BBA1-85723225EC13"), Name = "Oyster"},
            //    new Category {Id = new Guid("28415902-893D-4E06-91D7-85751E60EB11"), Name = "Chata"},
            //    new Category {Id = new Guid("1F474620-1A95-43CB-97B1-2B64241F356F"), Name = "Piwo"},
            //    new Category {Id = new Guid("1F474620-1A95-43CB-97B1-2B64241F356F"), Name = "Lancz"},
            //};
            //todo how to store it?

            var task = _restConnection.Get(RestCallsConstants.Category);
            var rawResult = await task;
            System.Diagnostics.Debug.WriteLine($"HTTP CALL RETURNING, Status: {rawResult.StatusCode}, Desc: {rawResult.StatusDescription} ");
            return JsonConvert.DeserializeObject<Category[]>(rawResult.Content);
        }
    }
}