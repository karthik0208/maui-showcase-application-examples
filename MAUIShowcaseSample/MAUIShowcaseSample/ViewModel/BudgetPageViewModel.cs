using CommunityToolkit.Mvvm.Input;
using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Toolkit.SegmentedControl;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Transaction = MAUIShowcaseSample.Services.Transaction;

namespace MAUIShowcaseSample
{
    /// <summary>
    /// ViewModel for managing budget page operations including budget tracking, categorization, and financial analysis
    /// </summary>
    public partial class BudgetPageViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Segment titles for budget filtering
        /// </summary>
        private List<SfSegmentItem> segmentTitle;

        /// <summary>
        /// Currently selected segment index
        /// </summary>
        private int selectedSegmentIndex;

        /// <summary>
        /// Data store service for data operations
        /// </summary>
        private readonly DataStore _dataStore;

        /// <summary>
        /// Selected segment data collection
        /// </summary>
        private ObservableCollection<Budget> selectedSegmentData = new ObservableCollection<Budget>();

        /// <summary>
        /// Summarized budget data collection
        /// </summary>
        private ObservableCollection<SummarizedBudgetData> budgetData = new();

        /// <summary>
        /// Transaction chart data collection
        /// </summary>
        private ObservableCollection<TransactionChartData> transactionChartData;

        /// <summary>
        /// Currency symbol for the current user
        /// </summary>
        private string currencySymbol;

