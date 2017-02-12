using learn_xamarin.Model;

namespace learn_xamarin.Vm
{
    public class MoneySpentDialogViewModel
    {
        public Category CategorySelected { get; set; }
        public decimal? Sum { get; set; }

        public void Clean()
        {
            Sum = null;
            CategorySelected = null;
        }
    }
}