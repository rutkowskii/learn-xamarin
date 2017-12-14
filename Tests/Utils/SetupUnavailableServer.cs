using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using learn_xamarin.Utils;
using Moq;
using RestSharp.Portable;

namespace Tests.Utils
{
    class SetupUnavailableServer : ITestSetup
    {
        public void Setup(TestingContext testingContext)
        {
            testingContext.RestConnection
                .Setup(r => r.Post(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(TaskThrowing);
            testingContext.RestConnection
                .Setup(r => r.Get(It.IsAny<string>(), It.IsAny<IEnumerable<RequestParameter>>()))
                .Returns(TaskThrowing);
            testingContext.RestConnection
                .Setup(r => r.Get(It.IsAny<string>(), It.IsAny<CancellationTokenSource>()))
                .Returns(TaskThrowing);
            testingContext.RestConnection
                .Setup(r => r.Get(It.IsAny<string>(), It.IsAny<CancellationTokenSource>(), It.IsAny<IEnumerable<RequestParameter>>()))
                .Returns(TaskThrowing);
        }

        private Task<IRestResponse> TaskThrowing
        {
            get
            {
                var tcs = new TaskCompletionSource<IRestResponse>();
                tcs.SetException(new InvalidOperationException());
                return tcs.Task;
            }
        }
    }

    class SetupStubServer : ITestSetup
    {
        public void Setup(TestingContext testingContext)
        {
            testingContext.RestConnection
                .Setup(r => r.Post(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>(Callback)
                .Returns(StubServerTask);
        }

        private void Callback(string uri, object data)
        {
            LastUri = uri;
            LastData = data;
        }

        public object LastData { get; private set; }
        public string LastUri { get; private set; }

        private Task<IRestResponse> StubServerTask
        {
            get
            {
                var tcs = new TaskCompletionSource<IRestResponse>();
                tcs.SetResult(null);
                return tcs.Task;
            }
        }
    }
}