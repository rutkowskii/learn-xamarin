using System.Threading.Tasks;
using learn_xamarin.Navigation;
using learn_xamarin.Utils;
using learn_xamarin.Vm;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class StatementPage : ContentPage
    {
        public StatementPage()
        {
            InitializeComponent();
            BindingContext = Container.Instance.Get<StatementViewModel>();
        }
    }
}