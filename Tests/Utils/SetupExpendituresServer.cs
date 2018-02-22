using System.Collections.Generic;
using System.Threading.Tasks;
using learn_xamarin.Model;
using Moq;
using Newtonsoft.Json;
using RestSharp.Portable;

namespace Tests.Utils
{
    class SetupExpendituresServer : ITestSetup
    {
        private readonly List<Expenditure> _serverExpenditures = new List<Expenditure>();

        public SetupExpendituresServer WithExpenditures(IEnumerable<Expenditure> expenditures)
        {
            _serverExpenditures.AddRange(expenditures);
            return this;
        }

        public void Setup(TestingContext testingContext)
        {
            testingContext.RestConnection
                .Setup(r => r.Post(RestCallsConstants.Expenditure, It.IsAny<object>()))
                .Callback<string, object>(PostCallback)
                .Returns(BuildServerResponseTask(null));

            testingContext.RestConnection
                .Setup(r => r.Get(RestCallsConstants.Expenditure))
                .Returns(BuildServerResponseTask(_serverExpenditures));
        }

        public object LastData { get; private set; }
        public string LastUri { get; private set; }
        public IEnumerable<Expenditure> ServerExpenditures => _serverExpenditures.ToArray();

        private Task<IRestResponse> BuildServerResponseTask(object responseContent)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            
            var response = new Mock<IRestResponse>();
            response.Setup(r => r.Content).Returns(JsonConvert.SerializeObject(responseContent));
            tcs.SetResult(response.Object);
            
            return tcs.Task;
        }

        private void PostCallback(string uri, object data)
        {
            SetLastData(uri, data);
            var newExpenditures = data as IEnumerable<Expenditure>;
            newExpenditures?.Foreach(e => _serverExpenditures.Add(e));
        }

        private void SetLastData(string uri, object data)
        {
            LastUri = uri;
            LastData = data;
        }
    }
}
