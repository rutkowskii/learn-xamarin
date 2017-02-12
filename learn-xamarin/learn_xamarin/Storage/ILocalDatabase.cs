using learn_xamarin.Model;

namespace learn_xamarin.Storage
{
    public interface ILocalDatabase
    {
        void Insert(Expenditure e);
        Expenditure[] GetAllExpenditures();
    }
}