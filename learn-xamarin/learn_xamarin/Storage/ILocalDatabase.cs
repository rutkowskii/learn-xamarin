using learn_xamarin.Model;
using learn_xamarin.Services;

namespace learn_xamarin.Storage
{
    public interface ILocalDatabase
    {
        void Insert(Expenditure e);
        void Insert(UnsynchronizedItem unsynchronizedItem);
        Expenditure[] GetAllExpenditures();
        UnsynchronizedItem[] GetAllUnsynchronizedItems();
        void ClearUnsynchronizedItems();
    }
}