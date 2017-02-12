using System.Windows.Input;
using learn_xamarin.Navigation;
using Xamarin.Forms;

namespace learn_xamarin.Vm
{
    public class WelcomeViewModel
    {
        private readonly INavigationService _navigationService;

        public WelcomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            MoneySpentCommnand = new Command(MoneySpent);
        }

        private void MoneySpent()
        {
            _navigationService.Request(new PushCategoriesPage());
        }

        public ICommand MoneySpentCommnand { get; private set; }
    }
}