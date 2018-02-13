using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace learn_xamarin.Utils
{
    public class RestConnection : IRestConnection
    {
        private readonly string machineAddress = "192.168.0.26";
//        private readonly string machineAddress = "192.168.0.10";
        private readonly int portNr = 19666;
        private readonly RestClient _restClient;

        public RestConnection()
        {
            _restClient = GetClient();
        }

        public Task<IRestResponse> Get(string uri)
        {
            return Get(uri, DefaultCancellationTokenSource);
        }

        public Task<IRestResponse> Get(string uri, IEnumerable<RequestParameter> parameters)
        {
            return Get(uri, DefaultCancellationTokenSource, parameters);
        }
        
        public Task<IRestResponse> Get(string uri, CancellationTokenSource cts)
        {
            return Get(uri, cts, Enumerable.Empty<RequestParameter>());
        }

        public Task<IRestResponse> Get(string uri, CancellationTokenSource cts, IEnumerable<RequestParameter> parameters)
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
            return _restClient.Execute(request, cts.Token);
        }

        public async Task<IRestResponse> Post(string uri, object data)
        {
            var request = new RestRequest(uri, Method.POST);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(data);
            return await _restClient.Execute(request);
        }
        
        private CancellationTokenSource DefaultCancellationTokenSource => new CancellationTokenSource(5000);

        private RestClient GetClient()
        {
            return new RestClient($"http://{machineAddress}:{portNr}/");
        }
    }
}
