using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Toolkit.SegmentedControl;
using Syncfusion.Maui.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    /// <summary>
    /// ViewModel for managing dashboard page operations including chart data, transactions, and financial summaries
    /// </summary>
    public class DashboardPageViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Data store service for data operations
        /// </summary>
        private readonly DataStore _dataStore;

        /// <summary>
        /// User data service for user-related operations
        /// </summary>
        private readonly UserDataService _userService;

        /// <summary>
        /// Random number generator for color selection
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// List of legend color keys for chart visualization
        /// </summary>
        private List<string> legendColors;

        /// <summary>
        /// Segment items for main chart control
        /// </summary>
        private List<SfSegmentItem> segmentItems;

        /// <summary>
        /// Segment items for area chart control
        /// </summary>
        private List<SfSegmentItem> areaChartSegmentItems;

        /// <summary>
        /// Currently selected segment index
        /// </summary>
        private int selectedSegmentIndex;

        /// <summary>
        /// Currently selected area chart segment
        /// </summary>
        private int areaChartSelectedSegment;

        /// <summary>
        /// Selected chart data collection
        /// </summary>
        private ObservableCollection<DataStore> selectedChartData;

        /// <summary>
        /// Transaction chart data collection
        /// </summary>
        private ObservableCollection<TransactionChartData> transactionChartData;

        /// <summary>
        /// Date range options for main chart
        /// </summary>
        private List<ChartDateRange> dateRange;

        /// <summary>
        /// Date range options for area chart
        /// </summary>
        private List<ChartDateRange> areaChartDateRange;

        /// <summary>
        /// Date range options for savings chart
        /// </summary>
        private List<ChartDateRange> savingsChartDateRange;

        /// <summary>
        /// Currently selected chart date range
        /// </summary>
        private ChartDateRange selectedChartDateRange;

        /// <summary>
        /// Currently selected area chart date range
        /// </summary>
        private ChartDateRange selectedAreaChartDateRange;

        /// <summary>
        /// Currently selected savings chart date range
        /// </summary>
        private ChartDateRange selectedSavingsChartDateRange;

        /// <summary>
        /// Dashboard income area chart data
        /// </summary>
        private ObservableCollection<AreaChartData> dashboardIncomeAreaChart;

        /// <summary>
        /// Dashboard expense area chart data
        /// </summary>
        private ObservableCollection<AreaChartData> dashboardExpenseAreaChart;

        /// <summary>
        /// Savings area chart data
        /// </summary>
        private ObservableCollection<AreaChartData> savingsAreaChart;

        /// <summary>
        /// Summarized goal data collection
        /// </summary>
        private ObservableCollection<SummarizedGoalData> goalData;

        /// <summary>
        /// Currency symbol for the current user
        /// </summary>
        private string currencySymbol;

        /// <summary>
        /// Indicates if the page is enabled for interaction
        /// </summary>
        private bool isPageEnabled = false;

        /// <summary>
        /// Background color of the page
        /// </summary>
        private Color pageBackgroundColor = Colors.Gray;

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the legend colors for chart visualization
        /// </summary>
        /// <value>List of Brush objects representing legend colors</value>
        public List<Brush> LegendColors { get; set; } = new();

        /// <summary>
        /// Gets or sets the collection of transactions
        /// </summary>
        /// <value>Observable collection of Transaction objects</value>
        public ObservableCollection<Transaction> Transactions { get; set; } = new();

        /// <summary>
        /// Gets or sets the goals page view model
        /// </summary>
        /// <value>GoalsPageViewModel instance</value>
        public GoalsPageViewModel GoalsPageViewModel { get; set; }

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
                UpdateChartData(GetSegmentTitle(SelectedSegmentIndex));
                OnPropertyChanged("SelectedChartDateRange");
            }
        }

        /// <summary>
        /// Gets or sets the selected area chart date range
        /// </summary>
        /// <value>ChartDateRange object representing the selected area chart date range</value>
        public ChartDateRange SelectedAreaChartDateRange
        {
            get
            {
                return this.selectedAreaChartDateRange;
            }
            set
            {
                this.selectedAreaChartDateRange = value;
                UpdateAreaChartData(this.selectedAreaChartDateRange.RangeType, this.selectedAreaChartDateRange.StartDate, this.selectedAreaChartDateRange.EndDate);
                OnPropertyChanged("SelectedAreaChartDateRange");
            }
        }

        /// <summary>
        /// Gets or sets the selected savings chart date range
        /// </summary>
        /// <value>ChartDateRange object representing the selected savings chart date range</value>
        public ChartDateRange SelectedSavingsChartDateRange
        {
            get
            {
                return this.selectedSavingsChartDateRange;
            }
            set
            {
                this.selectedSavingsChartDateRange = value;
                UpdateSavingsAreaChartData(this.selectedSavingsChartDateRange.RangeType, this.selectedSavingsChartDateRange.StartDate, this.selectedSavingsChartDateRange.EndDate);
                OnPropertyChanged("SelectedSavingsChartDateRange");
            }
        }

        /// <summary>
        /// Gets or sets the selected chart data collection
        /// </summary>
        /// <value>Observable collection of DataStore objects</value>
        public ObservableCollection<DataStore> SelectedChartData
        {
            get
            {
                return this.selectedChartData;
            }
            set
            {
                this.selectedChartData = value;
            }
        }

        /// <summary>
        /// Gets or sets the chart data for transactions
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
        /// Gets or sets the goal data collection
        /// </summary>
        /// <value>Observable collection of SummarizedGoalData objects</value>
        public ObservableCollection<SummarizedGoalData> GoalData
        {
            get
            {
                return this.goalData;
            }
            set
            {
                this.goalData = value;
                OnPropertyChanged(nameof(GoalData));
            }
        }

        /// <summary>
        /// Gets or sets the total transaction summary
        /// </summary>
        /// <value>TransactionSummary object containing financial totals</value>
        public TransactionSummary TotalTransactionSummary { get; set; } = new();

        /// <summary>
        /// Gets or sets the segment titles for the main chart
        /// </summary>
        /// <value>List of SfSegmentItem objects</value>
        public List<SfSegmentItem> SegmentTitle 
        {
            get
            {
                return this.segmentItems;
            }
            set
            {
                this.segmentItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected segment index
        /// </summary>
        /// <value>Integer representing the selected segment index</value>
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
        /// Gets or sets the selected area chart segment
        /// </summary>
        /// <value>Integer representing the selected area chart segment</value>
        public int AreaChartSelectedSegment
        {
            get
            {
                return this.areaChartSelectedSegment;
            }
            set
            {
                this.areaChartSelectedSegment = value;
            }
        }

        /// <summary>
        /// Gets or sets the area chart segment titles
        /// </summary>
        /// <value>List of SfSegmentItem objects</value>
        public List<SfSegmentItem> AreaChartSegmentTitle
        {
            get
            {
                return this.segmentItems;
            }
            set
            {
                this.segmentItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the date range options for the main chart
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
        /// Gets or sets the date range options for the area chart
        /// </summary>
        /// <value>List of ChartDateRange objects</value>
        public List<ChartDateRange> AreaChartDateRange
        {
            get
            {
                return this.areaChartDateRange;
            }
            set
            {
                this.areaChartDateRange = value;
                OnPropertyChanged("AreaChartDateRange");
            }
        }

        /// <summary>
        /// Gets or sets the date range options for the savings chart
        /// </summary>
        /// <value>List of ChartDateRange objects</value>
        public List<ChartDateRange> SavingsChartDateRange
        {
            get
            {
                return this.savingsChartDateRange;
            }
            set
            {
                this.savingsChartDateRange = value;
                OnPropertyChanged("SavingsChartDateRange");
            }
        }

        /// <summary>
        /// Gets or sets the dashboard income area chart data
        /// </summary>
        /// <value>Observable collection of AreaChartData objects</value>
        public ObservableCollection<AreaChartData> DashboardIncomeAreaChart
        {
            get
            {
                return this.dashboardIncomeAreaChart;
            }
            set
            {
                this.dashboardIncomeAreaChart = value;
                OnPropertyChanged("DashboardIncomeAreaChart");
            }
        }

        /// <summary>
        /// Gets or sets the dashboard expense area chart data
        /// </summary>
        /// <value>Observable collection of AreaChartData objects</value>
        public ObservableCollection<AreaChartData> DashboardExpenseAreaChart
        {
            get
            {
                return this.dashboardExpenseAreaChart;
            }
            set
            {
                this.dashboardExpenseAreaChart = value;
                OnPropertyChanged("DashboardExpenseAreaChart");
            }
        }

        /// <summary>
        /// Gets or sets the savings area chart data
        /// </summary>
        /// <value>Observable collection of AreaChartData objects</value>
        public ObservableCollection<AreaChartData> SavingsAreaChart
        {
            get
            {
                return this.savingsAreaChart;
            }
            set
            {
                this.savingsAreaChart = value;
                OnPropertyChanged("SavingsAreaChart");
            }
        }

        /// <summary>
        /// Gets or sets the currency symbol for the current user
        /// </summary>
        /// <value>String representing the currency symbol</value>
        public string CurrencySymbol
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

        /// <summary>
        /// Gets or sets the background color of the page
        /// </summary>
        /// <value>Color object representing the page background color</value>
        public Color PageBackgroundColor
        {
            get => pageBackgroundColor;
            set
            {
                if (pageBackgroundColor != value)
                {
                    pageBackgroundColor = value;
                    OnPropertyChanged(nameof(PageBackgroundColor));
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DashboardPageViewModel class
        /// </summary>
        /// <param name="userService">User data service for user operations</param>
        /// <param name="dataStore">Data store service for data operations</param>
        public DashboardPageViewModel(UserDataService userService, DataStore dataStore)
        {
            _userService = userService;
            _dataStore = dataStore;
            CurrencySymbol = _userService.GetUserCurrencySymbol(userService.LoggedInAccount);

            // Initialize segment titles based on platform
#if WINDOWS
            SegmentTitle = new List<SfSegmentItem>()
            {
                new SfSegmentItem() { Text = "Income" },
                new SfSegmentItem() { Text = "Expense" }
            };
#elif ANDROID
            SegmentTitle = new List<SfSegmentItem>()
            {
                new SfSegmentItem() { Text = "\ue735" },
                new SfSegmentItem() { Text = "\ue736" }
            };
#else
            SegmentTitle = new List<SfSegmentItem>()
            {
                new SfSegmentItem() { Text = "Income" },
                new SfSegmentItem() { Text = "Expense" }
            };
#endif

            SelectedSegmentIndex = 0;

            // Initialize date ranges for main chart
            DateRange = new List<ChartDateRange>
            {
                new ChartDateRange() {RangeType = "This Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6)},
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)},                
                new ChartDateRange() {RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31)}
            };

            // Initialize date ranges for area chart
            AreaChartDateRange = new List<ChartDateRange>
            {
                new ChartDateRange() {RangeType = "This Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6)},
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)},
                new ChartDateRange() {RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31)}
            };

            // Initialize date ranges for savings chart
            SavingsChartDateRange = new List<ChartDateRange>
            {
                new ChartDateRange() { RangeType = "This Week",StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6) },
                new ChartDateRange() { RangeType = "Last Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday - 7), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday - 1) },
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1) },
                new ChartDateRange() { RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31) }
            };

            // Set default selected date ranges
            SelectedChartDateRange = DateRange.ElementAt(1);
            SelectedAreaChartDateRange = AreaChartDateRange.ElementAt(1);
            SelectedSavingsChartDateRange = SavingsChartDateRange.ElementAt(2);

            // Initialize goal data with active goals only
            var activeGoalData = dataStore.GetGoalsData().Where(t => t.IsCompleted == false).ToObservableCollection();
            GoalData = GetSummarizedGoalData(activeGoalData);

            // Update dashboard with initial data
            UpdateDashboardPage();
        }

        #endregion
        
        #region Private Helper Methods

        /// <summary>
        /// Gets the segment title based on the segment index
        /// </summary>
        /// <param name="segmentIndex">Index of the segment (0 for Income, 1 for Expense)</param>
        /// <returns>String representing the segment title</returns>
        private string GetSegmentTitle(int segmentIndex)
        {
            if (segmentIndex == 0)
                return "Income";
            else
                return "Expense";
        }

        /// <summary>
        /// Updates recent transactions with formatted amounts and currency symbols
        /// </summary>
        /// <param name="allTransaction">Collection of all transactions</param>
        /// <param name="limitCount">Optional limit for number of transactions to return</param>
        /// <returns>Observable collection of formatted Transaction objects</returns>
        private ObservableCollection<Transaction> UpdateRecentTransaction(ObservableCollection<Transaction> allTransaction, int? limitCount)
        {
            var _transactions = new ObservableCollection<Transaction>();

            // Apply limit if specified, otherwise take all transactions
            var selectedTransactions = limitCount != null? allTransaction.Take((int)limitCount) : allTransaction;            

            CurrencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            foreach (var transaction in selectedTransactions)
            {
                var amount = string.Empty;
                
                // Add appropriate prefix based on transaction type
                if (transaction.TransactionType == "Income")
                {
                    amount = "+";
                }
                else
                {
                    amount = "-";
                }
                amount = amount + CurrencySymbol + transaction.TransactionAmount;

                // Create a new Transaction object instead of modifying the existing one
                _transactions.Add(new Transaction
                {
                    TransactionName = transaction.TransactionName,
                    TransactionDate = transaction.TransactionDate,
                    TransactionDescription = transaction.TransactionDescription,
                    TransactionType = transaction.TransactionType,
                    TransactionAmount = amount
                });
            }
            return _transactions;
        }

        /// <summary>
        /// Gets transaction summary from the provided transaction collection
        /// </summary>
        /// <param name="transactions">Collection of Transaction objects</param>
        /// <returns>TransactionSummary object containing financial totals</returns>
        private TransactionSummary GetTransactionSummary(ObservableCollection<Transaction> transactions)
        {
            var CurrencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            var previousBalance = CurrencySymbol + _dataStore.GetPreviousBalance();
            var totalExpense =  transactions.Where(t => t.TransactionType == "Expense").Sum(t => double.Parse(t.TransactionAmount));
            var totalIncome = transactions.Where(t => t.TransactionType == "Income").Sum(t => double.Parse(t.TransactionAmount));
            var totalSavings = CurrencySymbol + (totalIncome - totalExpense);

            return new TransactionSummary() 
            { 
                CurrentBalance = previousBalance, 
                TotalExpense = CurrencySymbol + totalExpense, 
                TotalIncome = CurrencySymbol + totalIncome, 
                TotalSavings = totalSavings 
            };
        }

        /// <summary>
        /// Gets a random legend color from the available colors
        /// </summary>
        /// <returns>Brush object representing a random legend color</returns>
        private Brush GetRandomLegendColor()
        {    
            var randomKey = legendColors[_random.Next(legendColors.Count)];

            if (Application.Current.Resources.TryGetValue(randomKey, out var colorValue) && colorValue is Color color)
            {
                return (new SolidColorBrush(Color.FromRgb(color.Red,color.Green,color.Blue)));
            }
            return (new SolidColorBrush(Color.FromRgb(Colors.Transparent.Red,Colors.Transparent.Green, Colors.Transparent.Blue)));
        }

        /// <summary>
        /// Initializes the legend colors from resource dictionary
        /// </summary>
        private void InitializeLegendColors()
        {
            // Fetch colors from the resource dictionary
            legendColors = new List<string>
            {
                "LegendColor1", "LegendColor2", "LegendColor3", "LegendColor4", "LegendColor5", 
                "LegendColor6", "LegendColor7", "LegendColor8", "LegendColor9", "LegendColor10",
                "LegendColor11", "LegendColor12", "LegendColor13", "LegendColor14", "LegendColor15", 
                "LegendColor16", "LegendColor17", "LegendColor18", "LegendColor19", "LegendColor20"
            };
        }

        /// <summary>
        /// Gets summarized goal data from the provided goal collection
        /// </summary>
        /// <param name="filterData">Collection of Goal objects to summarize</param>
        /// <returns>Observable collection of SummarizedGoalData objects</returns>
        private ObservableCollection<SummarizedGoalData> GetSummarizedGoalData(ObservableCollection<Goal> filterData)
        {
            var summarizedData = new ObservableCollection<SummarizedGoalData>();
            var elementCount = 1;
            
            foreach (var data in filterData)
            {
                var categoryIcon = DataHelper.GetGoalIcon(data.GoalTitle);
                Color categoryColor = DataHelper.GetColorCode(elementCount);
                double remainingAmount = data.GoalAmount - data.ContributionAmount;
                double utilizedPercent = Math.Round(((data.ContributionAmount / data.GoalAmount) * 100), 1, MidpointRounding.ToZero);
                
                summarizedData.Add(new SummarizedGoalData() 
                { 
                    GoalId = data.GoalId, 
                    GoalTitle = data.GoalTitle, 
                    GoalPriority = data.GoalPriority, 
                    GoalDate = data.GoalDate, 
                    GoalAmount = data.GoalAmount, 
                    CurrencySymbol = this.CurrencySymbol, 
                    Icon = categoryIcon, 
                    IconColor = categoryColor, 
                    ContributionAmount = data.ContributionAmount, 
                    RemainingAmount = remainingAmount, 
                    Utilization = utilizedPercent, 
                    Remarks = data.Remarks, 
                    RemainingDays = (data.GoalDate - DateTime.Today).Days, 
                    Transactions = data.Transactions 
                });
                elementCount++;
            }
            return summarizedData;
        }

        /// <summary>
        /// Updates savings area chart data based on the specified date range
        /// </summary>
        /// <param name="rangeType">Type of date range (This Week, Last Week, This Month, This Year, Last 6 Months)</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        private void UpdateSavingsAreaChartData(string rangeType, DateTime? startDate, DateTime? endDate)
        {
            ObservableCollection<Savings> data = _dataStore.GetSavingsData();
            var filteredData = data.Where(s => s.SavingsDate >= startDate && s.SavingsDate <= endDate).ToList();

            switch (rangeType)
            {
                case "This Week":
                    // Initialize weekly savings dictionary with all days
                    Dictionary<string, double> weeklySavings = new Dictionary<string, double>
                    {
                        { "Sun", 0 }, { "Mon", 0 }, { "Tue", 0 }, { "Wed", 0 },
                        { "Thu", 0 }, { "Fri", 0 }, { "Sat", 0 }
                    };

                    // Aggregate savings by day of week
                    foreach (var savings in filteredData)
                    {
                        if (double.TryParse(savings.SavingsAmount, out double amount))
                        {
                            string dayKey = savings.SavingsDate.ToString("ddd");
                            weeklySavings[dayKey] += amount;
                        }
                    }

                    SavingsAreaChart = weeklySavings.Select(entry => new AreaChartData
                    {
                        TimeInterval = entry.Key,
                        Data = entry.Value
                    }).ToObservableCollection<AreaChartData>();
                    break;

                case "Last Week":
                    DateTime lastSunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 7);
                    DateTime lastSaturday = lastSunday.AddDays(6);

                    // Initialize last week savings dictionary
                    Dictionary<string, double> lastWeekSavings = new Dictionary<string, double>
                    {
                        { "Sun", 0 }, { "Mon", 0 }, { "Tue", 0 }, { "Wed", 0 },
                        { "Thu", 0 }, { "Fri", 0 }, { "Sat", 0 }
                    };

                    // Aggregate savings by day of last week
                    foreach (var savings in filteredData)
                    {
                        if (double.TryParse(savings.SavingsAmount, out double amount))
                        {
                            string dayKey = savings.SavingsDate.ToString("ddd");
                            lastWeekSavings[dayKey] += amount;
                        }
                    }

                    SavingsAreaChart = lastWeekSavings.Select(entry => new AreaChartData
                    {
                        TimeInterval = entry.Key,
                        Data = entry.Value
                    }).ToObservableCollection();
                    break;

                case "This Month":
                    // Initialize daily savings for the entire month
                    Dictionary<int, double> dailySavingsMonth = Enumerable.Range(1, DateTime.DaysInMonth(startDate.Value.Year, startDate.Value.Month))
                        .ToDictionary(day => day, day => 0.0);

                    // Aggregate savings by day of month
                    foreach (var savings in filteredData)
                    {
                        if (double.TryParse(savings.SavingsAmount, out double amount))
                        {
                            dailySavingsMonth[savings.SavingsDate.Day] += amount;
                        }
                    }

                    SavingsAreaChart = dailySavingsMonth.Select(entry => new AreaChartData
                    {
                        TimeInterval = entry.Key.ToString(),
                        Data = entry.Value
                    }).ToObservableCollection();
                    break;

                case "This Year":
                    // Initialize monthly savings for the entire year
                    Dictionary<string, double> monthlySavings = Enumerable.Range(1, 12)
                        .ToDictionary(month => CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(month), _ => 0.0);

                    // Aggregate savings by month
                    foreach (var savings in filteredData)
                    {
                        if (double.TryParse(savings.SavingsAmount, out double amount))
                        {
                            string monthKey = savings.SavingsDate.ToString("MMM");
                            monthlySavings[monthKey] += amount;
                        }
                    }

                    SavingsAreaChart = monthlySavings.Select(entry => new AreaChartData
                    {
                        TimeInterval = entry.Key,
                        Data = entry.Value
                    }).ToObservableCollection();
                    break;

                case "Last 6 Months":
                    DateTime sixMonthsAgo = DateTime.Today.AddMonths(-5);
                    
                    // Initialize last 6 months savings dictionary
                    Dictionary<string, double> lastSixMonthsSavings = Enumerable.Range(0, 6)
                        .Select(i => sixMonthsAgo.AddMonths(i).ToString("MMM"))
                        .ToDictionary(month => month, month => 0.0);

                    // Aggregate savings by month for last 6 months
                    foreach (var savings in filteredData)
                    {
                        if (double.TryParse(savings.SavingsAmount, out double amount))
                        {
                            string monthKey = savings.SavingsDate.ToString("MMM");
                            if (lastSixMonthsSavings.ContainsKey(monthKey))
                            {
                                lastSixMonthsSavings[monthKey] += amount;
                            }
                        }
                    }

                    SavingsAreaChart = lastSixMonthsSavings.Select(entry => new AreaChartData
                    {
                        TimeInterval = entry.Key,
                        Data = entry.Value
                    }).ToObservableCollection();
                    break;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the entire dashboard page with fresh data
        /// </summary>
        public void UpdateDashboardPage()
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();
            Transactions = UpdateRecentTransaction(dailyTransaction, 8);
            ChartData = GetChartData(dailyTransaction, GetSegmentTitle(SelectedSegmentIndex));
            UpdateAreaChartData(SelectedAreaChartDateRange.RangeType, SelectedChartDateRange.StartDate, SelectedChartDateRange.EndDate);
            UpdateSavingsAreaChartData(SelectedSavingsChartDateRange.RangeType, SelectedSavingsChartDateRange.StartDate, SelectedSavingsChartDateRange.EndDate);
            TotalTransactionSummary = GetTransactionSummary(dailyTransaction);
        }

        /// <summary>
        /// Gets chart data for transactions based on type and date range
        /// </summary>
        /// <param name="transactions">Collection of transactions to process</param>
        /// <param name="transactionType">Type of transaction (Income or Expense)</param>
        /// <returns>Observable collection of TransactionChartData objects</returns>
        public ObservableCollection<TransactionChartData> GetChartData(ObservableCollection<Transaction> transactions, string transactionType)
        {
            var CurrencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            
            // Filter transactions by type and date range
            var _transactions = transactions.Where(t => t.TransactionType == transactionType)
                                          .Where(t => t.TransactionDate >= SelectedChartDateRange.StartDate && 
                                                     t.TransactionDate <= SelectedChartDateRange.EndDate);
            var _chartData = new ObservableCollection<TransactionChartData>();

            var sumAmount = _transactions.Sum(t => double.Parse(t.TransactionAmount));
            var groupedTransaction = _transactions.GroupBy(t => t.TransactionName);
            
            // Initialize legend colors for chart visualization
            InitializeLegendColors();
            
            // Process each transaction group
            foreach (var _transaction in groupedTransaction)
            {
                var totalAmount = _transaction.Sum(t => double.Parse(t.TransactionAmount));
                var totalPercent = (totalAmount / sumAmount) * 100;
                Brush categoryColor = GetRandomLegendColor();
                LegendColors.Add(categoryColor);
                
                _chartData.Add(new TransactionChartData() 
                { 
                    TransactionCategory = _transaction.Key, 
                    TransactionAmount = CurrencySymbol + totalAmount.ToString(), 
                    TransactionPercent = totalPercent, 
                    CategoryColor = categoryColor 
                });
            }
            return _chartData;
        }

        /// <summary>
        /// Updates chart data based on transaction type
        /// </summary>
        /// <param name="transactionType">Type of transaction (Income or Expense)</param>
        public void UpdateChartData(string transactionType)
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();            
            ChartData = GetChartData(dailyTransaction, transactionType);
        }

        /// <summary>
        /// Updates area chart data for income and expense based on date range
        /// </summary>
        /// <param name="dateRangeType">Type of date range</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        public void UpdateAreaChartData(string dateRangeType, DateTime? startDate, DateTime? endDate)
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();
            
            // Separate income and expense transactions
            var incomeTransactions = (dailyTransaction.Where(t => t.TransactionType == "Income")).ToObservableCollection<Transaction>();
            var expenseTransactions = (dailyTransaction.Where(t => t.TransactionType == "Expense")).ToObservableCollection<Transaction>();
            
            // Generate area chart data for both income and expense
            DashboardIncomeAreaChart = DataHelper.GetAreaChartData(incomeTransactions, dateRangeType, startDate, endDate);
            DashboardExpenseAreaChart = DataHelper.GetAreaChartData(expenseTransactions, dateRangeType, startDate, endDate);
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
    /// Represents chart data for transaction visualization
    /// </summary>
    public class TransactionChartData
    {
        #region Private Fields

        /// <summary>
        /// Transaction category name
        /// </summary>
        private string transactionCategory;

        /// <summary>
        /// Formatted transaction amount with currency
        /// </summary>
        private string transactionAmount;

        /// <summary>
        /// Transaction percentage of total
        /// </summary>
        private double transactionPercent;

        /// <summary>
        /// Color brush for category visualization
        /// </summary>
        private Brush categoryColor;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the transaction category
        /// </summary>
        /// <value>String representing the transaction category</value>
        public string TransactionCategory
        {
            get 
            { 
                return this.transactionCategory; 
            }
            set
            {
                this.transactionCategory = value;
            }
        }

        /// <summary>
        /// Gets or sets the formatted transaction amount
        /// </summary>
        /// <value>String representing the transaction amount with currency symbol</value>
        public string TransactionAmount
        {
            get
            {
                return this.transactionAmount;
            }
            set
            {
                this.transactionAmount = value;
            }
        }

        /// <summary>
        /// Gets or sets the transaction percentage
        /// </summary>
        /// <value>Double representing the percentage of total transactions</value>
        public double TransactionPercent
        {
            get
            {
                return this.transactionPercent;
            }
            set
            {
                this.transactionPercent = value;
            }
        }

        /// <summary>
        /// Gets or sets the category color
        /// </summary>
        /// <value>Brush object representing the color for this category</value>
        public Brush CategoryColor
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

        #endregion
    }

    /// <summary>
    /// Represents a summary of financial transactions
    /// </summary>
    public class TransactionSummary
    {
        #region Private Fields

        /// <summary>
        /// Current account balance
        /// </summary>
        private string? currenBalance;

        /// <summary>
        /// Total income amount
        /// </summary>
        private string? totalIncome;

        /// <summary>
        /// Total expense amount
        /// </summary>
        private string? totalExpense;

        /// <summary>
        /// Total savings amount
        /// </summary>
        private string? totalSavings;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current balance
        /// </summary>
        /// <value>String representing the current account balance with currency</value>
        public string? CurrentBalance
        {
            get
            {
                return this.currenBalance;
            }
            set
            {
                this.currenBalance = value;
            }
        }

        /// <summary>
        /// Gets or sets the total income
        /// </summary>
        /// <value>String representing the total income with currency</value>
        public string? TotalIncome
        {
            get
            {
                return this.totalIncome;
            }
            set
            {
                this.totalIncome = value;
            }
        }

        /// <summary>
        /// Gets or sets the total expense
        /// </summary>
        /// <value>String representing the total expense with currency</value>
        public string? TotalExpense
        {
            get
            {
                return this.totalExpense;
            }
            set
            {
                this.totalExpense = value;
            }
        }

        /// <summary>
        /// Gets or sets the total savings
        /// </summary>
        /// <value>String representing the total savings with currency</value>
        public string? TotalSavings
        {
            get
            {
                return this.totalSavings;
            }
            set
            {
                this.totalSavings = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents a date range for chart filtering
    /// </summary>
    public class ChartDateRange : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Type of date range (This Week, This Month, etc.)
        /// </summary>
        private string? rangeType;

        /// <summary>
        /// Start date of the range
        /// </summary>
        private DateTime? startDate;

        /// <summary>
        /// End date of the range
        /// </summary>
        private DateTime? endDate;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the range type
        /// </summary>
        /// <value>String representing the type of date range</value>
        public string? RangeType
        {
            get
            {
                return this.rangeType;
            }
            set
            {
                this.rangeType = value;
                OnPropertyChanged("RangeType");
            }
        }

        /// <summary>
        /// Gets or sets the start date
        /// </summary>
        /// <value>DateTime representing the start of the date range</value>
        public DateTime? StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        /// <summary>
        /// Gets or sets the end date
        /// </summary>
        /// <value>DateTime representing the end of the date range</value>
        public DateTime? EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                this.endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

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

    #endregion
}