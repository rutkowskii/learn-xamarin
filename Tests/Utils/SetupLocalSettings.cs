using learn_xamarin.Storage;

namespace Tests.Utils
{
    class SetupLocalSettings : ITestSetup
    {
        public string Main { get; }
        public string Current { get; }

        public SetupLocalSettings(string main = "PLN", string current = "GBP")
        {
            Main = main;
            Current = current;
        }
        
        public void Setup(TestingContext testingContext)
        {
            testingContext.SettingsRepo.Setup(r => r.Get(SettingsKeys.CurrentCurrency)).Returns(Current);
            testingContext.SettingsRepo.Setup(r => r.Get(SettingsKeys.MainCurrency)).Returns(Main);
        }
    }
}