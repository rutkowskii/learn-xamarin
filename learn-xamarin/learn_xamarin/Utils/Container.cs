using learn_xamarin.Navigation;
using learn_xamarin.Services;
using learn_xamarin.Storage;
using learn_xamarin.Vm;
using Ninject;
using RestSharp.Portable.HttpClient;

namespace learn_xamarin.Utils
{
    public class Container
    {
        private static Container _instance;
        public static Container Instance => _instance ?? (_instance = new Container());
        private readonly StandardKernel _kernel;

        public Container()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ILocalDatabase>().To<LocalDatabase>().InSingletonScope();
            _kernel.Bind<IConnectionService>().To<ConnectionService>().InSingletonScope();
            _kernel.Bind<INavigationService>().To<NavigationService>().InSingletonScope();
            _kernel.Bind<RestClient>().To<RestClient>().InSingletonScope();
            _kernel.Bind<IDateTimeProvider>().To<DateTimeProvider>().InSingletonScope();

            _kernel.Bind<MoneySpentDialogViewModel>().To<MoneySpentDialogViewModel>().InSingletonScope();

            _kernel.Bind<ICategoriesDataService>().To<CategoriesDataService>().InSingletonScope();
            _kernel.Bind<IExpendituresDataService>().To<ExpendituresDataService>().InSingletonScope();
           
        }

        public T Get<T>()
        {
            return _kernel.Get<T>();
        }
    }
}