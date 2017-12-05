using System.Threading.Tasks;
using learn_xamarin.Navigation;
using learn_xamarin.Utils;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class MainNavigationPage : NavigationPage
    {
        private readonly INavigationService _navigationService;

        public MainNavigationPage()
        {
            InitializeComponent();
            var container = Container.Instance;
            _navigationService = container.Get<INavigationService>();
            _navigationService.NavigationRequested += InstanceOnNavigationRequested;
        }

        private void InstanceOnNavigationRequested(INavigationRequest navigationRequest)
        {
            if (navigationRequest is PushCategoriesPage) Navigation.PushAsync(new CategoriesPage(), true);
            else if (navigationRequest is PushMoneySpentSumPage) Navigation.PushAsync(new MoneySpentSumPage(), true);
            else if (navigationRequest is BackToWelcomePage ) Navigation.PopToRootAsync();
            else if (navigationRequest is MainMenuWelcome) ReplaceRoot(new WelcomePage());
            else if (navigationRequest is MainMenuCategories) ReplaceRoot(new CategoriesSummaryPage());
            else if (navigationRequest is MainMenuSettings) ReplaceRoot(new SettingsPage());
            else if (navigationRequest is MainMenuStatement) ReplaceRoot(new StatementPage());
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