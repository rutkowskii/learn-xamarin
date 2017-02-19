using System;
using learn_xamarin.Model;

namespace learn_xamarin.Services
{
    public interface IExpendituresDataService
    {
        void Add(Expenditure expenditure);
        void TrySynchronize(Action<Expenditure[]> callback);
    }
}