using System;

namespace learn_xamarin.Navigation
{
    public class NavigationService : INavigationService
    {
        public void Request(INavigationRequest pushCategoriesPage)
        {
            System.Diagnostics.Debug.WriteLine("Hey, we inside navigation service !!!!2");
            NavigationRequested?.Invoke(pushCategoriesPage);
        }

        public event Action<INavigationRequest> NavigationRequested;
    }
}