using learn_xamarin.Model;
using MongoDB.Driver;

namespace learn_xamarin.Sever
{
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
}