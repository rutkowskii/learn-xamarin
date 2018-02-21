using System.Collections.Generic;
using System.Threading.Tasks;
using learn_xamarin.Model;
using Moq;
using RestSharp.Portable;

namespace Tests.Utils
{
    class SetupStubServer : ITestSetup
    {
        private readonly List<Expenditure> _serverExpenditures = new List<Expenditure>();
        
        public SetupStubServer WithExpenditures(IEnumerable<Expenditure> expenditures)
        {
            _serverExpenditures.AddRange(expenditures);
            return this;
        }
        
        public void Setup(TestingContext testingContext)
        {
            testingContext.RestConnection
                .Setup(r => r.Post(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>(PostCallback)
                .Returns(StubServerTask);
        }
        public object LastData { get; private set; }
        public string LastUri { get; private set; }
        public IEnumerable<Expenditure> ServerExpenditures => _serverExpenditures.ToArray();

        private Task<IRestResponse> StubServerTask
        {
            get
            {
                var tcs = new TaskCompletionSource<IRestResponse>();
                tcs.SetResult(null);
                return tcs.Task;
            }
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
