using System.Threading.Tasks;
using learn_xamarin.Navigation;
using learn_xamarin.Utils;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class MyNavigationPage : NavigationPage
    {
        private readonly INavigationService _navigationService;

        public MyNavigationPage()
        {
            InitializeComponent();
            var container = Container.Instance;
            _navigationService = container.Get<INavigationService>();
            _navigationService.NavigationRequested += InstanceOnNavigationRequested;
        }

        private void InstanceOnNavigationRequested(INavigationRequest navigationRequest)
        {
            System.Diagnostics.Debug.WriteLine($"Hey, we inside on navigation requested, type is {navigationRequest.GetType().FullName}");

            if (navigationRequest is PushCategoriesPage) Navigation.PushAsync(new CategoriesPage(), true);
            else if (navigationRequest is PushMoneySpentSumPage) Navigation.PushAsync(new MoneySpentSumPage(), true);
            else if (navigationRequest is BackToWelcomePage) Navigation.PopToRootAsync();
            else if (navigationRequest is MainMenuWelcome) ReplaceRoot(new WelcomePage());
            else if (navigationRequest is MainMenuCategories) ReplaceRoot(new CategoriesSummaryPage());
        }

        async Task ReplaceRoot<TPage>(TPage page) where TPage : Page
        {
            var root = Navigation.NavigationStack[0];
            if (root is TPage)
            {
                Navigation.PopToRootAsync();
            }
            else
            {
                Navigation.InsertPageBefore(page, root);
                await PopToRootAsync();
            }
        }
    }
}