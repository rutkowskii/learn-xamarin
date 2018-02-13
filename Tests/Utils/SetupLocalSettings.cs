using learn_xamarin.Storage;

namespace Tests.Utils
{
    class SetupLocalSettings : ITestSetup
    {
        public string MainCurrency { get; }
        public string CurrentCurrency { get; }

        public SetupLocalSettings(string mainCurrency = "PLN", string currentCurrency = "GBP")
        {
            MainCurrency = mainCurrency;
            CurrentCurrency = currentCurrency;
        }
        
        public void Setup(TestingContext testingContext)
        {
            testingContext.SettingsRepo.Setup(r => r.Get(SettingsKeys.CurrentCurrency)).Returns(CurrentCurrency);
            testingContext.SettingsRepo.Setup(r => r.Get(SettingsKeys.MainCurrency)).Returns(MainCurrency);
        }
    }
}
