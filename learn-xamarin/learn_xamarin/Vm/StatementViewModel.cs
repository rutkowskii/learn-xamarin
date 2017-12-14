﻿using System.Collections.ObjectModel;
using System.Linq;
using learn_xamarin.DataServices;
using learn_xamarin.Model;

namespace learn_xamarin.Vm
{
    public class StatementViewModel : ObservableObject
    {
        private readonly IExpendituresDataService _expendituresDataService;
        private ObservableCollection<Expenditure> _statementElements;

        public StatementViewModel(IExpendituresDataService expendituresDataService)
        {
            _expendituresDataService = expendituresDataService;
            StatementElements = new ObservableCollection<Expenditure>();
        }

        public ObservableCollection<Expenditure> StatementElements
        {
            get
            {
                _expendituresDataService.GetAll(OnReceivingExpenditures);
                return _statementElements;
            }
            set
            {
                _statementElements = value;
                OnPropertyChanged(nameof(StatementElements));
            }
        }

        private void OnReceivingExpenditures(Expenditure[] expenditures)
        {
            _statementElements.Clear();
            expenditures.OrderByDescending(e => e.Timestamp).Foreach(_statementElements.Add);
        }
    }
}