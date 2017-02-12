using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Utils;
using learn_xamarin.Vm;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            BindingContext = Container.Instance.Get<WelcomeViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as WelcomeViewModel).RefreshSummaryInfos();    
        }
    }
}
