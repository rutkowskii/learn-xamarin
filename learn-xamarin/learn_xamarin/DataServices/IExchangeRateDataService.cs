namespace learn_xamarin.DataServices
{
    public interface IExchangeRateDataService
    {
        decimal? Get(string sourceCode, string targetCode);
    }

    public class ExchangeRateDataService : IExchangeRateDataService
    {
        public decimal? Get(string sourceCode, string targetCode)
        {
            throw new System.NotImplementedException();
        }
    }
}