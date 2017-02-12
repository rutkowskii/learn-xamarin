using System.Windows.Input;
using Xamarin.Forms;

namespace learn_xamarin
{
    public class WelcomeViewModel
    {
        private readonly INavigationService _navigationService;

        public WelcomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            System.Diagnostics.Debug.WriteLine("Hey, we inside WelcomeViewModel ctor !!!!1");
            MoneySpentCommnand = new Command(MoneySpent);
        }

        private void MoneySpent()
        {
            System.Diagnostics.Debug.WriteLine("Hey, we inside WelcomeViewmodel.MoneySpent AND SOMETHIGN CHANGED #77");
            //NavigationService.Instance.Request(new PushCategoriesPage());
            _navigationService.Request(new PushCategoriesPage());
        }


        public ICommand MoneySpentCommnand { get; private set; }
    }
}
