using System;
using learn_xamarin.Model;

namespace learn_xamarin.Services
{
    public interface ICategoriesDataService
    {
        void GetAll(Action<Category[]> onSuccess);
    }
}