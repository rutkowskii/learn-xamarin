using learn_xamarin.Model;
using learn_xamarin.Vm;
using Ninject;

namespace Tests.Utils
{
    public class InsertExpenditureAction
    {
        private readonly Category _category;
        private readonly decimal _sum;

        public InsertExpenditureAction(Category category, decimal sum)
        {
            _category = category;
            _sum = sum;
        }

        public void Run(TestingContext tc)
        {
            tc.Kernel.Get<MoneySpentDialogViewModel>().CategorySelected = _category;
            tc.Kernel.Get<MoneySpentDialogViewModel>().Sum = _sum;
            tc.Kernel.Get<MoneySpentSumViewModel>().ConfirmationCommand.Execute(null);
        }
    }
}
