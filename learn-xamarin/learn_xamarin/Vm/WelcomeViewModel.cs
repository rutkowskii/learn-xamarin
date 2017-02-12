using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using learn_xamarin.Navigation;
using learn_xamarin.Services;
using Xamarin.Forms;

namespace learn_xamarin.Vm
{
    public class WelcomeViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IExpendituresDataService _expendituresDataService;
        private string _spentThisWeek;
        private string _spentToday;

        public WelcomeViewModel(INavigationService navigationService, IExpendituresDataService expendituresDataService)
        {
            _navigationService = navigationService;
            _expendituresDataService = expendituresDataService;
            MoneySpentCommnand = new Command(MoneySpent);
        }

        private void MoneySpent()
        {
            _navigationService.Request(new PushCategoriesPage());
        }

        public ICommand MoneySpentCommnand { get; private set; }

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
            var allExpenditures = _expendituresDataService.GetAll();
            var sumToday = allExpenditures
                .Where(e => e.Timestamp >= DateTime.Today && e.Timestamp < DateTime.Today.AddDays(1))
                .Sum(e => e.Sum);
            var sumThisWeek = allExpenditures //todo later use Monday for calculations 
                .Where(e => e.Timestamp > DateTime.Today.AddDays(-7))
                .Sum(e => e.Sum);

            SpentToday = $"Spent {sumToday} today";
            SpentThisWeek = $"Spent {sumThisWeek} this week";
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}