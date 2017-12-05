using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace learn_xamarin.Utils
{
    public class RestConnection
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

        public async void Post(string uri, object data)
        {
            var request = new RestRequest(uri, Method.POST);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(data);
            await _restClient.Execute(request);
        }
        
        private CancellationTokenSource DefaultCancellationTokenSource => new CancellationTokenSource(5000);

        private RestClient GetClient()
        {
            return new RestClient($"http://{machineAddress}:{portNr}/");
        }
    }

    public class AsyncOp<T>
    {
        private readonly Func<Task<T>> _taskPrep;

        private readonly Action<T> _onSuccess;

//        private readonly Action<T> _onSuccess;
        private readonly Action<Exception> _onFailure;
        private readonly Action _onCancel;
        private T _result;

        public AsyncOp(Func<Task<T>> taskPrep, Action<T> onSuccess, Action<Exception> onFailure, Action onCancel)
        {
            _taskPrep = taskPrep;
            _onSuccess = onSuccess;
            _onFailure = onFailure;
            _onCancel = onCancel;
        }

        public async void Run()
        {
            var asyncOpResult = await RunCore();
            if (asyncOpResult.ResultFlag == OperationResult.Completed)
            {
                _onSuccess?.Invoke(asyncOpResult.Result);
            }
        }

        private async Task<AsyncOpResult<T>> RunCore()
        {
            try
            {
                var task = _taskPrep();
                var resultCore = await task;
                return new AsyncOpResult<T>
                {
                    ResultFlag = OperationResult.Completed, 
                    Result = resultCore
                };
            }
            catch (OperationCanceledException e)
            {
                _onCancel.Invoke();
                return new AsyncOpResult<T>{ResultFlag = OperationResult.Cancelled};
            }
            catch (Exception e)
            {
                _onFailure?.Invoke(e);
                return new AsyncOpResult<T>{ResultFlag = OperationResult.Failed};
            }
        }
        
        enum OperationResult{ Cancelled, Completed, Failed}

        class AsyncOpResult<T>
        {
            public OperationResult ResultFlag { get; set; }
            public T Result { get; set; }
        }
    }

    public static class AsyncOp // todo piotr fluent builder. 
    {
        public static AsyncOp<T> Get<T>(Func<Task<T>> asyncOp, Action<T> onSuccess, Action<Exception> onFailure, Action onCancel)
        {
            return new AsyncOp<T>(asyncOp, onSuccess, onFailure, onCancel);
        }
    }
}