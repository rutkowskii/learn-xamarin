using System;
using System.Collections.ObjectModel;
using learn_xamarin.Model;
using learn_xamarin.Navigation;
using System.Linq;
using learn_xamarin.DataServices;

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
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get
            {
                LoadCategoriesIfNeeded();
                return _categories;
            }
            private set { _categories = value; }
        }

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

        private void LoadCategoriesIfNeeded()
        {
            if (_categories != null) return;
            _categories = new ObservableCollection<Category>();
            _categoriesDataService.GetAll(categories => categories.Foreach(_categories.Add));
        }
    }
}