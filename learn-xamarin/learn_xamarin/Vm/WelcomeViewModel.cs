using System;
using System.Windows.Input;
using learn_xamarin.AppSettings;
using learn_xamarin.Cache;
using learn_xamarin.Navigation;
using Xamarin.Forms;

namespace learn_xamarin.Vm
{
    public class WelcomeViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private string _spentThisWeek;
        private string _spentOverall;
        private string _spentThisMonth;
        private string _spentToday;
        private string _lastExpenditures;
        private readonly IExpendituresCache _expendituresCache;
        private readonly ISettingsRepo _settingsRepo;

        public WelcomeViewModel(
            INavigationService navigationService,
            IExpendituresCache expendituresCache,
            ISettingsRepo settingsRepo)
        {
            _navigationService = navigationService;
            _expendituresCache = expendituresCache;
            _settingsRepo = settingsRepo;
            _expendituresCache.ElementAdded.Subscribe(_ => UpdateSummariesFromCache());
            _settingsRepo.SettingsUpdated.Subscribe(_ => UpdateSummariesFromCache());
            MoneySpentCommnand = new Command(MoneySpent);
        }

        public ICommand MoneySpentCommnand { get; }
        
        private void MoneySpent()
        {
            _navigationService.Request(new PushCategoriesPage());
        }

        public string LastExpenditures
        {
            get { return _lastExpenditures; }
            set
            {
                _lastExpenditures = value;
                OnPropertyChanged(nameof(LastExpenditures));
            }
        }

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

        public string SpentThisWeek // todo piotr show in ui. 
        {
            get { return _spentThisWeek; }
            set
            {
                _spentThisWeek = value;
                OnPropertyChanged(nameof(SpentThisWeek));
            }
        }

        private void UpdateSummariesFromCache()
        {
            SpentThisWeek = _expendituresCache.SumThisWeek > 0 ? $"Spent {_expendituresCache.SumThisWeek:0.##} {MainCurrency} this week" : string.Empty;
            SpentThisMonth = _expendituresCache.SumThisMonth > 0 ?  $"Spent {_expendituresCache.SumThisMonth:0.##} {MainCurrency} this month" : string.Empty;
            SpentOverall = _expendituresCache.Sum > 0 ? $"Spent {_expendituresCache.Sum:0.##} {MainCurrency} overall" : string.Empty;
        }

        private string MainCurrency => _settingsRepo.MainCurrency;
    }
}
