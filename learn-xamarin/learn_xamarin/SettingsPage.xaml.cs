using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using learn_xamarin.Utils;
using learn_xamarin.Vm;
using Xamarin.Forms;

namespace learn_xamarin
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = Container.Instance.Get<SettingsViewModel>();
        }

        private async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var itemCasted = e.Item as SettingsViewModel.SettingsItem;
            ((ListView)sender).SelectedItem = null; // get rid of the selection, otherwise, the item will stay highlighted 
            itemCasted.Cmd.Execute(this);
        }
    }
}
