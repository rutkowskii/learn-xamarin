using System;

namespace learn_xamarin.Navigation
{
    public class NavigationService : INavigationService
    {
        private static NavigationService _instance;

        public static NavigationService Instance
        {
            get { return _instance ?? (_instance = new NavigationService()); }
        }

        public void Request(INavigationRequest pushCategoriesPage)
        {
            System.Diagnostics.Debug.WriteLine("Hey, we inside navigation service !!!!2");
            NavigationRequested?.Invoke(pushCategoriesPage);
        }

        public event Action<INavigationRequest> NavigationRequested;
    }
}