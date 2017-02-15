using System;
using System.Linq;
using learn_xamarin.Model;
using MongoDB.Driver;
using Nancy;
using Nancy.Hosting.Self;
using Newtonsoft.Json;

namespace learn_xamarin.Sever
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new Uri("http://localhost:19666")))
            {
                host.Start();
                Console.WriteLine("Running on http://localhost:19666");
                Console.ReadLine();
            }
        }
    }

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

    public class ExpendituresRepo
    {
        private readonly MongoDb _mongoDb;

        public ExpendituresRepo(MongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }        

        public Expenditure[] GetAll()
        {
            return _mongoDb.Expenditures.AsQueryable().ToArray();
        }

        public void Add(Expenditure newExpenditure)
        {
            _mongoDb.Expenditures.InsertOne(newExpenditure);
        }
    }

    public class MongoDb
    {
        private static IMongoDatabase _db;

        private static IMongoDatabase Instance
        {
            get
            {
                if (_db == null)
                {
                    var connectionString = "mongodb://localhost:27017";
                    var mongoClient = new MongoClient(connectionString);
                    _db = mongoClient.GetDatabase("expenditures");
                }
                return _db;
            }
        }

        public IMongoCollection<Category> Categories 
            => Instance.GetCollection<Category>("categories");

        public IMongoCollection<Expenditure> Expenditures 
            => Instance.GetCollection<Expenditure>("expenditures");
    }

    //public class ServerKernel
    //{
    //    private static IKernel _instance;
    //    public static IKernel Instance => _instance ?? (_instance = new StandardKernel());
    //}


}
