using System;
using System.Windows.Input;
using learn_xamarin.Model;
using learn_xamarin.Navigation;
using learn_xamarin.Services;
using learn_xamarin.Storage;
using Xamarin.Forms;

namespace learn_xamarin.Vm
{
    public class MoneySpentSumViewModel
    {
        private readonly IExpendituresDataService _expendituresDataService;
        private readonly MoneySpentDialogViewModel _moneySpentDialogViewModel;
        private readonly INavigationService _navigationService;
        private readonly ISettingsRepo _settingsRepo;
        private string _sum;

        public MoneySpentSumViewModel(IExpendituresDataService expendituresDataService,
            MoneySpentDialogViewModel moneySpentDialogViewModel,
            INavigationService navigationService,
            ISettingsRepo settingsRepo)
        {
            _expendituresDataService = expendituresDataService;
            _moneySpentDialogViewModel = moneySpentDialogViewModel;
            _navigationService = navigationService;
            _settingsRepo = settingsRepo;
            ConfirmationCommand = new Command(ConfirmSum);
        }

        private void ConfirmSum()
        {
            _expendituresDataService.Add(new Expenditure
            {
                CategoryId = _moneySpentDialogViewModel.CategorySelected.Id,
                Id = Guid.NewGuid(),
                Sum = _moneySpentDialogViewModel.Sum.Value,
                Timestamp = DateTime.UtcNow,
                CurrencyCode = CurrentCurrencyCode
            });
            _moneySpentDialogViewModel.Clean();
            _navigationService.Request(new BackToWelcomePage());
        }

        public ICommand ConfirmationCommand { get; }
        public string CurrentCurrencyCode => _settingsRepo.Get(SettingsKeys.CurrentCurrency);

        public string Sum
        {
            get { return _sum; }
            set
            {
                _sum = value;
                decimal parseResult;
                if (decimal.TryParse(_sum, out parseResult))
                {
                    _moneySpentDialogViewModel.Sum = parseResult;
                }
            }
        }
    }
}