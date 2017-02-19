using System.Linq;
using learn_xamarin.Model;
using MongoDB.Driver;

namespace learn_xamarin.Sever
{
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

        public Expenditure[] Get(ExpendituresQueryParams queryParams)
        {
            return _mongoDb.Expenditures.AsQueryable()
                .Where(e => e.Timestamp >= queryParams.IgnoreBelow)
                .ToArray();
        }
    }
}