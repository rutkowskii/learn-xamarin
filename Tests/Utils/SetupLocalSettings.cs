using learn_xamarin.AppSettings;
using Ninject;

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
            var repo = testingContext.Kernel.Get<ISettingsRepo>();
            repo.CurrentCurrency = CurrentCurrency;
            repo.MainCurrency = MainCurrency;
        }
    }
}
