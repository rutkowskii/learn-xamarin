using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using learn_xamarin.Cache;
using learn_xamarin.DataServices;
using learn_xamarin.Model;

namespace learn_xamarin.Vm
{
    public class StatementViewModel : ObservableObject
    {
        private readonly IExpendituresDataService _expendituresDataService;
        private ObservableCollection<Expenditure> _statementElements;
        

        public StatementViewModel(IExpendituresCache expendituresCache)
        {
            StatementElements = new ObservableCollection<Expenditure>();
            expendituresCache.All().Foreach(Insert);
            expendituresCache.CollectionChanged += OnCollectionChanged;
        }

        public ObservableCollection<Expenditure> StatementElements
        {
            get { return _statementElements; }
            set
            {
                _statementElements = value;
                OnPropertyChanged(nameof(StatementElements));
            }
        }
        
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            notifyCollectionChangedEventArgs.NewItems?
                .Cast<Expenditure>()
                .Foreach(Insert);
        }

        private void Insert(Expenditure newElement)
        {
            var index = 0;
            foreach (var existingElement in _statementElements)
            {
                if (newElement.Timestamp < existingElement.Timestamp)
                {
                    index++;
                }
                else
                {
                    _statementElements.Insert(index, newElement);
                    return;
                }
            }
            _statementElements.Add(newElement); // meaning we went through the whole collection
        }
    }
}