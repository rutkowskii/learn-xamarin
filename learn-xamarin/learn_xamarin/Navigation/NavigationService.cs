using System;

namespace learn_xamarin.Navigation
{
    public class NavigationService : INavigationService
    {
        //private static NavigationService _instance;
        //private Guid id = Guid.NewGuid(); //todo tmp
        //private static  object monitor = new object();
        //public static NavigationService Instance
        //{
        //    get
        //    {
        //        lock (monitor)
        //        {
        //            if (_instance != null) return _instance;
        //            else return _instance = new NavigationService();
        //        }
        //    }
        //}

        public void Request(INavigationRequest pushCategoriesPage)
        {
            System.Diagnostics.Debug.WriteLine("Hey, we inside navigation service !!!!2");
            NavigationRequested?.Invoke(pushCategoriesPage);
        }

        public event Action<INavigationRequest> NavigationRequested;
    }
}