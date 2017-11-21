using System;
using System.Linq;
using learn_xamarin.Model;
using Nancy.Hosting.Self;

namespace learn_xamarin.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new Uri("http://localhost:19666")))
            {
                InitialDataLoader.Run();

                host.Start();
               Logger.Info("Running on http://localhost:19666, press any key to quit");
                Console.ReadLine();
            }
        }
    }

    // used just for test purposes
    class InitialDataLoader
    {
        public static void Run()
        {
           Logger.Info("Above to start initial load");

            var db = new MongoDb();
            
            MongoDb.Instance.DropCollection("expenditures");
            // ResetCategories(db);
           Logger.Info("Initial load completed successfully");
        }

        private static void ResetCategories(MongoDb db)
        {
            MongoDb.Instance.DropCollection("categories");
            var categoryNames = new[] { "życie", "rachunki", "lekarze", "dziecko", "rozpusta", "rtv", "remonty" };
            db.Categories.InsertMany(categoryNames.Select(x => new Category { Name = x, Id = Guid.NewGuid() })); ;
        }
    }
}
