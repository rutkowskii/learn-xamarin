using System;
using System.Globalization;
using learn_xamarin.Model;
using Nancy;
using Newtonsoft.Json;

namespace learn_xamarin.Server
{
    public class ExpendituresModule : NancyModule
    {
        private readonly ServerRepo _serverRepo;

        public ExpendituresModule(ServerRepo serverRepo)
        {
            _serverRepo = serverRepo;
            SetupRoutes();
        }

        private void SetupRoutes()
        {
            Get[$"/{RestCallsConstants.Expenditure}"] = _ => GetAllExpenditures(ResolveParams());
            Post[$"/{RestCallsConstants.Expenditure}"] = _ => AddExpenditure();
        }

        private ExpendituresQueryParams ResolveParams()
        {
            var ignoreBelow = Request.Query[RestCallsConstants.IgnoreBelow];
            if (ignoreBelow == null) return null;

            var res = new ExpendituresQueryParams
            {
                IgnoreBelow = DateTime.ParseExact(
                    ignoreBelow, RestCallsConstants.DateFormat, CultureInfo.InvariantCulture)
            };
            return res;
        }

        private object AddExpenditure()
        {
            var len = Request.Body.Length;
            var buffer = new byte[len];
            Request.Body.Read(buffer, 0, (int)len);

            var asJson = System.Text.Encoding.Default.GetString(buffer);
            var newExpenditures = JsonConvert.DeserializeObject<Expenditure[]>(asJson);
            newExpenditures.Foreach(_serverRepo.Add);

            newExpenditures.Foreach(newExpenditure => Logger.Info($"About to insert new expenditure with sum {newExpenditure.Sum}"));

            return new object();
        }

        private Expenditure[] GetAllExpenditures(ExpendituresQueryParams queryParams)
        {
            if (queryParams == null)
            {
                Logger.Info("About to download all existing expenditures");
                return _serverRepo.GetAllExpenditures();
            }
            Logger.Info($"About to download expenditures not-older than {queryParams.IgnoreBelow}");
            return _serverRepo.Get(queryParams);
        }
    }
}