using System.Collections.ObjectModel;
using System.Linq;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using Xamarin.Forms;

namespace learn_xamarin.Vm
{
    public class SettingsViewModel
    {
        private const string CancelToken = "Cancel";
        private readonly ISettingsRepo _settingsRepo;
        private Currency _currentCurrency;
        private Currency _mainCurrency;
        private readonly SettingsItem _mainCurrencyMenuItem;
        private readonly SettingsItem _currentCurrencyMenuItem;

        public SettingsViewModel(ISettingsRepo settingsRepo)
        {
            _settingsRepo = settingsRepo;
            _mainCurrencyMenuItem = new SettingsItem
            {
                Cmd = new Command<Page>(EditMainCurrency)
            };
            _currentCurrencyMenuItem = new SettingsItem
            {
                Cmd = new Command<Page>(EditCurrentCurrency)
            };
            SettingsCollection = new ObservableCollection<SettingsItem>
            {
                _mainCurrencyMenuItem,
                _currentCurrencyMenuItem
            };
            MainCurrency = Currency.FromCodeString(_settingsRepo.Get(SettingsKeys.MainCurrency));
            CurrentCurrency = Currency.FromCodeString(_settingsRepo.Get(SettingsKeys.CurrentCurrency));
        }

        public ObservableCollection<SettingsItem> SettingsCollection { get; }

        private Currency MainCurrency
        {
            get { return _mainCurrency; }
            set
            {
                if (_mainCurrency != value && _mainCurrency != null) // do not update settings repo if initializing 
                    _settingsRepo.Set(SettingsKeys.MainCurrency, value.Code);
                _mainCurrency = value;
                _mainCurrencyMenuItem.Label = $"Main currency: {MainCurrency.Code}";
            }
        }

        private Currency CurrentCurrency
        {
            get { return _currentCurrency; }
            set
            {
                if (_currentCurrency != value && _currentCurrency != null
                ) // do not update settings repo if initializing
                    _settingsRepo.Set(SettingsKeys.CurrentCurrency, value.Code);
                _currentCurrency = value;
                _currentCurrencyMenuItem.Label = $"Current currency: {CurrentCurrency.Code}";
            }
        }

        private async void EditMainCurrency(Page page)
        {
            // todo piotr wrap the page inside sth to make this code testable
            var codeChosen =
                await page.DisplayActionSheet("Choose the main currency", CancelToken, null, AllCurrencyCodes);
            if (codeChosen == CancelToken)
                return;
            MainCurrency = Currency.FromCodeString(codeChosen);
        }

        private string[] AllCurrencyCodes => Currency.All.Select(c => c.Code).OrderBy(c => c).ToArray();

        private async void EditCurrentCurrency(Page page)
        {
            var codeChosen =
                await page.DisplayActionSheet("Choose current currency", CancelToken, null, AllCurrencyCodes);
            if (codeChosen == CancelToken)
                return;
            CurrentCurrency = Currency.FromCodeString(codeChosen);
        }

        public class SettingsItem : ObservableObject
        {
            private string _label;

            public string Label
            {
                get { return _label; }
                set
                {
                    _label = value;
                    OnPropertyChanged(nameof(Label));
                }
            }

            public Command<Page> Cmd { get; set; }
        }
    }
}