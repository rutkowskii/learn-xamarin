using System;
using learn_xamarin.Navigation;
using learn_xamarin.Utils;
using Ninject;
using Ninject.Modules;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
           
            var mainPage = new MainPage();

            mainPage.Detail = new MainNavigationPage();
            var mainMenuPage = new MainMenuPage();
            mainMenuPage.MenuItems.ItemSelected += onMainMenuItemSelected;

            mainPage.Master = mainMenuPage;

            MainPage = mainPage;
            ((NavigationPage)mainPage.Detail).PushAsync(new WelcomePage());
        }

        private void onMainMenuItemSelected(object sender, SelectedItemChangedEventArgs selectedItemChangedEventArgs) // todo piotr move it to main menu page?
        {
            var itemSelected = selectedItemChangedEventArgs.SelectedItem as MainMenuItem;
            //NavigationService.Instance.Request(itemSelected.NavigationRequest);
            Container.Instance.Get<INavigationService>().Request(itemSelected.NavigationRequest);
            //NavigationService.Instance.Request(itemSelected.NavigationRequest);
            ((MainPage) MainPage).Hide();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

}
