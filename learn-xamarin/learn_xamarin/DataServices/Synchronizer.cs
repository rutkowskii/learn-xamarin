using System;
using System.Reactive.Linq;

namespace learn_xamarin.DataServices
{
    public class Synchronizer
    {
        private readonly IExpendituresDataService _expendituresDataService;

        public Synchronizer(IExpendituresDataService expendituresDataService)
        {
            _expendituresDataService = expendituresDataService;
        }
        
        public void Synchronize()
        {
            _expendituresDataService.TrySynchronize();
        }
    }
}