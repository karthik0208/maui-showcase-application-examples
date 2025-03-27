using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Buttons;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Transaction = MAUIShowcaseSample.Services.Transaction;

namespace MAUIShowcaseSample
{
    public class BudgetPageViewModel : INotifyPropertyChanged
    {
        private List<SfSegmentItem> segmentTitle;

        private int selectedSegmentIndex;

        private readonly DataStore _dataStore;

        private ObservableCollection<Budget> selectedSegmentData = new ObservableCollection<Budget>();

        private ObservableCollection<SummarizedBudgetData> budgetData = new();

        private ObservableCollection<TransactionChartData> transactionChartData;

        private string currencySymbol;

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BudgetPageViewModel(UserDataService userDataService, DataStore dataStore)
        {
            _dataStore = dataStore;
            SegmentTitle = new List<SfSegmentItem>()
            {
                new SfSegmentItem() {Text = "Active Budget"},
                new SfSegmentItem() {Text = "Completed Budget"}
            };
            SelectedSegmentIndex = 0;
            currencySymbol = userDataService.GetUserCurrencySymbol(userDataService.LoggedInAccount);
            SelectedSegmentData = dataStore.GetBudgetList(SegmentTitle[SelectedSegmentIndex].Text);
            BudgetData = GetSummarizedBudgetData(selectedSegmentData);
        }

        public void UpdateBudgetData(string selectedSegment)
        {
            SelectedSegmentData = _dataStore.GetBudgetList(selectedSegment);
            BudgetData = GetSummarizedBudgetData(selectedSegmentData);
        }

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
                summarizedData.Add(new SummarizedBudgetData() { BudgetTitle = data.BudgetTitle, BudgetCategory = data.BudgetCategory, BudgetDate = data.BudgetDate, BudgetAmount = data.BudgetAmount, CurrencySymbol = this.currencySymbol, Icon = categoryIcon, CategoryColor = categoryColor, AmountSpent = amountSpent, RemainingAmount = remainingAmount, Utilization = utilizedPercent });
            }
            return summarizedData;
        }

        private string GetBudgetCategoryIcon(string category)
        {
            string iconCode = "\ue73d";

            if (Enum.TryParse(category, out BudgetCategory budgetCategory))
            {
                switch (budgetCategory)
                {
                    case BudgetCategory.MonthlyBudget:
                        iconCode = "\ue723";
                        break;

                    case BudgetCategory.VacationBudget:
                        iconCode = "\ue73b";
                        break;

                    case BudgetCategory.TransportBudget:
                        iconCode = "\ue740";
                        break;                    
                }
            }           
            return iconCode;
        }

        private Color GetBudgetCategoryColor(string category)
        {
            Color color = Color.FromArgb("#EC5C7B");

            if (Enum.TryParse(category, out BudgetCategory budgetCategory))
            {
                switch (budgetCategory)
                {
                    case BudgetCategory.MonthlyBudget:
                        if(Application.Current.Resources.TryGetValue("SeriesColor1", out var series1))
                        {
                            color = (Color)series1;
                        }                        
                        break;

                    case BudgetCategory.VacationBudget:
                        if (Application.Current.Resources.TryGetValue("SeriesColor2", out var series2))
                        {
                            color = (Color)series2;
                        }
                        break;

                    case BudgetCategory.TransportBudget:
                        if (Application.Current.Resources.TryGetValue("SeriesColor3", out var series3))
                        {
                            color = (Color)series3;
                        }
                        break;
                }
            }
            return color;
        }

        private double GetAmountSpent(DataStore dataStore, Budget budgetData)
        {
            double amountSpent = 0;
            ObservableCollection<Transaction> filteredData = new ObservableCollection<Transaction>();
            switch(budgetData.BudgetCategory)
            {
                case BudgetCategory.MonthlyBudget:
                    filteredData = dataStore.GetDailyTransactions(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1));
                    amountSpent = filteredData.Where(t => t.TransactionType == "Expense").Sum(t => double.TryParse(t.TransactionAmount, out double amount) ? amount : 0);
                    break;

                case BudgetCategory.TransportBudget:
                    filteredData = dataStore.GetDailyTransactions();
                    List<string> transportExpenseCategory = BudgetCategories.CategoryItems.GetValueOrDefault(BudgetCategory.TransportBudget);
                    amountSpent = filteredData.Where(t => t.TransactionType == "Expense" && transportExpenseCategory.Contains(item: t.TransactionCategory)).Sum(t => double.TryParse(t.TransactionAmount, out double amount) ? amount : 0);
                    break;

                case BudgetCategory.VacationBudget:
                    filteredData = dataStore.GetDailyTransactions();
                    List<string> vacationExpenseCategory = BudgetCategories.CategoryItems.GetValueOrDefault(BudgetCategory.VacationBudget);
                    amountSpent = filteredData.Where(t => t.TransactionType == "Expense" && vacationExpenseCategory.Contains(item: t.TransactionCategory)).Sum(t => double.TryParse(t.TransactionAmount, out double amount) ? amount : 0);
                    break;
            }
            return amountSpent;
        }

        public void UpdateChartData(string transactionType)
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();
            ChartData = GetChartData(dailyTransaction, transactionType);
        }

        private ObservableCollection<TransactionChartData> GetChartData(ObservableCollection<Transaction> transactions, string transactionType)
        {
            //var currencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            //var _transactions = transactions.Where(t => t.TransactionType == transactionType).Where(t => t.TransactionDate >= SelectedChartDateRange.StartDate && t.TransactionDate <= SelectedChartDateRange.EndDate);
            var _chartData = new ObservableCollection<TransactionChartData>();

            //var sumAmount = _transactions.Sum(t => double.Parse(t.TransactionAmount));

            //var groupedTransaction = _transactions.GroupBy(t => t.TransactionName);

            //InitializeLegendColors(); // Initializing legend colors 

            //foreach (var _transaction in groupedTransaction)
            //{
            //    var totalAmount = _transaction.Sum(t => double.Parse(t.TransactionAmount));
            //    var totalPercent = (totalAmount / sumAmount) * 100;
            //    Brush categoryColor = GetRandomLegendColor();
            //    LegendColors.Add(categoryColor);
            //    _chartData.Add(new TransactionChartData() { TransactionCategory = _transaction.Key, TransactionAmount = currencySymbol + totalAmount.ToString(), TransactionPercent = totalPercent, CategoryColor = categoryColor });
            //}
            return _chartData;
        }
    }

    public class SummarizedBudgetData
    {
        private string? budgetTitle;

        private BudgetCategory budgetCategory;

        private double? budgetAmount;

        private DateTime budgetDate;

        private string? budgetRemarks;

        private bool isCompleted;

        private string? currencySymbol;

        private double amountSpent;

        private double remainingAmount;

        private double utilization;

        private string? icon;

        private Color? categoryColor;

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

        public BudgetCategory BudgetCategory
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
    }

    public class BudgetCategories
    {
        public static readonly Dictionary<BudgetCategory, List<string>> CategoryItems = new()
    {
        { BudgetCategory.TransportBudget, new List<string> { "Transportation" } },
        { BudgetCategory.VacationBudget, new List<string> { "Shopping" } }
    };
    }
}
