using learn_xamarin.Model;
using learn_xamarin.Services;

namespace learn_xamarin.Storage
{
    public interface ILocalDatabase
    {
        void UpdateCategories(Category[] e);
        Category[] GetAllCategories();

        void Insert(UnsynchronizedItem unsynchronizedItem);
        Expenditure[] GetAllExpenditures();
        UnsynchronizedItem[] GetAllUnsynchronizedItems();
        void ClearUnsynchronizedItems();
    }
}