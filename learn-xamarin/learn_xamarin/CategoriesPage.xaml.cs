using learn_xamarin.Utils;
using learn_xamarin.Vm;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class CategoriesPage : ContentPage
    {
        public CategoriesPage()
        {
            InitializeComponent();
            BindingContext = Container.Instance.Get<CategoriesViewModel>();
        }
    }
}
