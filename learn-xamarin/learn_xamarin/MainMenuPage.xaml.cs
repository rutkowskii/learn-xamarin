using System.Collections;
using System.Collections.Generic;
using learn_xamarin.Navigation;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class MainMenuPage : ContentPage
    {
        public ListView MenuItems => listView;

        public MainMenuPage()
        {
            InitializeComponent();

            listView.ItemsSource = PrepareItemsSource();
        }

        private IEnumerable<MainMenuItem> PrepareItemsSource()
        {
            yield return new MainMenuItem {Title = "Home", NavigationRequest = new MainMenuWelcome()};
            yield return new MainMenuItem {Title = "Categories", NavigationRequest = new MainMenuCategories()};
            yield return new MainMenuItem {Title = "Settings", NavigationRequest = new MainMenuSettings()};
            yield return new MainMenuItem {Title = "Statement", NavigationRequest = new MainMenuStatement()};
        }
    }

    public class MainMenuItem
    {
        public string Title { get; set; }
        public INavigationRequest NavigationRequest { get; set; }
    }
}
