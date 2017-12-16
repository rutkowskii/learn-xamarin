using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
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
        private readonly IExpendituresDataService _expendituresDataService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private string _spentThisWeek;
        private string _spentOverall;
        private string _spentThisMonth;
        private string _spentToday;
        private string _lastExpenditures;
        private IExpendituresCache _expendituresCache;

        public WelcomeViewModel(INavigationService navigationService, 
            IExpendituresDataService expendituresDataService,
            IDateTimeProvider dateTimeProvider)
        {
            _navigationService = navigationService;
            _expendituresDataService = expendituresDataService;
            _expendituresCache = _expendituresDataService.GetCache();
            _expendituresCache.CollectionChanged += OnCacheUpdated;
            _dateTimeProvider = dateTimeProvider;
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
            SpentThisWeek = $"Spent {_expendituresCache.SumThisWeek} this week";
            SpentThisMonth = $"Spent {_expendituresCache.SumThisMonth} this month";
            SpentOverall = $"Spent {_expendituresCache.Sum} overall";
        }
      
    }
}