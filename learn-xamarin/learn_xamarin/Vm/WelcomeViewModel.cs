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


    public class MoneySpentSumViewModel
    {
        private string _sum;

        public MoneySpentSumViewModel()
        {
            ConfirmationCommand = new Command(ConfirmSum);
        }

        private void ConfirmSum()
        {
            return;
        }

        public ICommand ConfirmationCommand { get; private set; }

        public string Sum
        {
            get { return _sum; }
            set { _sum = value; }
        }
    }
}
