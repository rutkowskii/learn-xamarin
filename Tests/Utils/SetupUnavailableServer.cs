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
}