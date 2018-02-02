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
        private SetupLocalSettings _setupLocalSettings;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _tc = new TestingContext();
            _setupLocalSettings = new SetupLocalSettings();
            _tc.RunSetups(_setupLocalSettings);
            _fixture = new Fixture();
        }

        [Test]
        public void Adding_expenditures_updates_this_week_summary()
        {
            var sums = _fixture.CreateMany<decimal>();
            sums.Foreach(sum => new InsertExpenditureAction(_fixture.Create<Category>(), sum).Run(_tc));
            
            Assert.AreEqual($"Spent {sums.Sum()} this week", _tc.Kernel.Get<WelcomeViewModel>().SpentThisWeek);
            
        }
    }
}