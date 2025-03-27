using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    public class SavingsPageViewModel : INotifyPropertyChanged
    {
        private List<SfSegmentItem> segmentTitle;
        private int selectedSegmentIndex;
        private List<ChartDateRange> dateRange;
        private ChartDateRange selectedGridDateRange;
        private ObservableCollection<SavingsChartData> savingsGridData;
        private DataStore _dataStore;
        private UserDataService _userDataService;
        private bool isAllRowsSelected;

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

        public ChartDateRange SelectedGridDateRange
        {
            get
            {
                return this.selectedGridDateRange;
            }
            set
            {
                this.selectedGridDateRange = value;
                UpdateGridData(SegmentTitle.ElementAt(SelectedSegmentIndex).Text);
                OnPropertyChanged("SelectedChartDateRange");
            }
        }

        public ObservableCollection<SavingsChartData> GridData
        {
            get
            {
                return this.savingsGridData;
            }
            set
            {
                this.savingsGridData = value;
                OnPropertyChanged(nameof(GridData));
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SavingsPageViewModel(UserDataService userDataService, DataStore dataStore)
        {
            _dataStore = dataStore;
            _userDataService = userDataService;
            SegmentTitle = new List<SfSegmentItem>()
            {
                new SfSegmentItem() {Text = "All"},
                new SfSegmentItem() {Text = "Deposit"},
                new SfSegmentItem() {Text = "Withdrawal"}
            };
            SelectedSegmentIndex = 0;

            DateRange = new List<ChartDateRange>
            {
                new ChartDateRange() {RangeType = "This Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6)},
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)},
                new ChartDateRange() {RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31)}
            };
            SelectedGridDateRange = DateRange[0];

            GridData = GetGridData(dataStore.GetSavingsData(), "All");
        }

        public void UpdateGridData(string transactionType)
        {
            var savingsTransaction = _dataStore.GetSavingsData();
            GridData = GetGridData(savingsTransaction, transactionType);
        }

        private ObservableCollection<SavingsChartData> GetGridData(ObservableCollection<Savings> transactions, string transactionType)
        {
            var currencySymbol = _userDataService.GetUserCurrencySymbol(_userDataService.LoggedInAccount);
            var _transactions = transactionType == SegmentTitle.ElementAt(0).Text ? transactions.Where(t => t.SavingsDate >= SelectedGridDateRange.StartDate && t.SavingsDate <= SelectedGridDateRange.EndDate) :
                transactions.Where(t => t.SavingsType == transactionType).Where(t => t.SavingsDate >= SelectedGridDateRange.StartDate && t.SavingsDate <= SelectedGridDateRange.EndDate);
            var _gridData = new ObservableCollection<SavingsChartData>();

            foreach (var transaction in _transactions)
            {
                _gridData.Add(new SavingsChartData()
                {
                    SavingsAmount = transaction.SavingsAmount + currencySymbol,
                    SavingsDescription = transaction.SavingsDescription,
                    TransactionDate = transaction.SavingsDate,
                    SavingsType = transaction.SavingsType,
                    SavingsRemark = transaction.SavingsRemark
                });
            }
            return _gridData;
        }

        public void SelectAllRowsInGrid(bool value)
        {
            var selectedRows = GridData;

            if (SelectedSegmentIndex == 1)
            {
                selectedRows = GridData.Where(t => t.SavingsType == "Deposit").ToObservableCollection<SavingsChartData>();
            }
            else if (SelectedSegmentIndex == 2)
            {
                selectedRows = GridData.Where(t => t.SavingsType == "Withdrawal").ToObservableCollection<SavingsChartData>();
            }

            foreach (var row in selectedRows)
            {
                row.IsSelected = value;
            }
        }

    }

    public class SavingsChartData : INotifyPropertyChanged
    {
        private DateTime transactionDate;
        private string savingsDescription;
        private string savingsType;
        private string savingsAmount;
        private string savingsRemark;
        private bool isSelected;

        public DateTime TransactionDate
        {
            get
            {
                return this.transactionDate;
            }
            set
            {
                this.transactionDate = value;
            }
        }

        public string SavingsDescription
        {
            get
            {
                return this.savingsDescription;
            }
            set
            {
                this.savingsDescription = value;
            }
        }

        public string SavingsType
        {
            get
            {
                return this.savingsType;
            }
            set
            {
                this.savingsType = value;
            }
        }

        public string SavingsAmount
        {
            get
            {
                return this.savingsAmount;
            }
            set
            {
                this.savingsAmount = value;
            }
        }

        public string SavingsRemark
        {
            get
            {
                return this.savingsRemark;
            }
            set
            {
                this.savingsRemark = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
