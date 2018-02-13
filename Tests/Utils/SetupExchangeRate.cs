namespace Tests.Utils
{
    public class SetupExchangeRate : ITestSetup
    {
        private readonly string _source;
        private readonly string _target;
        private readonly decimal _rate;

        public SetupExchangeRate(string source, string target, decimal rate)
        {
            _source = source;
            _target = target;
            _rate = rate;
        }
        
        public void Setup(TestingContext testingContext)
        {
            testingContext.ExchangeRateService.Setup(s => s.Get(_source, _target)).Returns(_rate);
        }
    }
}
