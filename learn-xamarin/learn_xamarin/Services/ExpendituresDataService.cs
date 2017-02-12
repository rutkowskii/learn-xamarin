using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Vm;

namespace learn_xamarin.Services
{
    class ExpendituresDataService : IExpendituresDataService
    {
        private readonly ILocalDatabase _localDatabase;

        public ExpendituresDataService(ILocalDatabase localDatabase)
        {
            _localDatabase = localDatabase;
        }

        public void Add(Expenditure expenditure)
        {
            //todo backend access 
            _localDatabase.Insert(expenditure);
        }

        //todo tmp
        public Expenditure[] GetAll()
        {
            return _localDatabase.GetAllExpenditures();
        }
    }
}