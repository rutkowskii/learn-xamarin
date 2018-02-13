using System.Threading.Tasks;
using Moq;
using RestSharp.Portable;

namespace Tests.Utils
{
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