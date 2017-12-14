using System;
using learn_xamarin.Model;

namespace learn_xamarin.DataServices
{
    public interface ICategoriesDataService
    {
        void GetAll(Action<Category[]> onSuccess);
    }
}