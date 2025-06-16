using CommunityToolkit.Mvvm.Input;
using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Toolkit.SegmentedControl;
using Syncfusion.Maui.Data;
using Syncfusion.XlsIO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    public partial class SavingsPageViewModel : INotifyPropertyChanged
    {
        private List<SfSegmentItem> segmentTitle;
        private int selectedSegmentIndex;
        private List<ChartDateRange> dateRange;
        private ChartDateRange selectedGridDateRange;
        private ObservableCollection<SavingsChartData> savingsGridData;
        private DataStore _dataStore;
        private UserDataService _userDataService;
        private bool isAllRowsSelected;
        private int selectedRowCount;
        private string totalSavingCartValue;
        private string currentMonthSavingsCartValue;
        private string emergencyFundCartValue;
        private bool isPageEnabled = false;

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
                UpdateGridData();
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

        public int SelectedRowCount
        {
            get
            {
                return this.selectedRowCount;
            }
            set
            {
                this.selectedRowCount = value;
                OnPropertyChanged(nameof(SelectedRowCount));
            }
        }

        public string TotalSavingsCartValue 
        { 
            get
            {
                return this.totalSavingCartValue;
            }
            set
            {
                this.totalSavingCartValue = value;
                OnPropertyChanged(nameof(TotalSavingsCartValue));
            }
        }

        public string CurrentMonthSavingsCartValue
        {
            get
            {
                return this.currentMonthSavingsCartValue;
            }
            set
            {
                this.currentMonthSavingsCartValue = value;
                OnPropertyChanged(nameof(CurrentMonthSavingsCartValue));
            }
        }

        public string EmergencyFundCartValue
        {
            get
            {
                return this.emergencyFundCartValue;
            }
            set
            {
                this.emergencyFundCartValue = value;
                OnPropertyChanged(nameof(EmergencyFundCartValue));
            }
        }

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
            UpdateGridData();            
            UpdateCartValue();
        }

        public void UpdateGridData()
        {
            var savingsTransaction = _dataStore.GetSavingsData();
            GridData = GetGridData(savingsTransaction, SegmentTitle[SelectedSegmentIndex].Text);
        }

        public void UpdateCartValue()
        {
            var currencySymbol = _userDataService.GetUserCurrencySymbol(_userDataService.LoggedInAccount);

            var savingsTransaction = _dataStore.GetSavingsData();
            var totalDeposit = savingsTransaction.Where(t => t.SavingsType == "Deposit").Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            var totalWithdraw = savingsTransaction.Where(t => t.SavingsType == "Withdrawal").Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            TotalSavingsCartValue = currencySymbol + (totalDeposit - totalWithdraw).ToString();

            var currentMonthDeposit = savingsTransaction.Where(t => t.SavingsType == "Deposit" && t.SavingsDate >= DateRange[1].StartDate && t.SavingsDate <= DateRange[1].EndDate).Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            var currentMonthWithdrawal = savingsTransaction.Where(t => t.SavingsType == "Withdrawal" && t.SavingsDate >= DateRange[1].StartDate && t.SavingsDate <= DateRange[1].EndDate).Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            CurrentMonthSavingsCartValue = currencySymbol + (currentMonthDeposit - currentMonthWithdrawal).ToString();

            var emergencyFundDeposit = savingsTransaction.Where(t => t.SavingsType == "Deposit" && t.SavingsDescription == "Emergency Fund").Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            var emergencyFundWithdrawal = savingsTransaction.Where(t => t.SavingsType == "Withdrawal" && t.SavingsDescription == "Emergency Access").Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            EmergencyFundCartValue = currencySymbol + (emergencyFundDeposit - emergencyFundWithdrawal).ToString();
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
                    TransactionId = transaction.TransactionId,
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

        public bool ExportExcel(ObservableCollection<SavingsChartData> data)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    Syncfusion.XlsIO.IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Xlsx;

                    // Create a workbook and worksheet
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];

                    // Add headers
                    worksheet.Range["A1"].Text = "Transaction Date";
                    worksheet.Range["B1"].Text = "Description";
                    worksheet.Range["C1"].Text = "Transaction Type";
                    worksheet.Range["D1"].Text = "Amount";
                    worksheet.Range["E1"].Text = "Remark";

                    // Apply styles (optional)
                    worksheet.Range["A1:E1"].CellStyle.Font.Bold = true;

                    // Fill data from ObservableCollection
                    int rowIndex = 2;
                    foreach (var transaction in data)
                    {
                        worksheet.Range[$"A{rowIndex}"].Value = transaction.TransactionDate.ToString("dd/MM/yyyy");
                        worksheet.Range[$"B{rowIndex}"].Value = transaction.SavingsDescription;
                        worksheet.Range[$"C{rowIndex}"].Value = transaction.SavingsType;
                        worksheet.Range[$"D{rowIndex}"].Value = transaction.SavingsAmount;
                        worksheet.Range[$"E{rowIndex}"].Value = transaction.SavingsRemark;
                        rowIndex++;
                    }

                    MemoryStream stream = new MemoryStream();
                    workbook.SaveAs(stream);

                    workbook.Close();
                    //Dispose stream
                    excelEngine.Dispose();

                    string OutputFilename = "ExpenseAnalysis.xlsx";
                    SaveService saveService = new();
                    saveService.SaveAndView(OutputFilename, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", stream);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [RelayCommand]
        public async Task ExportAllDataAsync()
        {
            if (ExportExcel(GridData))
            {
                return;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "Excel export failed", "Okay");
            }
        }

        [RelayCommand]
        public async Task ExportSelectedDataAsync()
        {
            var selectedData = GridData.Where(t => t.IsSelected).ToObservableCollection<SavingsChartData>();
            if (ExportExcel(selectedData))
            {
                return;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "Excel export failed", "Okay");
            }
        }

        [RelayCommand]
        public async Task DeleteTransactionAsync()
        {
            List<double> transactionIds = GridData.Where(t => t.IsSelected == true).Select(t => t.TransactionId).ToList();
            if(_dataStore.DeleteTransactions(transactionIds, "Savings"))
            {
                UpdateGridData();
                UpdateCartValue();
                await Application.Current.MainPage.DisplayAlert("Success", "Transaction deleted successfully", "Okay");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "Deleting transaction failed", "Okay");
            }
        }
    }

    public class SavingsChartData : INotifyPropertyChanged
    {
        private double transactionId;
        private DateTime transactionDate;
        private string savingsDescription;
        private string savingsType;
        private string savingsAmount;
        private string savingsRemark;
        private bool isSelected;

        public double TransactionId
        {
            get
            {
                return this.transactionId;
            }
            set
            {
                this.transactionId = value;
            }
        }

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
