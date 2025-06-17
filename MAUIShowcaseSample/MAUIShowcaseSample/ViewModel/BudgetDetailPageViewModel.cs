using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    /// <summary>
    /// ViewModel for managing budget detail page operations including transaction analysis, chart visualization, and grid data management
    /// </summary>
    public class BudgetDetailPageViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Selected budget data for detailed view
        /// </summary>
        private SummarizedBudgetData selectedBudgetData;

        /// <summary>
        /// Transaction chart data for pie/donut chart visualization
        /// </summary>
        private ObservableCollection<TransactionChartData> transactionChartData;

        /// <summary>
        /// Transaction area chart data for trend visualization
        /// </summary>
        private ObservableCollection<AreaChartData> transactionAreaChartData;

        /// <summary>
        /// Raw transaction data collection
        /// </summary>
        private ObservableCollection<Transaction> transactionData;

        /// <summary>
        /// Grid-formatted transaction data for display
        /// </summary>
        private ObservableCollection<TransactionGridData> transactionGridData;

        /// <summary>
        /// User data service for user-related operations
        /// </summary>
        private UserDataService _userService;

        /// <summary>
        /// Data store service for data operations
        /// </summary>
        private DataStore _dataStore;

        /// <summary>
        /// Indicates if all rows in the grid are selected
        /// </summary>
        private bool isAllRowsSelected;

        /// <summary>
        /// Available date range options for chart filtering
        /// </summary>
        private List<ChartDateRange> dateRange;

        /// <summary>
        /// Currently selected chart date range
        /// </summary>
        private ChartDateRange selectedChartDateRange;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the legend colors for chart visualization
        /// </summary>
        /// <value>List of Brush objects representing legend colors</value>
        public List<Brush> LegendColors { get; set; } = new();

        /// <summary>
        /// Gets or sets the selected budget data
        /// </summary>
        /// <value>SummarizedBudgetData object containing budget details</value>
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

        /// <summary>
        /// Gets or sets the transaction chart data for visualization
        /// </summary>
        /// <value>Observable collection of TransactionChartData objects</value>
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

        /// <summary>
        /// Gets or sets the transaction area chart data for trend analysis
        /// </summary>
        /// <value>Observable collection of AreaChartData objects</value>
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

        /// <summary>
        /// Gets or sets the raw transaction data
        /// </summary>
        /// <value>Observable collection of Transaction objects</value>
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

        /// <summary>
        /// Gets or sets the grid-formatted transaction data
        /// </summary>
        /// <value>Observable collection of TransactionGridData objects</value>
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

        /// <summary>
        /// Gets or sets whether all rows in the grid are selected
        /// </summary>
        /// <value>Boolean indicating if all rows are selected</value>
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

        /// <summary>
        /// Gets or sets the available date range options
        /// </summary>
        /// <value>List of ChartDateRange objects</value>
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

        /// <summary>
        /// Gets or sets the selected chart date range
        /// </summary>
        /// <value>ChartDateRange object representing the selected date range</value>
        public ChartDateRange SelectedChartDateRange
        {
            get
            {
                return this.selectedChartDateRange;
            }
            set
            {
                this.selectedChartDateRange = value;
                // Update area chart data when date range changes
                TransactionAreaChartData = DataHelper.GetAreaChartData(TransactionData, this.selectedChartDateRange.RangeType, this.selectedChartDateRange.StartDate, this.selectedChartDateRange.EndDate);
                OnPropertyChanged("SelectedChartDateRange");
            }
        }

        /// <summary>
        /// Gets or sets the currency symbol for display
        /// </summary>
        /// <value>String representing the currency symbol</value>
        public string CurrencySymbol { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the BudgetDetailPageViewModel class
        /// </summary>
        /// <param name="userDataService">User data service for user operations</param>
        /// <param name="dataStore">Data store service for data operations</param>
        /// <param name="budgetData">Selected budget data to display details for</param>
        public BudgetDetailPageViewModel(UserDataService userDataService, DataStore dataStore, SummarizedBudgetData budgetData)
        {
            _userService = userDataService;
            _dataStore = dataStore;
            SelectedBudgetData = budgetData;
            CurrencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            
            // Get transaction data based on budget category
            TransactionData = GetTransactionData(dataStore, budgetData);
            
            // Initialize date range options for chart filtering
            DateRange = new List<ChartDateRange>
            {
                new ChartDateRange() {RangeType = "This Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6)},
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)},
                new ChartDateRange() {RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31)}
            };
            
            // Set default selected date range to "This Month"
            SelectedChartDateRange = DateRange.ElementAt(1);
            
            // Initialize chart data and visualizations
            TransactionChartData = DataHelper.GetChartData(userDataService, transactionData, "Expense");
            LegendColors = DataHelper.LegendColors;
            TransactionAreaChartData = DataHelper.GetAreaChartData(transactionData, SelectedChartDateRange.RangeType, SelectedChartDateRange.StartDate, SelectedChartDateRange.EndDate);
            TransactionGridData = GetGridData(transactionData);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Gets transaction data filtered by budget category
        /// </summary>
        /// <param name="dataStore">Data store instance for retrieving transactions</param>
        /// <param name="budgetData">Budget data to filter transactions for</param>
        /// <returns>Observable collection of filtered Transaction objects</returns>
        private ObservableCollection<Transaction> GetTransactionData(DataStore dataStore, SummarizedBudgetData budgetData)
        {
            ObservableCollection<Transaction> filteredData = dataStore.GetDailyTransactions();
            List<string> expenseCategory = new List<string>();
            
            switch (budgetData.BudgetCategory)
            {
                case "Monthly Budget":
                    // Get transactions for the current month only
                    filteredData = dataStore.GetDailyTransactions(
                        new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), 
                        new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1));
                    break;

                case "Transport Budget":
                    // Get transport-related expense categories
                    expenseCategory = BudgetCategories.CategoryItems.GetValueOrDefault("Transport Budget");                    
                    break;

                case "Vacation Budget":
                    // Get vacation-related expense categories
                    expenseCategory = BudgetCategories.CategoryItems.GetValueOrDefault("Vacation Budget");                    
                    break;
            }
            
            // Filter transactions by expense type and category (if applicable)
            return filteredData.Where(t => t.TransactionType == "Expense" && 
                                          (expenseCategory.Count == 0 || expenseCategory.Contains(t.TransactionCategory)))
                                .ToObservableCollection<Transaction>();
        }

        /// <summary>
        /// Converts transaction data to grid-formatted data for display
        /// </summary>
        /// <param name="transactions">Collection of transactions to format</param>
        /// <returns>Observable collection of TransactionGridData objects</returns>
        private ObservableCollection<TransactionGridData> GetGridData(ObservableCollection<Transaction> transactions)
        {
            var currencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);           
            var _gridData = new ObservableCollection<TransactionGridData>();

            // Convert each transaction to grid format with currency symbol
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Selects or deselects all rows in the transaction grid
        /// </summary>
        /// <param name="value">True to select all rows, false to deselect all</param>
        public void SelectAllRowsInGrid(bool value)
        {
            var selectedRows = TransactionGridData;            

            // Apply selection state to all rows
            foreach (var row in selectedRows)
            {
                row.IsSelected = value;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the PropertyChanged event for the specified property
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}