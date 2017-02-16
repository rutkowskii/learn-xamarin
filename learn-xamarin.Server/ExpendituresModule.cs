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
            Get["/expenditure"] = _ => GetAllExpenditures();
            Post["/expenditure"] = _ => AddExpenditure();
        }

        private object AddExpenditure()
        {
            var len = Request.Body.Length;
            var buffer = new byte[len];
            Request.Body.Read(buffer, 0, (int)len);

            var asJson = System.Text.Encoding.Default.GetString(buffer);
            var newExpenditure = JsonConvert.DeserializeObject<Expenditure>(asJson);
            _expendituresRepo.Add(newExpenditure);
            return new object();
        }

        private Expenditure[] GetAllExpenditures()
        {
            return _expendituresRepo.GetAll();
        }
    }
}