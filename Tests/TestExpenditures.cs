using System.Linq;
using learn_xamarin.Model;
using learn_xamarin.Vm;
using Ninject;
using Ploeh.AutoFixture;
using Xbehave;
using Xunit;

namespace Tests
{
    public class TestExpenditures
    {
        private readonly Fixture _fixture;

        public TestExpenditures()
        {
            _fixture = new Fixture();
        }
        
        [Scenario]
        public void Expenditures_are_added_to_the_local_db_when_server_is_down()
        {
            using (var tc = new TestingContext())
            {
                var category = _fixture.Create<Category>();
                var sum = _fixture.Create<decimal>();
            
                "GIVEN server is down".x(() =>
                {
                    new SetupUnavailableServer().Setup(tc);
                    new SetupLocalSettings().Setup(tc);
                });

                "WHEN expenditure created".x(() =>
                {
                    tc.Kernel.Get<MoneySpentDialogViewModel>().CategorySelected = category;
                    tc.Kernel.Get<MoneySpentDialogViewModel>().Sum = sum;
                    tc.Kernel.Get<MoneySpentSumViewModel>().ConfirmationCommand.Execute(null);
                });
            
                "THEN it becomes visible in the Statement".x(() =>
                {
                    var actual = tc.Kernel.Get<StatementViewModel>().StatementElements.First();
                    Assert.Equal(actual.CategoryId, category.Id);
                    Assert.Equal(actual.Sum, sum);
                });
            }
        }
    }
}