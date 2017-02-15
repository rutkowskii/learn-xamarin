using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Vm;
using Newtonsoft.Json;

namespace learn_xamarin.Services
{
    class ExpendituresDataService : IExpendituresDataService
    {
        private readonly ILocalDatabase _localDatabase;

        public ExpendituresDataService(ILocalDatabase localDatabase)
        {
            _localDatabase = localDatabase;
            
        }

        // todo store things in local db temporarily so we sync when we have the internet. 
        // todo modify the api so we download only the delta. 
        public void Add(Expenditure expenditure) 
        {
        }

        //todo tmp
        public Expenditure[] GetAll()
        {

            throw new NotImplementedException();
        }
    }

    //public interface IRestClient
    //{
    //    void Post(string uri, object data);
    //    Task<T> Get<T>(string uri);
    //}

    //public class RestClient : IRestClient
    //{
    //    private const string LocalhostIp = "169.254.80.80";

    //    public RestClient()
    //    {
            
    //    }

    //    public async void Post(string uri, object data)
    //    {
    //        var effectiveUri = $@"http://{LocalhostIp}/{uri}";
    //        using (var client = new HttpClient())
    //        {
    //            var response = await client.PostAsync(effectiveUri, new StringContent(JsonConvert.SerializeObject(data)));
    //            var responseString = await response.Content.ReadAsStringAsync();
    //        }
    //    }

    //    public async Task<T> Get<T>(string uri)
    //    {
    //        var effectiveUri = $@"http://{LocalhostIp}/{uri}";
    //        using (var client = new HttpClient())
    //        {
    //            var response = await client.GetAsync(effectiveUri);
    //            var responseString = await response.Content.ReadAsStringAsync();
    //            return JsonConvert.DeserializeObject<T>(responseString);
    //        }
    //    }   


    //    //private async void Post(string uri, object data)
    //    //{
    //    //    var effectiveUri = $@"http://{LocalhostIp}/{uri}";
    //    //    var request = HttpWebRequest.Create(effectiveUri) as HttpWebRequest;
    //    //    request.ContentType = "application/json";
    //    //    request.Method = "POST";
    //    //    var dataSerialized = JsonConvert.SerializeObject(data);
    //    //    //todo quick and dirty
    //    //    var buffer = 
    //    //    var requestStream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, request);
    //    //    requestStream.Write();


    //    //    Task<WebResponse> requestTask = Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, request);
    //    //    using (var response = await requestTask)
    //    //    {
    //    //        using (var stream = response.GetResponseStream())
    //    //        {
    //    //            using (var sr = new StreamReader(stream))
    //    //            {
    //    //                while (!sr.EndOfStream)
    //    //                {
    //    //                    sr.ReadLine(); //todo from hierrrr.
    //    //                }
    //    //            }
    //    //        }
    //    //    }



    //    //    var request = (HttpWebRequest)WebRequest.Create("http://www.example.com/recepticle.aspx");

    //    //    var postData = "thing1=hello";
    //    //    postData += "&thing2=world";
    //    //    var data = Encoding.ASCII.GetBytes(postData);

    //    //    request.Method = "POST";
    //    //    request.ContentType = "application/x-www-form-urlencoded";
    //    //    request.ContentLength = data.Length;

    //    //    using (var stream = request.GetRequestStream())
    //    //    {
    //    //        stream.Write(data, 0, data.Length);
    //    //    }

    //    //    var response = (HttpWebResponse)request.GetResponse();

    //    //    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
    //    //}

    //    private void HandleIncomingResponse(IAsyncResult ar)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}