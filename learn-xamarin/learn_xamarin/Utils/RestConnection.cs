using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace learn_xamarin.Utils
{
    public class RestConnection
    {
        private RestClient _restClient;

        public RestConnection()
        {
            _restClient = GetClient();
        }

        public Task<IRestResponse> Get(string uri)
        {
            return Get(uri, Enumerable.Empty<RequestParameter>());
        }

        public Task<IRestResponse> Get(string uri, IEnumerable<RequestParameter> parameters)
        {
            var request = new RestRequest(uri, Method.GET);
            foreach (var parameter in parameters)
            {
                request.Parameters.Add(new Parameter
                {
                    Name = parameter.Key,
                    Value = parameter.Value
                });
            }
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

    public class RequestParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}