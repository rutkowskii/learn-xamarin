using System;
using learn_xamarin.Model;
using learn_xamarin.Vm;
using Ninject;
using Ploeh.AutoFixture;

namespace Tests.Utils
{
    public class InsertExpenditureAction : ITestSetup
    {
        private readonly Category _category;
        private readonly decimal _sum;

        public InsertExpenditureAction(Category category, decimal sum)
        {
            _category = category;
            _sum = sum;
        }

        public decimal Sum => _sum;
        public Guid CategoryId => _category.Id;

        public InsertExpenditureAction()
        {
            var fixture = new Fixture();
            _category = fixture.Create<Category>();
            _sum = fixture.Create<decimal>();
        }

        public void Setup(TestingContext tc)
        {
            tc.Kernel.Get<MoneySpentDialogViewModel>().CategorySelected = _category;
            tc.Kernel.Get<MoneySpentDialogViewModel>().Sum = _sum;
            tc.Kernel.Get<MoneySpentSumViewModel>().ConfirmationCommand.Execute(null);
        }
    }
}
