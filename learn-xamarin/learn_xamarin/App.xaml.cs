using System;
using Ninject;
using Ninject.Modules;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            ((NavigationPage) MainPage).PushAsync(new WelcomePage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }


    public interface INavigationService
    {
        void Request(INavigationRequest pushCategoriesPage);
        event Action<INavigationRequest> NavigationRequested;
    }

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

    public interface INavigationRequest { }
    public class PushCategoriesPage : INavigationRequest { }

    public class Container
    {
        private static Container _instance;
        public static Container Instance => _instance ?? (_instance = new Container());
        private readonly StandardKernel _kernel;

        public Container()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<INavigationService>().To<NavigationService>().InSingletonScope();
        }

        public T Get<T>()
        {
            return _kernel.Get<T>();
        }
    }

    //public class MainModule : NinjectModule
    //{
    //    public override void Load()
    //    {
         
    //    }
    //}
}
