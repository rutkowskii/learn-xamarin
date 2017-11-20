using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using learn_xamarin.Model;
using learn_xamarin.Services;
using learn_xamarin.Storage;
using System;

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
        }

        public IEnumerable<CategorySummary> CategorySummaries
        {
            get
            {
                if(_categorySummaries == null) LoadCategories();
                return _categorySummaries;
            }
            set
            {
                _categorySummaries = value;
                OnPropertyChanged(nameof(CategorySummaries));
            }
        }

        private void LoadCategories()
        {
            _expendituresDataService.TrySynchronize(ExpendituresSynchronizationCallback);
        }

        private void ExpendituresSynchronizationCallback(Expenditure[] expenditures)
        {
            throw new NotImplementedException();

            //todo piotr tmp

            //var summaries = new List<CategorySummary>();
            //var loadCategoriesTask = _categoriesDataService.GetAll();
            //var categoriesByIds = loadCategoriesTask.ToDictionary(c => c.Id, c => c);
            //foreach (var group in expenditures.GroupBy(e => e.CategoryId))
            //{
            //    var summary = new CategorySummary
            //    {
            //        CategoryName = categoriesByIds[group.Key].Name,
            //        SumSpent = group.Sum(e => e.Sum)
            //    };
            //    summaries.Add(summary);
            //}
            //CategorySummaries = summaries;
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