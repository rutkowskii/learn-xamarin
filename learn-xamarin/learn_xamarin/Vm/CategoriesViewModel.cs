using System.Collections.ObjectModel;
using learn_xamarin.Model;
using learn_xamarin.Navigation;
using learn_xamarin.Services;

namespace learn_xamarin.Vm
{
    public class CategoriesViewModel
    {
        private readonly ICategoriesDataService _categoriesDataService;
        private readonly MoneySpentDialogViewModel _moneySpentDialogViewModel;
        private readonly INavigationService _navigationService;
        private Category _categorySelected;

        public CategoriesViewModel(ICategoriesDataService categoriesDataService,
            MoneySpentDialogViewModel moneySpentDialogViewModel, INavigationService navigationService)
        {
            _categoriesDataService = categoriesDataService;
            _moneySpentDialogViewModel = moneySpentDialogViewModel;
            _navigationService = navigationService;
            Categories = new ObservableCollection<Category>(_categoriesDataService.GetAll());
        }

        public ObservableCollection<Category> Categories { get; private set; }

        public Category CategorySelected
        {
            get { return _categorySelected; }
            set
            {
                if (value == null) return;
                _categorySelected = value;
                _moneySpentDialogViewModel.CategorySelected = value;
                _navigationService.Request(new PushMoneySpentSumPage());
            }
        }
    }
}