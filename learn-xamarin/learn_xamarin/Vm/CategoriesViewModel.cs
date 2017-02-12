using System;
using System.Collections.ObjectModel;
using learn_xamarin.Navigation;

namespace learn_xamarin.Vm
{
    public class CategoriesViewModel
    {
        private readonly CategoriesDataService _categoriesDataService;
        private readonly MoneySpentDialogViewModel _moneySpentDialogViewModel;
        private readonly INavigationService _navigationService;
        private Category _categorySelected;

        public CategoriesViewModel(CategoriesDataService categoriesDataService,
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

    public class CategoriesDataService
    {
        public Category[] GetAll()
        {
            return new[]
            {
                new Category {Id = new Guid("B1EE74CD-A750-4352-967B-C7A443782634"), Name = "Tesco"},
                new Category {Id = new Guid("1733106D-36E9-491B-BBA1-85723225EC13"), Name = "Oyster"},
                new Category {Id = new Guid("28415902-893D-4E06-91D7-85751E60EB11"), Name = "Chata"},
                new Category {Id = new Guid("1F474620-1A95-43CB-97B1-2B64241F356F"), Name = "Piwo"},
                new Category {Id = new Guid("1F474620-1A95-43CB-97B1-2B64241F356F"), Name = "Lancz"},
            };
        }
    }


    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class MoneySpentDialogViewModel
    {
        public Category CategorySelected { get; set; }
        public decimal? Sum { get; set; }

        public void Clean()
        {
            // todo.
        }
    }
}