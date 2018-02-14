using learn_xamarin.AppSettings;
using learn_xamarin.Cache;
using learn_xamarin.DataServices;
using learn_xamarin.Navigation;
using learn_xamarin.Storage;
using learn_xamarin.UiUtils;
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

        private Container()
        {
            _kernel = new StandardKernel();
            new BasicInstaller().RunInstallation(_kernel);
        }

        public T Get<T>()
        {
            return _kernel.Get<T>();
        }
    }
    
    public class BasicInstaller
    {
        public void RunInstallation(IKernel kernel)
        {
            kernel.Bind<IFilePathProvider>().To<XamarinFilePathProvider>().InSingletonScope();
            kernel.Bind<ILocalDatabase>().To<LocalDatabase>().InSingletonScope();
            kernel.Bind<IRestConnection>().To<RestConnection>().InSingletonScope();
            kernel.Bind<ISettingsRepo>().To<SettingsRepo>().InSingletonScope();
            kernel.Bind<IAppSettingsDictionaryProvider>().To<AppSettingsDictionaryProvider>().InSingletonScope();
            kernel.Bind<IConnectionService>().To<ConnectionService>().InSingletonScope();
            kernel.Bind<INavigationService>().To<NavigationService>().InSingletonScope();
            kernel.Bind<RestClient>().To<RestClient>().InSingletonScope();
            kernel.Bind<IDateTimeProvider>().To<DateTimeProvider>().InSingletonScope();
            kernel.Bind<IExpendituresCache>().To<ExpendituresCache>().InSingletonScope();

            kernel.Bind<MoneySpentDialogViewModel>().To<MoneySpentDialogViewModel>().InSingletonScope();

            kernel.Bind<ICategoriesDataService>().To<CategoriesDataService>().InSingletonScope();
            kernel.Bind<IExpendituresDataService>().To<ExpendituresDataService>().InSingletonScope();
            kernel.Bind<IDialogService>().To<DialogService>().InSingletonScope();
            kernel.Bind<IExchangeRateDataService>().To<ExchangeRateDataService>().InSingletonScope();
        }
    }
}