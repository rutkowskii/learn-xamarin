using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using learn_xamarin.Utils;
using learn_xamarin.Vm;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class MoneySpentSumPage : ContentPage
    {
        public MoneySpentSumPage()
        {
            System.Diagnostics.Debug.WriteLine($"we inside moneyspentsumpage ctor!!!!!!!!!!!!!!!!!");
            InitializeComponent();
            BindingContext = Container.Instance.Get<MoneySpentSumViewModel>();
        }
    }
}
