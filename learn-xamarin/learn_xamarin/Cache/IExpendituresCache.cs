using learn_xamarin.Cache.Base;
using learn_xamarin.Model;

namespace learn_xamarin.Cache
{
    /*
     * MAIN currency should always be used here 
     */
    
    public interface IExpendituresCache :  ICache<Expenditure>
    {
        decimal Sum { get; }
        decimal SumThisWeek { get; }
        decimal SumThisMonth { get; }
    }
}