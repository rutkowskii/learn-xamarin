using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using learn_xamarin.Model;
using learn_xamarin.Storage;
using System;
using learn_xamarin.DataServices;

namespace learn_xamarin.Vm
{
    public class CategoriesSummaryViewModel : INotifyPropertyChanged
    {
        private readonly ICategoriesDataService _categoriesDataService;
        private readonly IExpendituresDataService _expendituresDataService;
        private IEnumerable<CategorySummary> _categorySummaries;

        public CategoriesSummaryViewModel(
            ICategoriesDataService categoriesDataService, 
            IExpendituresDataService expendituresDataService)
        {
            _categoriesDataService = categoriesDataService;
            _expendituresDataService = expendituresDataService;
            CategorySummaries = new CategorySummary[0];
        }

        public IEnumerable<CategorySummary> CategorySummaries
        {
            get
            {
             //   if(_categorySummaries == null) LoadCategories();
                return _categorySummaries;
            }
            set
            {
                _categorySummaries = value;
               // OnPropertyChanged(nameof(CategorySummaries));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CategorySummary
    {
        public string CategoryName { get; set; }
        public decimal SumSpent { get; set; }
    }
}