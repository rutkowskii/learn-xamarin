using learn_xamarin.Utils;
using learn_xamarin.Vm;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class MoneySpentSumPage : ContentPage
    {
        public MoneySpentSumPage()
        {
            InitializeComponent();
            BindingContext = Container.Instance.Get<MoneySpentSumViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.sumEntry.Focus();
        }
    }
}