        /// <summary>
        /// Indicates if the page is enabled for interaction
        /// </summary>
        private bool isPageEnabled = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the segment titles for budget filtering
        /// </summary>
        /// <value>List of SfSegmentItem objects representing filter options</value>
        public List<SfSegmentItem> SegmentTitle
        {
            get
            {
                return this.segmentTitle;
            }
            set
            {
                this.segmentTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected segment index
        /// </summary>
        /// <value>Integer representing the currently selected segment</value>
        public int SelectedSegmentIndex
        {
            get
            {
                return this.selectedSegmentIndex;
            }
            set
            {
                this.selectedSegmentIndex = value;
                OnPropertyChanged(nameof(SelectedSegmentIndex));
            }
        }

        /// <summary>
        /// Gets or sets the selected segment data
        /// </summary>
        /// <value>Observable collection of Budget objects for the selected segment</value>
        public ObservableCollection<Budget> SelectedSegmentData
        {
            get
            {
                return this.selectedSegmentData;
            }
            set
            {
                this.selectedSegmentData = value;
                OnPropertyChanged(nameof(SelectedSegmentData));
            }
        }

        /// <summary>
        /// Gets or sets the summarized budget data
        /// </summary>
        /// <value>Observable collection of SummarizedBudgetData objects</value>
        public ObservableCollection<SummarizedBudgetData> BudgetData
        {
            get
            {
                return this.budgetData;
            }
            set
            {
                this.budgetData = value;
                OnPropertyChanged(nameof(BudgetData));
            }
        }

        /// <summary>
        /// Gets or sets the chart data for visualization
        /// </summary>
        /// <value>Observable collection of TransactionChartData objects</value>
        public ObservableCollection<TransactionChartData> ChartData
        {
            get
            {
                return this.transactionChartData;
            }
            set
            {
                this.transactionChartData = value;
                OnPropertyChanged(nameof(ChartData));
            }
        }

        /// <summary>
        /// Gets or sets whether the page is enabled for interaction
        /// </summary>
        /// <value>Boolean indicating if the page is enabled</value>
        public bool IsPageEnabled
        {
            get => isPageEnabled;
            set
            {
                if (isPageEnabled != value)
                {
                    isPageEnabled = value;
                    OnPropertyChanged(nameof(IsPageEnabled));
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the BudgetPageViewModel class
        /// </summary>
        /// <param name="userDataService">User data service for user operations</param>
        /// <param name="dataStore">Data store service for data operations</param>
        public BudgetPageViewModel(UserDataService userDataService, DataStore dataStore)
        {
            _dataStore = dataStore;
            
            // Initialize segment titles for budget filtering
            SegmentTitle = new List<SfSegmentItem>()
            {
                new SfSegmentItem() {Text = "Active Budget"},
                new SfSegmentItem() {Text = "Completed Budget"}
            };
            
            SelectedSegmentIndex = 0;
            currencySymbol = userDataService.GetUserCurrencySymbol(userDataService.LoggedInAccount);
            
            // Load initial data for the selected segment
            SelectedSegmentData = dataStore.GetBudgetList(SegmentTitle[SelectedSegmentIndex].Text);
            BudgetData = GetSummarizedBudgetData(selectedSegmentData);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates budget data based on selected segment
        /// </summary>
        /// <param name="selectedSegment">Optional segment name to filter by</param>
        public void UpdateBudgetData(string? selectedSegment = null)
        {
            if(selectedSegment != null)
            {
                SelectedSegmentData = _dataStore.GetBudgetList(selectedSegment);
            }
            else
            {
                SelectedSegmentData = _dataStore.GetBudgetList(SegmentTitle[SelectedSegmentIndex].Text);
            }
            BudgetData = GetSummarizedBudgetData(SelectedSegmentData);
        }

        /// <summary>
        /// Attempts to update budget data with error handling
        /// </summary>
        /// <param name="selectedSegment">Optional segment name to filter by</param>
        /// <returns>True if operation is successful, false otherwise</returns>
        public bool TryUpdateBudgetData(string? selectedSegment = null)
        {
            try
            {
                if(selectedSegment != null)
                {
                    SelectedSegmentData = _dataStore.GetBudgetList(selectedSegment);
                }
                else
                {
                    SelectedSegmentData = _dataStore.GetBudgetList(SegmentTitle[SelectedSegmentIndex].Text);
                }
                BudgetData = GetSummarizedBudgetData(SelectedSegmentData);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates chart data based on transaction type
        /// </summary>
        /// <param name="transactionType">Type of transaction to filter by</param>
        public void UpdateChartData(string transactionType)
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();
            ChartData = GetChartData(dailyTransaction, transactionType);
        }

        /// <summary>
        /// Attempts to update chart data with error handling
        /// </summary>
        /// <param name="transactionType">Type of transaction to filter by</param>
        /// <returns>True if operation is successful, false otherwise</returns>
        public bool TryUpdateChartData(string transactionType)
        {
            try
            {
                var dailyTransaction = _dataStore.GetDailyTransactions();
                ChartData = GetChartData(dailyTransaction, transactionType);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Opens or closes popup for the specified budget
        /// </summary>
        /// <param name="budgetId">ID of the budget to toggle popup for</param>
        public void OpenPopup(double budgetId)
        {
            foreach(var data in BudgetData)
            {
                if(data.BudgetId == budgetId)
                {
                    // Toggle popup state for the selected budget
                    if (data.IsPopupOpen == true)
                    {
                        data.IsPopupOpen = false;
                    }                        
                    else
                    {
                        data.IsPopupOpen = true;
                    }                       
                }
                else
                {
                    // Close popup for all other budgets
                    data.IsPopupOpen = false;
                }
            }
        }

        /// <summary>
        /// Attempts to open or close popup with error handling
        /// </summary>
        /// <param name="budgetId">ID of the budget to toggle popup for</param>
        /// <returns>True if operation is successful, false otherwise</returns>
        public bool TryOpenPopup(double budgetId)
        {
            try
            {
                foreach(var data in BudgetData)
                {
                    if(data.BudgetId == budgetId)
                    {
                        // Toggle popup state for the selected budget
                        if (data.IsPopupOpen == true)
                        {
                            data.IsPopupOpen = false;
                        }                        
                        else
                        {
                            data.IsPopupOpen = true;
                        }                       
                    }
                    else
                    {
                        // Close popup for all other budgets
                        data.IsPopupOpen = false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes the budget with open popup and shows confirmation dialog
        /// </summary>
        public async void DeleteBudget()
        {
            // Get IDs of budgets with open popups
            List<double> transactionIds = BudgetData.Where(t => t.IsPopupOpen == true).Select(t => t.BudgetId).ToList();
            
            if (_dataStore.DeleteTransactions(transactionIds, "Budget"))
            {
                UpdateBudgetData();
                await Application.Current.MainPage.DisplayAlert("Success", "Transaction deleted successfully", "Okay");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "Deleting transaction failed", "Okay");
            }
        }

        /// <summary>
        /// Attempts to delete budget with error handling
        /// </summary>
        /// <returns>True if operation is successful, false otherwise</returns>
        public async Task<bool> TryDeleteBudget()
        {
            try
            {
                // Get IDs of budgets with open popups
                List<double> transactionIds = BudgetData.Where(t => t.IsPopupOpen == true).Select(t => t.BudgetId).ToList();
                
                if (_dataStore.DeleteTransactions(transactionIds, "Budget"))
                {
                    UpdateBudgetData();
                    await Application.Current.MainPage.DisplayAlert("Success", "Transaction deleted successfully", "Okay");
                    return true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Deleting transaction failed", "Okay");
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Gets summarized budget data from the provided budget collection
        /// </summary>
        /// <param name="filterData">Collection of Budget objects to summarize</param>
        /// <returns>Observable collection of SummarizedBudgetData objects</returns>
        private ObservableCollection<SummarizedBudgetData> GetSummarizedBudgetData(ObservableCollection<Budget> filterData)
        {
            var summarizedData = new ObservableCollection<SummarizedBudgetData>();        

            foreach(var data in filterData)
            {
                var categoryIcon = GetBudgetCategoryIcon(data.BudgetCategory.ToString());
                Color categoryColor = GetBudgetCategoryColor(data.BudgetCategory.ToString());
                double amountSpent = GetAmountSpent(_dataStore, data);
                double remainingAmount = data.BudgetAmount - amountSpent;
                double utilizedPercent = Math.Round(((amountSpent / data.BudgetAmount) * 100), 1, MidpointRounding.ToZero);
                
                summarizedData.Add(new SummarizedBudgetData() 
                {
                    BudgetId = data.BudgetId, 
                    BudgetTitle = data.BudgetTitle, 
                    BudgetCategory = data.BudgetCategory, 
                    BudgetDate = data.BudgetDate, 
                    BudgetAmount = data.BudgetAmount, 
                    CurrencySymbol = this.currencySymbol, 
                    Icon = categoryIcon, 
                    CategoryColor = categoryColor, 
                    AmountSpent = amountSpent, 
                    RemainingAmount = remainingAmount, 
                    Utilization = utilizedPercent 
                });
            }
            return summarizedData;
        }

        /// <summary>
        /// Gets the appropriate icon for the budget category
        /// </summary>
        /// <param name="category">Budget category name</param>
        /// <returns>String representing the icon code</returns>
        private string GetBudgetCategoryIcon(string category)
        {
            string iconCode = "\ue73d"; // Default icon

            switch (category)
            {
                case "Monthly Budget":
                    iconCode = "\ue723";
                    break;

                case "Vacation Budget":
                    iconCode = "\ue73b";
                    break;

                case "Transport Budget":
                    iconCode = "\ue740";
                    break;
            }
            return iconCode;
        }

        /// <summary>
        /// Gets the appropriate color for the budget category
        /// </summary>
        /// <param name="category">Budget category name</param>
        /// <returns>Color object for the category</returns>
        private Color GetBudgetCategoryColor(string category)
        {
            Color color = Color.FromArgb("#EC5C7B"); // Default color

            switch (category)
            {
                case "Monthly Budget":
                    if (Application.Current.Resources.TryGetValue("SeriesColor1", out var series1))
                    {
                        color = (Color)series1;
                    }
                    break;

                case "Vacation Budget":
                    if (Application.Current.Resources.TryGetValue("SeriesColor2", out var series2))
                    {
                        color = (Color)series2;
                    }
                    break;

                case "Transport Budget":
                    if (Application.Current.Resources.TryGetValue("SeriesColor3", out var series3))
                    {
                        color = (Color)series3;
                    }
                    break;
            }
            return color;
        }

        /// <summary>
        /// Calculates the amount spent for a specific budget based on its category
        /// </summary>
        /// <param name="dataStore">Data store instance for retrieving transactions</param>
        /// <param name="budgetData">Budget data to calculate spending for</param>
        /// <returns>Double representing the total amount spent</returns>
        private double GetAmountSpent(DataStore dataStore, Budget budgetData)
        {
            double amountSpent = 0;
            ObservableCollection<Transaction> filteredData = new ObservableCollection<Transaction>();
            
            switch(budgetData.BudgetCategory)
            {
                case "Monthly Budget":
                    // Get transactions for the current month
                    filteredData = dataStore.GetDailyTransactions(
                        new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), 
                        new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1));
                    amountSpent = filteredData.Where(t => t.TransactionType == "Expense")
                                            .Sum(t => double.TryParse(t.TransactionAmount, out double amount) ? amount : 0);
                    break;

                case "Transport Budget":
                    // Get all transactions and filter by transport categories
                    filteredData = dataStore.GetDailyTransactions();
                    List<string> transportExpenseCategory = BudgetCategories.CategoryItems.GetValueOrDefault("Transport Budget");
                    amountSpent = filteredData.Where(t => t.TransactionType == "Expense" && 
                                                        transportExpenseCategory.Contains(item: t.TransactionCategory))
                                            .Sum(t => double.TryParse(t.TransactionAmount, out double amount) ? amount : 0);
                    break;

                case "Vacation Budget":
                    // Get all transactions and filter by vacation categories
                    filteredData = dataStore.GetDailyTransactions();
                    List<string> vacationExpenseCategory = BudgetCategories.CategoryItems.GetValueOrDefault("Vacation Budget");
                    amountSpent = filteredData.Where(t => t.TransactionType == "Expense" && 
                                                        vacationExpenseCategory.Contains(item: t.TransactionCategory))
                                            .Sum(t => double.TryParse(t.TransactionAmount, out double amount) ? amount : 0);
                    break;
            }
            return amountSpent;
        }

        /// <summary>
        /// Gets chart data for transactions (currently returns empty collection)
        /// </summary>
        /// <param name="transactions">Collection of transactions to process</param>
        /// <param name="transactionType">Type of transaction to filter by</param>
        /// <returns>Observable collection of TransactionChartData objects</returns>
        private ObservableCollection<TransactionChartData> GetChartData(ObservableCollection<Transaction> transactions, string transactionType)
        {
            var _chartData = new ObservableCollection<TransactionChartData>();
            // Chart data processing logic would be implemented here
            return _chartData;
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

    #region Supporting Classes

    /// <summary>
    /// Represents summarized budget data for display and interaction
    /// </summary>
    public class SummarizedBudgetData : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Unique identifier for the budget
        /// </summary>
        private double budgetId;

        /// <summary>
        /// Title of the budget
        /// </summary>
        private string? budgetTitle;

        /// <summary>
        /// Category of the budget
        /// </summary>
        private string? budgetCategory;

        /// <summary>
        /// Budget amount allocated
        /// </summary>
        private double? budgetAmount;

        /// <summary>
        /// Date when the budget was created
        /// </summary>
        private DateTime budgetDate;

        /// <summary>
        /// Additional remarks for the budget
        /// </summary>
        private string? budgetRemarks;

        /// <summary>
        /// Indicates if the budget is completed
        /// </summary>
        private bool isCompleted;

        /// <summary>
        /// Currency symbol for display
        /// </summary>
        private string? currencySymbol;

        /// <summary>
        /// Amount spent from the budget
        /// </summary>
        private double amountSpent;

        /// <summary>
        /// Remaining amount in the budget
        /// </summary>
        private double remainingAmount;

        /// <summary>
        /// Percentage of budget utilized
        /// </summary>
        private double utilization;

        /// <summary>
        /// Icon representing the budget category
        /// </summary>
        private string? icon;

        /// <summary>
        /// Color associated with the budget category
        /// </summary>
        private Color? categoryColor;

        /// <summary>
        /// Indicates if the popup is open for this budget
        /// </summary>
        private bool isPopupOpen;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the budget ID
        /// </summary>
        /// <value>Double representing the unique budget identifier</value>
        public double BudgetId
        { 
            get
            {
                return this.budgetId;
            }
            set
            {
                this.budgetId = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the popup is open
        /// </summary>
        /// <value>Boolean indicating if the popup is open</value>
        public bool IsPopupOpen
        {
            get
            {
                return this.isPopupOpen;
            }
            set
            {
                this.isPopupOpen = value;
                OnPropertyChanged(nameof(IsPopupOpen));
            }
        }

        /// <summary>
        /// Gets or sets the budget title
        /// </summary>
        /// <value>String representing the budget title</value>
        public string? BudgetTitle
        {
            get
            {
                return this.budgetTitle;
            }
            set
            {
                this.budgetTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the budget category
        /// </summary>
        /// <value>String representing the budget category</value>
        public string? BudgetCategory
        {
            get
            {
                return this.budgetCategory;
            }
            set
            {
                this.budgetCategory = value;
            }
        }

        /// <summary>
        /// Gets or sets the budget amount
        /// </summary>
        /// <value>Double representing the allocated budget amount</value>
        public double? BudgetAmount
        {
            get
            {
                return this.budgetAmount;
            }
            set
            {
                this.budgetAmount = value;
            }
        }

        /// <summary>
        /// Gets or sets the budget date
        /// </summary>
        /// <value>DateTime representing when the budget was created</value>
        public DateTime BudgetDate
        {
            get
            {
                return this.budgetDate;
            }
            set
            {
                this.budgetDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the budget remarks
        /// </summary>
        /// <value>String containing additional budget information</value>
        public string? Remarks
        {
            get
            {
                return this.budgetRemarks;
            }
            set
            {
                this.budgetRemarks = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the budget is completed
        /// </summary>
        /// <value>Boolean indicating if the budget is completed</value>
        public bool IsCompleted
        {
            get
            {
                return this.isCompleted;
            }
            set
            {
                this.isCompleted = value;
            }
        }

        /// <summary>
        /// Gets or sets the currency symbol
        /// </summary>
        /// <value>String representing the currency symbol</value>
        public string? CurrencySymbol
        {
            get
            {
                return this.currencySymbol;
            }
            set
            {
                this.currencySymbol = value;
            }
        }

        /// <summary>
        /// Gets or sets the category icon
        /// </summary>
        /// <value>String representing the icon code</value>
        public string? Icon
        {
            get
            {
                return this.icon;
            }
            set
            {
                this.icon = value;
            }
        }

        /// <summary>
        /// Gets or sets the category color
        /// </summary>
        /// <value>Color object for the budget category</value>
        public Color? CategoryColor
        {
            get
            {
                return this.categoryColor;
            }
            set
            {
                this.categoryColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount spent
        /// </summary>
        /// <value>Double representing the amount spent from the budget</value>
        public double AmountSpent
        {
            get
            {
                return this.amountSpent;
            }
            set
            {
                this.amountSpent = value;
            }
        }

        /// <summary>
        /// Gets or sets the remaining amount
        /// </summary>
        /// <value>Double representing the remaining budget amount</value>
        public double RemainingAmount
        {
            get
            {
                return this.remainingAmount;
            }
            set
            {
                this.remainingAmount = value;
            }
        }

        /// <summary>
        /// Gets or sets the utilization percentage
        /// </summary>
        /// <value>Double representing the percentage of budget utilized</value>
        public double Utilization
        {
            get
            {
                return this.utilization;
            }
            set
            {
                this.utilization = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SummarizedBudgetData class
        /// </summary>
        public SummarizedBudgetData()
        {
            IsPopupOpen = false;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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

    /// <summary>
    /// Static class containing budget category mappings
    /// </summary>
    public class BudgetCategories
    {
        /// <summary>
        /// Dictionary mapping budget categories to their associated transaction categories
        /// </summary>
        /// <value>Dictionary with budget category names as keys and lists of transaction categories as values</value>
        public static readonly Dictionary<string, List<string>> CategoryItems = new()
        {
            { "Transport Budget", new List<string> { "Transportation" } },
            { "Vacation Budget", new List<string> { "Shopping" } }
        };
    }

    #endregion
}