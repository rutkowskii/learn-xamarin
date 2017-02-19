using System.Linq;
using learn_xamarin.Model;
using MongoDB.Driver;

namespace learn_xamarin.Sever
{
    public class ServerRepo
    {
        private readonly MongoDb _mongoDb;

        public ServerRepo(MongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }        

        public Expenditure[] GetAllExpenditures()
        {
            return _mongoDb.Expenditures.AsQueryable().ToArray();
        }

        public Category[] GetAllCategories()
        {
            return _mongoDb.Categories.AsQueryable().ToArray();
        }

        public void Add(Expenditure newExpenditure)
        {
            _mongoDb.Expenditures.InsertOne(newExpenditure);
        }

        public Expenditure[] Get(ExpendituresQueryParams queryParams)
        {
            return _mongoDb.Expenditures.AsQueryable()
                .Where(e => e.Timestamp >= queryParams.IgnoreBelow)
                .ToArray();
        }
    }
}