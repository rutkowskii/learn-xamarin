using System.Collections.ObjectModel;
using System.Linq;
using learn_xamarin.AppSettings;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using learn_xamarin.UiUtils;
using Xamarin.Forms;

namespace learn_xamarin.Vm
{
    public class SettingsViewModel
    {
        private string[] AllCurrencyCodes => Currency.All.Select(c => c.Code).OrderBy(c => c).ToArray();
        private const string CancelToken = "Cancel";
        public const string MainCurrencyDialogTitle = "Choose the main currency";
        public const string CurrentCurrencyDialogTitle = "Choose current currency";
        private readonly ISettingsRepo _settingsRepo;
        private readonly IDialogService _dialogService;
        private Currency _currentCurrency;
        private Currency _mainCurrency;
        private readonly SettingsItem _mainCurrencyMenuItem;
        private readonly SettingsItem _currentCurrencyMenuItem;

        public SettingsViewModel(ISettingsRepo settingsRepo, IDialogService dialogService)
        {
            _settingsRepo = settingsRepo;
            _dialogService = dialogService;
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
            MainCurrency = Currency.FromCodeString(_settingsRepo.MainCurrency);
            CurrentCurrency = Currency.FromCodeString(_settingsRepo.CurrentCurrency);
        }

        public ObservableCollection<SettingsItem> SettingsCollection { get; }

        private Currency MainCurrency
        {
            get => _mainCurrency;
            set
            {
                if (_mainCurrency != value && _mainCurrency != null) // do not update settings repo if initializing 
                {
                    _settingsRepo.MainCurrency = value.Code;
                }
                _mainCurrency = value;
                _mainCurrencyMenuItem.Label = $"Main currency: {MainCurrency.Code}";
            }
        }

        private Currency CurrentCurrency
        {
            get => _currentCurrency;
            set
            {
                if (_currentCurrency != value && _currentCurrency != null) // do not update settings repo if initializing
                {
                    _settingsRepo.CurrentCurrency = value.Code;
                }
                _currentCurrency = value;
                _currentCurrencyMenuItem.Label = $"Current currency: {CurrentCurrency.Code}";
            }
        }

        private async void EditMainCurrency(Page page)
        {
            var codeChosen = await _dialogService.DisplayActionSheet(page, MainCurrencyDialogTitle, AllCurrencyCodes);
            if (codeChosen != CancelToken)
            {
                MainCurrency = Currency.FromCodeString(codeChosen);
            }            
        }

        private async void EditCurrentCurrency(Page page)
        {
            var codeChosen = await _dialogService.DisplayActionSheet(page, CurrentCurrencyDialogTitle, AllCurrencyCodes);
            if (codeChosen != CancelToken)
            {
                CurrentCurrency = Currency.FromCodeString(codeChosen);
            }
        }

        public class SettingsItem : ObservableObject
        {
            private string _label;

            public string Label
            {
                get => _label;
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
