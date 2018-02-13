using System.Linq;
using learn_xamarin.Model;
using learn_xamarin.Vm;
using Ninject;
using NUnit.Framework;
using Tests.Utils;

namespace Tests
{
    [TestFixture]
    public class TestSettings
    {
        private TestingContext _tc;
        private SettingsViewModel _settingsViewModel;

        [SetUp]
        public void Setup()
        {
            _tc = new TestingContext();
            _tc.RunSetups(new SetupLocalSettings(Currency.Pln.Code, Currency.Pln.Code));
            _settingsViewModel = _tc.Kernel.Get<SettingsViewModel>();
        }
        
        [Test]
        public void Main_currency_is_updated_from_ui() 
        {
            Assert.Inconclusive();
        }
        
        [Test]
        public void Current_currency_is_updated_from_ui() 
        {
            Assert.Inconclusive();
        }

    }
}