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
    public partial class TransactionPageViewModel : INotifyPropertyChanged
    {
        private readonly DataStore _dataStore;

        private readonly UserDataService _userService;

        private ObservableCollection<Transaction> transactions;

        private List<SfSegmentItem> segmentTitle;

        private int selectedSegmentIndex;

        private List<ChartDateRange> dateRange;

        private ChartDateRange selectedGridDateRange;

        private ObservableCollection<TransactionGridData> transactionGridData;

        private int selectedRowCount;

        private bool isAllRowsSelected;

        private bool isPageEnabled = false;

        public ObservableCollection<Transaction> Transactions
        {
            get
            {
                return this.transactions;
            }
            set
            {
                this.transactions = value;
            }
        }

       
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

        public ObservableCollection<TransactionGridData> GridData
        {
            get
            {
                return this.transactionGridData;
            }
            set
            {
                this.transactionGridData = value;
                OnPropertyChanged(nameof(GridData));
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

        public bool IsAllRowsSelected
        {
            get
            {
                return this.isAllRowsSelected;
            }
            set
            {
                this.isAllRowsSelected = value;
                OnPropertyChanged(nameof(IsAllRowsSelected));
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


        public TransactionPageViewModel(UserDataService userService, DataStore dataStore)
        {
            _userService = userService;
            _dataStore = dataStore;
            SegmentTitle = new List<SfSegmentItem>()
            {
                new SfSegmentItem() {Text = "All"},
                new SfSegmentItem() {Text = "Income"},
                new SfSegmentItem() {Text = "Expense"}
            };
            SelectedSegmentIndex = 0;
            Transactions = dataStore.GetDailyTransactions();
            DateRange = new List<ChartDateRange>
            {
                new ChartDateRange() {RangeType = "This Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6)},
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)},
                new ChartDateRange() {RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31)}
            };
            SelectedGridDateRange = DateRange.ElementAt(1);
            UpdateGridData();
            IsAllRowsSelected = false;
        }

        public void UpdateGridData()
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();
            var transactionType = SegmentTitle.ElementAt(SelectedSegmentIndex).Text;
            GridData = GetGridData(dailyTransaction, transactionType);
        }

        private ObservableCollection<TransactionGridData> GetGridData(ObservableCollection<Transaction> transactions, string transactionType)
        {
            var currencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            var _transactions = transactionType == SegmentTitle.ElementAt(0).Text ? transactions.Where(t => t.TransactionDate >= SelectedGridDateRange.StartDate && t.TransactionDate <= SelectedGridDateRange.EndDate) : 
                transactions.Where(t => t.TransactionType == transactionType).Where(t => t.TransactionDate >= SelectedGridDateRange.StartDate && t.TransactionDate <= SelectedGridDateRange.EndDate);
            var _gridData = new ObservableCollection<TransactionGridData>();

            foreach (var transaction in _transactions)
            {
                _gridData.Add(new TransactionGridData()
                {
                    TransactionId = transaction.TransactionId,
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

        public void SelectAllRowsInGrid(bool value, int pageIndex, int pageSize)
        {
            var selectedRows = GridData;

            if (SelectedSegmentIndex == 1)
            {
                selectedRows = GridData.Where(t => t.TransactionType == "Income").ToObservableCollection<TransactionGridData>();
            }
            else if (SelectedSegmentIndex == 2)
            {
                selectedRows = GridData.Where(t => t.TransactionType == "Expense").ToObservableCollection<TransactionGridData>();
            }
            var indexedRows = selectedRows.Skip(pageIndex * pageSize).Take(pageSize);
            foreach (var row in indexedRows)
            {
                row.IsSelected = value;
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
            var selectedData = GridData.Where(t => t.IsSelected).ToObservableCollection<TransactionGridData>();
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
            if (_dataStore.DeleteTransactions(transactionIds, "Transaction"))
            {
                UpdateGridData();
                await Application.Current.MainPage.DisplayAlert("Success", "Transaction deleted successfully", "Okay");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "Deleting transaction failed", "Okay");
            }
        }

        public bool ExportExcel(ObservableCollection<TransactionGridData> selectedRows)
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
                    worksheet.Range["B1"].Text = "Category";
                    worksheet.Range["C1"].Text = "Transaction Type";
                    worksheet.Range["D1"].Text = "Amount";
                    worksheet.Range["E1"].Text = "Remark";

                    // Apply styles (optional)
                    worksheet.Range["A1:E1"].CellStyle.Font.Bold = true;

                    // Fill data from ObservableCollection
                    int rowIndex = 2;
                    foreach (var transaction in selectedRows)
                    {
                        worksheet.Range[$"A{rowIndex}"].Value = transaction.TransactionDate.ToString("dd/MM/yyyy");
                        worksheet.Range[$"B{rowIndex}"].Value = transaction.TransactionCategory;
                        worksheet.Range[$"C{rowIndex}"].Value = transaction.TransactionType;
                        worksheet.Range[$"D{rowIndex}"].Value = transaction.TransactionAmount;
                        worksheet.Range[$"E{rowIndex}"].Value = transaction.TransactionDescription;
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
    }

    public class TransactionGridData : INotifyPropertyChanged
    {
        private string? transactionCategory;

        private string? transactionName;

        private string? transactionAmount;

        private string? transactionDescription;

        private DateTime transactionDate;

        private string? transactionType;

        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public string? TransactionName
        {
            get
            {
                return this.transactionName;
            }
            set
            {
                this.transactionName = value;
            }
        }

        public string? TransactionCategory
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

        public string? TransactionAmount
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

        public string? TransactionDescription
        {
            get
            {
                return this.transactionDescription;
            }
            set
            {
                this.transactionDescription = value;
            }
        }

        public string? TransactionType
        {
            get
            {
                return this.transactionType;
            }
            set
            {
                this.transactionType = value;
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

        public double TransactionId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
