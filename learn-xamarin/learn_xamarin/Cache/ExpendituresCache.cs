using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using learn_xamarin.AppSettings;
using learn_xamarin.Cache.Base;
using learn_xamarin.DataServices;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.Utils;

namespace learn_xamarin.Cache
{
    public class ExpendituresCache : Cache<Expenditure>, IExpendituresCache
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ISettingsRepo _settingsRepo;
        private readonly IExchangeRateDataService _exchangeRateDataService;
        private readonly HashSet<Guid> _ids;

        public ExpendituresCache(IDateTimeProvider dateTimeProvider, ISettingsRepo settingsRepo, IExchangeRateDataService exchangeRateDataService)
        {
            _dateTimeProvider = dateTimeProvider;
            _settingsRepo = settingsRepo;
            _exchangeRateDataService = exchangeRateDataService;
            _ids = new HashSet<Guid>();
            Sum = 0;
        }
        
        public decimal Sum { get; private set; }
        public decimal SumThisWeek { get; private set; }
        public decimal SumThisMonth { get; private set; }
        
        public bool IsStored(Guid id)
        {
            return _ids.Contains(id);
        }

        protected override void OnItemAdded(Expenditure exp)
        {
            _ids.Add(exp.Id);
            var effectiveSum = SumInMainCurrency(exp);
            Sum += effectiveSum;
            if (exp.Timestamp > WeekStart(_dateTimeProvider.Now))
            {
                SumThisWeek += effectiveSum;
            }
            if (exp.Timestamp > MonthStart(_dateTimeProvider.Now))
            {
                SumThisMonth += effectiveSum;
            }
        }

        private decimal SumInMainCurrency(Expenditure exp)
        {
            if (exp.CurrencyCode == _settingsRepo.MainCurrency)
            {
                return exp.Sum;
            }
            // todo piotr more convenient way to access settings 
            var exchangeRateRatio = _exchangeRateDataService.Get(exp.CurrencyCode, _settingsRepo.MainCurrency);
            return exchangeRateRatio * exp.Sum ?? 0;
        }
        
        private DateTime MonthStart(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        private DateTime WeekStart(DateTime dt)
        {
            for (var i = dt;; i = i.AddDays(-1))
            {
                if (i.DayOfWeek == DayOfWeek.Monday) return i.Date;
            }
        }
    }
}
