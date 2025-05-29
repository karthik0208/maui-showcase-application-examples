using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    public class BudgetDetailPageViewModel : INotifyPropertyChanged
    {
        private SummarizedBudgetData selectedBudgetData;

        private ObservableCollection<TransactionChartData> transactionChartData;

        private ObservableCollection<AreaChartData> transactionAreaChartData;

        private ObservableCollection<Transaction> transactionData;

        private ObservableCollection<TransactionGridData> transactionGridData;

        private UserDataService _userService;

        private DataStore _dataStore;

        private bool isAllRowsSelected;

        private List<ChartDateRange> dateRange;

        private ChartDateRange selectedChartDateRange;

        public List<Brush> LegendColors { get; set; } = new();

        public SummarizedBudgetData SelectedBudgetData
        {
            get
            {
                return this.selectedBudgetData;
            }
            set
            {
                this.selectedBudgetData = value;
            }
        }

        public ObservableCollection<TransactionChartData> TransactionChartData
        {
            get
            {
                return this.transactionChartData;
            }
            set
            {
                this.transactionChartData = value;
            }
        }

        public ObservableCollection<AreaChartData> TransactionAreaChartData
        {
            get
            {
                return this.transactionAreaChartData;
            }
            set
            {
                this.transactionAreaChartData = value;
                OnPropertyChanged("TransactionAreaChartData");
            }
        }

        public ObservableCollection<Transaction> TransactionData
        {
            get
            {
                return this.transactionData;
            }
            set
            {
                this.transactionData = value;
            }
        }

        public ObservableCollection<TransactionGridData> TransactionGridData
        {
            get
            {
                return this.transactionGridData;
            }
            set
            {
                this.transactionGridData = value;
            }
        }

        public bool IsAllRowsSelected
        {
            get
            {
                return this.isAllRowsSelected;
            }
            set
            {
                this.isAllRowsSelected = value;
                SelectAllRowsInGrid(this.IsAllRowsSelected);
                OnPropertyChanged(nameof(IsAllRowsSelected));
            }
        }

        public List<ChartDateRange> DateRange
        {
            get
            {
                return this.dateRange;
            }
            set
            {
                this.dateRange = value;
                OnPropertyChanged("DateRange");
            }
        }

        public ChartDateRange SelectedChartDateRange
        {
            get
            {
                return this.selectedChartDateRange;
            }
            set
            {
                this.selectedChartDateRange = value;
                TransactionAreaChartData = DataHelper.GetAreaChartData(TransactionData, this.selectedChartDateRange.RangeType, this.selectedChartDateRange.StartDate, this.selectedChartDateRange.EndDate);
                OnPropertyChanged("SelectedChartDateRange");
            }
        }

        public string CurrencySymbol { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BudgetDetailPageViewModel(UserDataService userDataService, DataStore dataStore, SummarizedBudgetData budgetData)
        {
            _userService = userDataService;
            _dataStore = dataStore;
            SelectedBudgetData = budgetData;
            CurrencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            TransactionData = GetTransactionData(dataStore, budgetData);
            DateRange = new List<ChartDateRange>
            {
                new ChartDateRange() {RangeType = "This Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6)},
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)},
                new ChartDateRange() {RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31)}
            };
            SelectedChartDateRange = DateRange.ElementAt(1);            
            TransactionChartData = DataHelper.GetChartData(userDataService, transactionData, "Expense");
            LegendColors = DataHelper.LegendColors;
            TransactionAreaChartData = DataHelper.GetAreaChartData(transactionData, SelectedChartDateRange.RangeType, SelectedChartDateRange.StartDate, SelectedChartDateRange.EndDate);
            TransactionGridData = GetGridData(transactionData);

        }

        private ObservableCollection<Transaction> GetTransactionData(DataStore dataStore, SummarizedBudgetData budgetData)
        {
            ObservableCollection<Transaction> filteredData = dataStore.GetDailyTransactions();
            List<string> expenseCategory = new List<string>();
            switch (budgetData.BudgetCategory)
            {
                case "Monthly Budget":
                    filteredData = dataStore.GetDailyTransactions(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1));
                    break;

                case "Transport Budget":                    
                    expenseCategory = BudgetCategories.CategoryItems.GetValueOrDefault("Transport Budget");                    
                    break;

                case "Vacation Budget":
                    expenseCategory = BudgetCategories.CategoryItems.GetValueOrDefault("Vacation Budget");                    
                    break;
            }
            return filteredData.Where(t => t.TransactionType == "Expense" && (expenseCategory.Count == 0 || expenseCategory.Contains(t.TransactionCategory))).ToObservableCollection<Transaction>();
        }

        private ObservableCollection<TransactionGridData> GetGridData(ObservableCollection<Transaction> transactions)
        {
            var currencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);           
            var _gridData = new ObservableCollection<TransactionGridData>();

            foreach (var transaction in transactions)
            {
                _gridData.Add(new TransactionGridData()
                {
                    TransactionCategory = transaction.TransactionCategory,
                    TransactionAmount = transaction.TransactionAmount + currencySymbol,
                    TransactionDescription = transaction.TransactionDescription,
                    TransactionDate = transaction.TransactionDate,
                    TransactionType = transaction.TransactionType,
                    TransactionName = transaction.TransactionName
                });
            }
            return _gridData;
        }

        public void SelectAllRowsInGrid(bool value)
        {
            var selectedRows = TransactionGridData;            

            foreach (var row in selectedRows)
            {
                row.IsSelected = value;
            }
        }
    }
}
