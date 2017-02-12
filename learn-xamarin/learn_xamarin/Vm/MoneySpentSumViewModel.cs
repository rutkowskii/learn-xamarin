using System;
using System.Windows.Input;
using learn_xamarin.Model;
using learn_xamarin.Navigation;
using learn_xamarin.Services;
using Xamarin.Forms;

namespace learn_xamarin.Vm
{
    public class MoneySpentSumViewModel
    {
        private readonly IExpendituresDataService _expendituresDataService;
        private readonly MoneySpentDialogViewModel _moneySpentDialogViewModel;
        private readonly INavigationService _navigationService;
        private string _sum;

        public MoneySpentSumViewModel(IExpendituresDataService expendituresDataService,
            MoneySpentDialogViewModel moneySpentDialogViewModel,
            INavigationService navigationService)
        {
            _expendituresDataService = expendituresDataService;
            _moneySpentDialogViewModel = moneySpentDialogViewModel;
            _navigationService = navigationService;
            ConfirmationCommand = new Command(ConfirmSum);
        }

        private void ConfirmSum()
        {
            _expendituresDataService.Add(new Expenditure
            {
                CategoryId = _moneySpentDialogViewModel.CategorySelected.Id,
                Id = Guid.NewGuid(),
                Sum = _moneySpentDialogViewModel.Sum.Value,
                Timestamp = DateTime.Now
            });
            _moneySpentDialogViewModel.Clean();
            _navigationService.Request(new BackToWelcomePage());
        }

        public ICommand ConfirmationCommand { get; private set; }

        public string Sum
        {
            get { return _sum; }
            set
            {
                _sum = value;
                decimal parseResult = default(decimal);
                if (decimal.TryParse(_sum, out parseResult))
                {
                    _moneySpentDialogViewModel.Sum = parseResult;
                }
            }
        }
    }
}