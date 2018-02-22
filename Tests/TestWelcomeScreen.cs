using System.Linq;
using learn_xamarin.Model;
using learn_xamarin.Vm;
using Ninject;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class TestWelcomeScreen
    {
        private TestingContext _tc;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _tc = new TestingContext();
            _tc.Run(new SetupLocalSettings(Currency.Pln.Code, Currency.Pln.Code));
            _fixture = new Fixture();
        }

        [Test]
        public void Adding_expenditures_updates_this_week_summary()
        {
            var sums = _fixture.CreateMany<decimal>();
            sums.Foreach(sum => new InsertExpenditureAction(_fixture.Create<Category>(), sum).Setup(_tc));

            AssertThisWeekSumLabel(sums.Sum());
        }

        [Test]
        public void Adding_expenditures_updates_summaries_using_currency_conversion()
        {
            /*
             * Spending 10 GBP (current currency) shown as 42.73 PLN (main currency)  
             */

            var gbpToPlnCode = 4.732m;
            var sum = 10.0m;
            
            _tc.Run(
                new SetupLocalSettings(mainCurrency: Currency.Pln.Code, currentCurrency: Currency.Gbp.Code), 
                new SetupExchangeRate(Currency.Gbp.Code, Currency.Pln.Code, gbpToPlnCode));

            new InsertExpenditureAction(_fixture.Create<Category>(), sum).Setup(_tc);
            
            AssertThisWeekSumLabel(gbpToPlnCode * sum);
        }

        private void AssertThisWeekSumLabel(decimal sum)
        {
            var actualLabel = _tc.Kernel.Get<WelcomeViewModel>().SpentThisWeek;
            Assert.AreEqual($"Spent {sum:0.##} PLN this week", actualLabel);
        }
    }
}
