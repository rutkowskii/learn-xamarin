using System.Linq;
using System.Threading.Tasks;
using learn_xamarin.Model;
using learn_xamarin.Vm;
using Moq;
using Ninject;
using NUnit.Framework;
using Tests.Utils;
using Xamarin.Forms;

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
            new InsertExpenditureAction().Run(_tc);
            _settingsViewModel = _tc.Kernel.Get<SettingsViewModel>();
        }
        
        [Test]
        public void Main_currency_is_updated_from_ui()
        {
            SetupDialog(SettingsViewModel.MainCurrencyDialogTitle, Currency.Eur.Code);

            _settingsViewModel
                .SettingsCollection
                .Single(x => x.Label.StartsWith("Main currency"))
                .Cmd
                .Execute(null);

            var actualSettingsState = _settingsViewModel.SettingsCollection.Select(x => x.Label).ToArray();
            CollectionAssert.Contains(actualSettingsState, "Main currency: EUR");
            
            var actualSpentOverall = _tc.Kernel.Get<WelcomeViewModel>().SpentOverall;
            Assert.That(actualSpentOverall.Contains("EUR"));
        }

        [Test]
        public void Current_currency_is_updated_from_ui() 
        {
            SetupDialog(SettingsViewModel.CurrentCurrencyDialogTitle, Currency.Gbp.Code);

            _settingsViewModel
                .SettingsCollection
                .Single(x => x.Label.StartsWith("Current currency"))
                .Cmd
                .Execute(null);
            
            var actualSettingsState = _settingsViewModel.SettingsCollection.Select(x => x.Label).ToArray();
            CollectionAssert.Contains(actualSettingsState, "Current currency: GBP");

            Assert.AreEqual(Currency.Gbp.Code, _tc.Kernel.Get<MoneySpentSumViewModel>().CurrentCurrencyCode);
        }

        private void SetupDialog(string dialogTitle, string dialogResult)
        {
            _tc.DialogService.Setup(s =>
                    s.DisplayActionSheet(It.IsAny<Page>(), dialogTitle, It.IsAny<string[]>()))
                .Returns(Task.FromResult(dialogResult));
        }
    }
}