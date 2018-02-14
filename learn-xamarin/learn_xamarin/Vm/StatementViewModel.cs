using System;
using System.Collections.ObjectModel;
using learn_xamarin.Cache;
using learn_xamarin.Model;

namespace learn_xamarin.Vm
{
    public class StatementViewModel : ObservableObject
    {
        private ObservableCollection<Expenditure> _statementElements;

        public StatementViewModel(IExpendituresCache expendituresCache)
        {
            StatementElements = new ObservableCollection<Expenditure>();
            expendituresCache.ElementAdded.Subscribe(Insert);
        }

        public ObservableCollection<Expenditure> StatementElements
        {
            get => _statementElements;
            private set
            {
                _statementElements = value;
                OnPropertyChanged(nameof(StatementElements));
            }
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