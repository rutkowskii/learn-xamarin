using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using learn_xamarin.Model;
using learn_xamarin.Navigation;
using learn_xamarin.Services;
using learn_xamarin.Utils;
using Xamarin.Forms;

namespace learn_xamarin.Vm
{
    public class WelcomeViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IExpendituresDataService _expendituresDataService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private string _spentThisWeek;
        private string _spentOverall;
        private string _spentThisMonth;
        private string _spentToday;

        public WelcomeViewModel(INavigationService navigationService, 
            IExpendituresDataService expendituresDataService,
            IDateTimeProvider dateTimeProvider)
        {
            _navigationService = navigationService;
            _expendituresDataService = expendituresDataService;
            _dateTimeProvider = dateTimeProvider;
            MoneySpentCommnand = new Command(MoneySpent);
        }

        private void MoneySpent()
        {
            _navigationService.Request(new PushCategoriesPage());
        }

        public ICommand MoneySpentCommnand { get; private set; }

        public string SpentOverall
        {
            get { return _spentOverall; }
            set
            {
                _spentOverall = value;
                OnPropertyChanged(nameof(SpentOverall));
            }
        }

        public string SpentThisMonth
        {
            get { return _spentThisMonth; }
            set
            {
                _spentThisMonth = value;
                OnPropertyChanged(nameof(SpentThisMonth));
            }
        }

        public string SpentThisWeek
        {
            get { return _spentThisWeek; }
            set
            {
                _spentThisWeek = value;
                OnPropertyChanged(nameof(SpentThisWeek));
            }
        }

        public string SpentToday
        {
            get { return _spentToday; }
            set
            {
                _spentToday = value;
                OnPropertyChanged(nameof(SpentToday));
            }
        }

        public void RefreshSummaryInfos()
        {
            _expendituresDataService.GetAll(Callback);
        }

        private void Callback(Expenditure[] allExpenditures)
        {
            var now = _dateTimeProvider.Now;

            var sumOverall = allExpenditures.Sum(e => e.Sum);
            var sumThisMonth = allExpenditures
                .Where(e => e.Timestamp >= MonthStart(now))
                .Sum(e => e.Sum);
            var sumToday = allExpenditures
                .Where(e => e.Timestamp >= DateTime.Today && e.Timestamp < DateTime.Today.AddDays(1))
                .Sum(e => e.Sum);
            var sumThisWeek = allExpenditures
                .Where(e => e.Timestamp > WeekStart(now))
                .Sum(e => e.Sum);

            SpentToday = $"Spent {sumToday} today";
            SpentThisWeek = $"Spent {sumThisWeek} this week";
            SpentThisMonth = $"Spent {sumThisMonth} this month";
            SpentOverall = $"Spent {sumOverall} overall";
        }

        public DateTime MonthStart(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public DateTime WeekStart(DateTime dt)
        {
            for (var i = dt;; i = i.AddDays(-1))
            {
                if (i.DayOfWeek == DayOfWeek.Monday)
                {
                    return i.Date;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}