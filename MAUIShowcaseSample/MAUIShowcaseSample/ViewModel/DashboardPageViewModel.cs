using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Toolkit.SegmentedControl;
using Syncfusion.Maui.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    public class DashboardPageViewModel : INotifyPropertyChanged
    {
        private readonly DataStore _dataStore;

        private readonly UserDataService _userService;

        private readonly Random _random = new Random();

        private List<string> legendColors;

        private List<SfSegmentItem> segmentItems;

        private List<SfSegmentItem> areaChartSegmentItems;

        private int selectedSegmentIndex;

        private int areaChartSelectedSegment;

        private ObservableCollection<DataStore> selectedChartData;

        private ObservableCollection<TransactionChartData> transactionChartData;

        private List<ChartDateRange> dateRange;

        private List<ChartDateRange> areaChartDateRange;

        private List<ChartDateRange> savingsChartDateRange;

        private ChartDateRange selectedChartDateRange;

        private ChartDateRange selectedAreaChartDateRange;

        private ChartDateRange selectedSavingsChartDateRange;

        private ObservableCollection<AreaChartData> dashboardIncomeAreaChart;

        private ObservableCollection<AreaChartData> dashboardExpenseAreaChart;

        private ObservableCollection<AreaChartData> savingsAreaChart;

        private ObservableCollection<SummarizedGoalData> goalData;

        private string currencySymbol;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Brush> LegendColors { get; set; } = new();

        public ObservableCollection<Transaction> Transactions { get; set; } = new();

        public GoalsPageViewModel GoalsPageViewModel { get; set; }

        public ChartDateRange SelectedChartDateRange
        {
            get
            {
                return this.selectedChartDateRange;
            }
            set
            {
                this.selectedChartDateRange = value;
                UpdateChartData(SegmentTitle.ElementAt(SelectedSegmentIndex).Text);
                OnPropertyChanged("SelectedChartDateRange");
            }
        }

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

        public TransactionSummary TotalTransactionSummary { get; set; } = new();

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

        public DashboardPageViewModel(UserDataService userService, DataStore dataStore)
        {
            _userService = userService;
            _dataStore = dataStore;
            CurrencySymbol = _userService.GetUserCurrencySymbol(userService.LoggedInAccount);
            SegmentTitle = new List<SfSegmentItem>() 
            { 
                new SfSegmentItem(){ Text = "Income"},

                new SfSegmentItem() {Text = "Expense"}
            };
            SelectedSegmentIndex = 0;
            DateRange = new List<ChartDateRange>
            {
                new ChartDateRange() {RangeType = "This Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6)},
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)},                
                new ChartDateRange() {RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31)}
            };

            AreaChartDateRange = new List<ChartDateRange>
            {
                new ChartDateRange() {RangeType = "This Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6)},
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)},
                new ChartDateRange() {RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31)}
            };

            SavingsChartDateRange = new List<ChartDateRange>
            {
                new ChartDateRange() { RangeType = "This Week",StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6) },
                new ChartDateRange() { RangeType = "Last Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday - 7), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday - 1) },
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1) },
                new ChartDateRange() { RangeType = "Last 6 Months", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-5), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1) },
                new ChartDateRange() { RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31) }
            };

            SelectedChartDateRange = DateRange.ElementAt(1);
            SelectedAreaChartDateRange = AreaChartDateRange.ElementAt(1);
            SelectedSavingsChartDateRange = SavingsChartDateRange.ElementAt(3);
            var activeGoalData = dataStore.GetGoalsData().Where(t => t.IsCompleted == false).ToObservableCollection();
            GoalData = GetSummarizedGoalData(activeGoalData);
            UpdateDashboardPage();
            //var incomeTransactions = (dailyTransaction.Where(t => t.TransactionType == "Income")).ToObservableCollection<Transaction>();
            //var expenseTransactions = (dailyTransaction.Where(t => t.TransactionType == "Expense")).ToObservableCollection<Transaction>();
            //DashboardIncomeAreaChart = DataHelper.GetAreaChartData(incomeTransactions, SelectedAreaChartDateRange.RangeType, SelectedChartDateRange.StartDate, SelectedChartDateRange.EndDate);
            //DashboardExpenseAreaChart = DataHelper.GetAreaChartData(expenseTransactions, SelectedAreaChartDateRange.RangeType, SelectedChartDateRange.StartDate, SelectedChartDateRange.EndDate);
           
           // GoalsPageViewModel = new GoalsPageViewModel(_userService, _dataStore);
        }

        public void UpdateDashboardPage()
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();
            Transactions = UpdateRecentTransaction(dailyTransaction, 8);
            ChartData = GetChartData(dailyTransaction, SegmentTitle.ElementAt(SelectedSegmentIndex).Text);
            UpdateAreaChartData(SelectedAreaChartDateRange.RangeType, SelectedChartDateRange.StartDate, SelectedChartDateRange.EndDate);
            UpdateSavingsAreaChartData(SelectedSavingsChartDateRange.RangeType, SelectedSavingsChartDateRange.StartDate, SelectedSavingsChartDateRange.EndDate);
            TotalTransactionSummary = GetTransactionSummary(dailyTransaction);
        }

        private ObservableCollection<Transaction> UpdateRecentTransaction(ObservableCollection<Transaction> allTransaction, int? limitCount)
        {
            var _transactions = new ObservableCollection<Transaction>();

            var selectedTransactions = limitCount != null? allTransaction.Take((int)limitCount) : allTransaction;            

            CurrencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            foreach (var transaction in selectedTransactions)
            {
                var amount = string.Empty;
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
        
        public ObservableCollection<TransactionChartData> GetChartData(ObservableCollection<Transaction> transactions, string transactionType)
        {
            var CurrencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            var _transactions = transactions.Where(t => t.TransactionType == transactionType).Where(t => t.TransactionDate >= SelectedChartDateRange.StartDate && t.TransactionDate <= SelectedChartDateRange.EndDate);
            var _chartData = new ObservableCollection<TransactionChartData>();

            var sumAmount = _transactions.Sum(t => double.Parse(t.TransactionAmount));

            var groupedTransaction = _transactions.GroupBy(t => t.TransactionName);
            
            InitializeLegendColors(); // Initializing legend colors 
            
            foreach (var _transaction in groupedTransaction)
            {
                var totalAmount = _transaction.Sum(t => double.Parse(t.TransactionAmount));
                var totalPercent = (totalAmount / sumAmount) * 100;
                Brush categoryColor = GetRandomLegendColor();
                LegendColors.Add(categoryColor);
                _chartData.Add(new TransactionChartData() { TransactionCategory = _transaction.Key, TransactionAmount = CurrencySymbol + totalAmount.ToString(), TransactionPercent = totalPercent, CategoryColor = categoryColor });
            }
            return _chartData;
        }
      
        public void UpdateChartData(string transactionType)
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();            
            ChartData = GetChartData(dailyTransaction, transactionType);
        }

        public void UpdateAreaChartData(string dateRangeType, DateTime? startDate, DateTime? endDate)
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();
            var incomeTransactions = (dailyTransaction.Where(t => t.TransactionType == "Income")).ToObservableCollection<Transaction>();
            var expenseTransactions = (dailyTransaction.Where(t => t.TransactionType == "Expense")).ToObservableCollection<Transaction>();
            DashboardIncomeAreaChart = DataHelper.GetAreaChartData(incomeTransactions, dateRangeType, startDate, endDate);
            DashboardExpenseAreaChart = DataHelper.GetAreaChartData(expenseTransactions, dateRangeType, startDate, endDate);
        }

        private void UpdateSavingsAreaChartData(string rangeType, DateTime? startDate, DateTime? endDate)
        {
            ObservableCollection<Savings> data = _dataStore.GetSavingsData();
            var filteredData = data.Where(s => s.SavingsDate >= startDate && s.SavingsDate <= endDate).ToList();

            switch (rangeType)
            {
                case "This Week":
                    Dictionary<string, double> weeklySavings = new Dictionary<string, double>
            {
                { "Sun", 0 }, { "Mon", 0 }, { "Tue", 0 }, { "Wed", 0 },
                { "Thu", 0 }, { "Fri", 0 }, { "Sat", 0 }
            };

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

                    Dictionary<string, double> lastWeekSavings = new Dictionary<string, double>
            {
                { "Sun", 0 }, { "Mon", 0 }, { "Tue", 0 }, { "Wed", 0 },
                { "Thu", 0 }, { "Fri", 0 }, { "Sat", 0 }
            };

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
                    Dictionary<int, double> dailySavingsMonth = Enumerable.Range(1, DateTime.DaysInMonth(startDate.Value.Year, startDate.Value.Month))
                        .ToDictionary(day => day, day => 0.0);

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
                    Dictionary<string, double> monthlySavings = Enumerable.Range(1, 12)
                        .ToDictionary(month => CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(month), _ => 0.0);

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
                    Dictionary<string, double> lastSixMonthsSavings = Enumerable.Range(0, 6)
                        .Select(i => sixMonthsAgo.AddMonths(i).ToString("MMM"))
                        .ToDictionary(month => month, month => 0.0);

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
                        TimeInterval = entry.Key, // "Jan", "Feb", etc.
                        Data = entry.Value
                    }).ToObservableCollection();
                    break;
            }
        }

        private TransactionSummary GetTransactionSummary(ObservableCollection<Transaction> transactions)
        {
            var CurrencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            var previousBalance = CurrencySymbol + _dataStore.GetPreviousBalance();
            var totalExpense =  transactions.Where(t => t.TransactionType == "Expense").Sum(t => double.Parse(t.TransactionAmount));
            var totalIncome = transactions.Where(t => t.TransactionType == "Income").Sum(t => double.Parse(t.TransactionAmount));
            var totalSavings = CurrencySymbol + (totalIncome - totalExpense);

            return new TransactionSummary() { CurrentBalance = previousBalance, TotalExpense = CurrencySymbol + totalExpense, TotalIncome = CurrencySymbol + totalIncome, TotalSavings = totalSavings };
        }

        private Brush GetRandomLegendColor()
        {    
            var randomKey = legendColors[_random.Next(legendColors.Count)];
            //legendColors.Remove(randomKey); // Removed from list to ensure duplicate colors in chart

            if (Application.Current.Resources.TryGetValue(randomKey, out var colorValue) && colorValue is Color color)
            {
                return (new SolidColorBrush(Color.FromRgb(color.Red,color.Green,color.Blue)));
            }
            return (new SolidColorBrush(Color.FromRgb(Colors.Transparent.Red,Colors.Transparent.Green, Colors.Transparent.Blue)));
        }      
        
        private void InitializeLegendColors()
        {
            // Fetch colors from the resource dictionary
            legendColors = new List<string>
            {
            "LegendColor1", "LegendColor2", "LegendColor3", "LegendColor4", "LegendColor5", "LegendColor6", "LegendColor7", "LegendColor8", "LegendColor9", "LegendColor10",
            "LegendColor11", "LegendColor12", "LegendColor13", "LegendColor14", "LegendColor15", "LegendColor16", "LegendColor17", "LegendColor18", "LegendColor19", "LegendColor20"
            };
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
                summarizedData.Add(new SummarizedGoalData() { GoalId = data.GoalId, GoalTitle = data.GoalTitle, GoalPriority = data.GoalPriority, GoalDate = data.GoalDate, GoalAmount = data.GoalAmount, CurrencySymbol = this.CurrencySymbol, Icon = categoryIcon, IconColor = categoryColor, ContributionAmount = data.ContributionAmount, RemainingAmount = remainingAmount, Utilization = utilizedPercent, Remarks = data.Remarks, RemainingDays = (data.GoalDate - DateTime.Today).Days, Transactions = data.Transactions });
                elementCount++;
            }
            return summarizedData;
        }
    }


    public class TransactionChartData
    {
        private string transactionCategory;

        private string transactionAmount;

        private double transactionPercent;

        private Brush categoryColor;

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
    }

    public class TransactionSummary
    {
        private string? currenBalance;

        private string? totalIncome;

        private string? totalExpense;

        private string? totalSavings;

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
    }

    public class ChartDateRange : INotifyPropertyChanged
    {
        private string? rangeType;

        private DateTime? startDate;

        private DateTime? endDate;

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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
