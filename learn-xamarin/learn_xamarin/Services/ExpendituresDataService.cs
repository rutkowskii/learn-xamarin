using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Vm;

namespace learn_xamarin.Services
{
    class ExpendituresDataService : IExpendituresDataService
    {
        private readonly ILocalDatabase _localDatabase;

        public ExpendituresDataService(ILocalDatabase localDatabase)
        {
            _localDatabase = localDatabase;
        }

        public void Add(Expenditure expenditure)
        {
            //todo backend access 
            _localDatabase.Insert(expenditure);
        }

        //todo tmp
        public Expenditure[] GetAll()
        {
            return _localDatabase.GetAllExpenditures();
        }
    }

    public class ExpendituresRestClient 
    {
        public ExpendituresRestClient()
        {
            
        }

        public void Add(Expenditure expenditure)
        {
            
        }

        private async void Post(string uri, object data)
        {
            var request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.ContentType = "application/request";
            request.Method = "POST";

            Task<WebResponse> requestTask = Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, request);
            using (var response = await requestTask)
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        while (!sr.EndOfStream)
                        {
                            sr.ReadLine(); //todo from hierrrr.
                        }
                    }
                }
            }

        }

        private void HandleIncomingResponse(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }
    }
}