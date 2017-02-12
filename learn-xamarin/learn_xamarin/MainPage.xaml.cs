using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class MainPage : NavigationPage
    {
        public MainPage()
        {
            InitializeComponent();
            var container = Container.Instance;
            container.Get<INavigationService>().NavigationRequested += InstanceOnNavigationRequested;
        }

        private void InstanceOnNavigationRequested(INavigationRequest navigationRequest)
        {
            if (navigationRequest is PushCategoriesPage)
                Navigation.PushAsync(new CategoriesPage());
        }
    }
}
