using learn_xamarin.Navigation;
using learn_xamarin.Vm;
using Ninject;

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
            _kernel.Bind<INavigationService>().To<NavigationService>().InSingletonScope();
            _kernel.Bind<MoneySpentDialogViewModel>().To<MoneySpentDialogViewModel>().InSingletonScope();
        }

        public T Get<T>()
        {
            return _kernel.Get<T>();
        }
    }
}