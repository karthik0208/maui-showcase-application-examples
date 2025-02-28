using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Buttons;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private int selectedSegmentIndex;

        private ObservableCollection<DataStore> selectedChartData;

        private ObservableCollection<TransactionChartData> transactionChartData;

        private List<ChartDateRange> dateRange;

        private ChartDateRange selectedChartDateRange;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Brush> LegendColors { get; set; } = new();

        public ObservableCollection<Transaction> Transactions { get; set; } = new();

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
        public DashboardPageViewModel(UserDataService userService, DataStore dataStore)
        {
            _userService = userService;
            _dataStore = dataStore;            
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
            SelectedChartDateRange = DateRange.ElementAt(1);
            var dailyTransaction = _dataStore.GetDailyTransactions();
            Transactions = UpdateRecentTransaction(dailyTransaction, 8);
            ChartData = GetChartData(dailyTransaction, SegmentTitle.ElementAt(SelectedSegmentIndex).Text);
            TotalTransactionSummary = GetTransactionSummary(dailyTransaction); 
        }

        private ObservableCollection<Transaction> UpdateRecentTransaction(ObservableCollection<Transaction> allTransaction, int? limitCount)
        {
            var _transactions = new ObservableCollection<Transaction>();

            var selectedTransactions = limitCount != null? allTransaction.Take((int)limitCount) : allTransaction;            

            var currencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
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
                amount = amount + currencySymbol + transaction.TransactionAmount;

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
        
        private ObservableCollection<TransactionChartData> GetChartData(ObservableCollection<Transaction> transactions, string transactionType)
        {
            var currencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
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
                _chartData.Add(new TransactionChartData() { TransactionCategory = _transaction.Key, TransactionAmount = currencySymbol + totalAmount.ToString(), TransactionPercent = totalPercent, CategoryColor = categoryColor });
            }
            return _chartData;
        }
      
        public void UpdateChartData(string transactionType)
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();            
            ChartData = GetChartData(dailyTransaction, transactionType);
        }

        private TransactionSummary GetTransactionSummary(ObservableCollection<Transaction> transactions)
        {
            var currencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            var previousBalance = currencySymbol + _dataStore.GetPreviousBalance();
            var totalExpense =  transactions.Where(t => t.TransactionType == "Expense").Sum(t => double.Parse(t.TransactionAmount));
            var totalIncome = transactions.Where(t => t.TransactionType == "Income").Sum(t => double.Parse(t.TransactionAmount));
            var totalSavings = currencySymbol + (totalIncome - totalExpense);

            return new TransactionSummary() { CurrentBalance = previousBalance, TotalExpense = currencySymbol + totalExpense, TotalIncome = currencySymbol + totalIncome, TotalSavings = totalSavings };
        }

        private Brush GetRandomLegendColor()
        {    
            var randomKey = legendColors[_random.Next(legendColors.Count)];
            legendColors.Remove(randomKey); // Removed from list to ensure duplicate colors in chart

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
