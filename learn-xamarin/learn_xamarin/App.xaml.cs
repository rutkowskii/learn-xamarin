using System;
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

            MainPage = new MainPage();
            ((NavigationPage) MainPage).PushAsync(new WelcomePage());
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
