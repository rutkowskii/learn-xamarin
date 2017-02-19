using System;
using System.Globalization;
using learn_xamarin.Model;
using Nancy;
using Newtonsoft.Json;

namespace learn_xamarin.Sever
{
    public class ExpendituresModule : NancyModule
    {
        private readonly ExpendituresRepo _expendituresRepo;

        public ExpendituresModule(ExpendituresRepo expendituresRepo)
        {
            _expendituresRepo = expendituresRepo;
            SetupRoutes();
        }

        private void SetupRoutes()
        {
            Get[$"/{RestCallsConstants.Expenditure}"] = request => GetAllExpenditures(ResolveParams());
            Post[$"/{RestCallsConstants.Expenditure}"] = _ => AddExpenditure();
        }

        private ExpendituresQueryParams ResolveParams()
        {
            var ignoreBelow = this.Request.Query[RestCallsConstants.IgnoreBelow];
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
            var newExpenditure = JsonConvert.DeserializeObject<Expenditure>(asJson);
            _expendituresRepo.Add(newExpenditure);

            Logger.Info($"About to insert new expenditure with sum {newExpenditure.Sum}");

            return new object();
        }

        private Expenditure[] GetAllExpenditures(ExpendituresQueryParams queryParams)
        {
            if (queryParams == null)
            {
                Logger.Info("About to download all existing expenditures");
                return _expendituresRepo.GetAll();
            }
            Logger.Info($"About to download expenditures not-older than {queryParams.IgnoreBelow}");
            return _expendituresRepo.Get(queryParams);
        }
    }

    public class ExpendituresQueryParams
    {
        public DateTime IgnoreBelow { get; set; }
    }

    public class Logger
    {
        public static void Info(string content)
        {
            Console.WriteLine(content);
        }
    }
}