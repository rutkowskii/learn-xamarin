using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;

namespace learn_xamarin.Utils
{
    public interface IRestConnection
    {
        Task<IRestResponse> Get(string uri);
        Task<IRestResponse> Get(string uri, IEnumerable<RequestParameter> parameters);
        Task<IRestResponse> Get(string uri, CancellationTokenSource cts);
        Task<IRestResponse> Get(string uri, CancellationTokenSource cts, IEnumerable<RequestParameter> parameters);
        Task<IRestResponse> Post(string uri, object data);
    }
}