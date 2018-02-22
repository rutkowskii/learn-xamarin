using System;
using learn_xamarin.Model;

namespace learn_xamarin.DataServices
{
    public interface IExpendituresDataService
    {
        void Add(Expenditure expenditure);
        void TrySynchronize();
    }
}