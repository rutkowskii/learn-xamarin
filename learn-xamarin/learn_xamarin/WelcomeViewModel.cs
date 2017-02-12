using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace learn_xamarin
{
    public class WelcomeViewModel
    {
        public WelcomeViewModel()
        {
            System.Diagnostics.Debug.WriteLine("Hey, we inside WelcomeViewModel ctor !!!!1");
            MoneySpentCommnand = new Command(MoneySpent);
        }

        private void MoneySpent()
        {
            System.Diagnostics.Debug.WriteLine("Hey, we inside WelcomeViewmodel.MoneySpent AND SOMETHIGN CHANGED #77");
            NavigationService.Instance.Request(new PushCategoriesPage());
        }


        public ICommand MoneySpentCommnand { get; private set; }
    }
}
