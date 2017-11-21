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
            // todo piotr add settings page here., 
            // in the settings page: main currency and current currency 
            // also store them in the local storage 
        }
    }

    public class MainMenuItem
    {
        public string Title { get; set; }

        public INavigationRequest NavigationRequest { get; set; }
    }
}
