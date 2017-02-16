using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace learn_xamarin.Utils
{
    class RestConnection
    {
        private RestClient _restClient;

        public RestConnection()
        {
            _restClient = GetClient();
        }

        public Task<IRestResponse> Get(string uri)
        {
            var request = new RestRequest(uri, Method.GET);
            return _restClient.Execute(request);
        }

        public async void Post(string uri, object data)
        {
            var request = new RestRequest(uri, Method.POST);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(data);
            await _restClient.Execute(request);
        }

        private RestClient GetClient()
        {
            return new RestClient("http://10.0.2.2:19666/");
        }
    }
}