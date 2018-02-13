using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using learn_xamarin.Cache;
using learn_xamarin.DataServices;
using learn_xamarin.Model;
using learn_xamarin.Navigation;
using learn_xamarin.Utils;
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

        public WelcomeViewModel(
            INavigationService navigationService,
            IExpendituresCache expendituresCache)
        {
            _navigationService = navigationService;
            _expendituresCache = expendituresCache;
            _expendituresCache.CollectionChanged += OnCacheUpdated;
            UpdateSummariesFromCache();
            MoneySpentCommnand = new Command(MoneySpent);
        }

        public ICommand MoneySpentCommnand { get; private set; }
        
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
        
        private void OnCacheUpdated(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            UpdateSummariesFromCache();
        }

        private void UpdateSummariesFromCache()
        {
            SpentThisWeek = _expendituresCache.SumThisWeek > 0 ? $"Spent {_expendituresCache.SumThisWeek:0.##} this week" : string.Empty;
            SpentThisMonth = _expendituresCache.SumThisMonth > 0 ?  $"Spent {_expendituresCache.SumThisMonth:0.##} this month" : string.Empty;
            SpentOverall = _expendituresCache.Sum > 0 ? $"Spent {_expendituresCache.Sum:0.##} overall" : string.Empty;
        }
    }
}
