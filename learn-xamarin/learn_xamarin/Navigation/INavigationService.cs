using System;

namespace learn_xamarin.Navigation
{
    public interface INavigationService
    {
        void Request(INavigationRequest pushCategoriesPage);
        event Action<INavigationRequest> NavigationRequested;
    }
}