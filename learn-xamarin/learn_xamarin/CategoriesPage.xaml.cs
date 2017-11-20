using learn_xamarin.Utils;
using learn_xamarin.Vm;
using Xamarin.Forms;
using System;

namespace learn_xamarin
{
    public partial class CategoriesPage : ContentPage
    {
        public CategoriesPage()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"we try to create categories page");
                BindingContext = Container.Instance.Get<CategoriesViewModel>();
            System.Diagnostics.Debug.WriteLine($"inside categoriespage ctor, binding context is: {BindingContext.ToString()}");
        }
    }
}
