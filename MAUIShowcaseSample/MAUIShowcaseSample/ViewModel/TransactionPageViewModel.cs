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
    /// <summary>
    /// ViewModel for managing transaction page operations including data filtering, export, and deletion
    /// </summary>
    public partial class TransactionPageViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Data store service for transaction operations
        /// </summary>
        private readonly DataStore _dataStore;

        /// <summary>
        /// User data service for user-related operations
        /// </summary>
        private readonly UserDataService _userService;

        /// <summary>
        /// Collection of transactions
        /// </summary>
        private ObservableCollection<Transaction> transactions;

        /// <summary>
        /// List of segment titles for transaction type filtering
        /// </summary>
        private List<SfSegmentItem> segmentTitle;

        /// <summary>
        /// Currently selected segment index
        /// </summary>
        private int selectedSegmentIndex;

        /// <summary>
        /// Available date ranges for filtering
        /// </summary>
        private List<ChartDateRange> dateRange;

        /// <summary>
        /// Currently selected date range for grid filtering
        /// </summary>
        private ChartDateRange selectedGridDateRange;

        /// <summary>
        /// Grid data for displaying transactions
        /// </summary>
        private ObservableCollection<TransactionGridData> transactionGridData;

        /// <summary>
        /// Count of selected rows in the grid
        /// </summary>
        private int selectedRowCount;

        /// <summary>
        /// Indicates if all rows are selected
        /// </summary>
        private bool isAllRowsSelected;

        /// <summary>
        /// Indicates if the page is enabled for user interaction
        /// </summary>
        private bool isPageEnabled = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the collection of transactions
        /// </summary>
        /// <value>Observable collection of Transaction objects</value>
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

        /// <summary>
        /// Gets or sets the segment titles for transaction type filtering
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
        /// Gets or sets the selected segment index for transaction filtering
        /// </summary>
        /// <value>Integer representing the selected filter index (0=All, 1=Income, 2=Expense)</value>
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
        /// Gets or sets the selected date range for grid data filtering
        /// </summary>
        /// <value>ChartDateRange object representing the selected time period</value>
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

        /// <summary>
        /// Gets or sets the available date ranges for filtering
        /// </summary>
        /// <value>List of ChartDateRange objects representing available time periods</value>
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
        /// Gets or sets the grid data for transaction display
        /// </summary>
        /// <value>Observable collection of TransactionGridData objects</value>
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

        /// <summary>
        /// Gets or sets the count of selected rows in the grid
        /// </summary>
        /// <value>Integer representing the number of selected rows</value>
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

        /// <summary>
        /// Gets or sets whether all rows are selected
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
                OnPropertyChanged(nameof(IsAllRowsSelected));
            }
        }

        /// <summary>
        /// Gets or sets whether the page is enabled for user interaction
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

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the TransactionPageViewModel class
        /// </summary>
        /// <param name="userService">User data service for user operations</param>
        /// <param name="dataStore">Data store service for transaction operations</param>
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the grid data based on current filters and date range
        /// </summary>
        public void UpdateGridData()
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();
            var transactionType = SegmentTitle.ElementAt(SelectedSegmentIndex).Text;
            GridData = GetGridData(dailyTransaction, transactionType);
        }

        /// <summary>
        /// Selects or deselects all rows in the current page of the grid
        /// </summary>
        /// <param name="value">True to select all rows, false to deselect</param>
        /// <param name="pageIndex">Current page index</param>
        /// <param name="pageSize">Number of items per page</param>
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

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates grid data from transactions based on type filter and date range
        /// </summary>
        /// <param name="transactions">Source transaction collection</param>
        /// <param name="transactionType">Transaction type filter (All, Income, or Expense)</param>
        /// <returns>Observable collection of TransactionGridData objects</returns>
        private ObservableCollection<TransactionGridData> GetGridData(ObservableCollection<Transaction> transactions, string transactionType)
        {
            var currencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);

            var _transactions = transactionType == SegmentTitle.ElementAt(0).Text ?
                transactions.Where(t => t.TransactionDate >= SelectedGridDateRange.StartDate && t.TransactionDate <= SelectedGridDateRange.EndDate) :
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

        #endregion

        #region Command Methods

        /// <summary>
        /// Exports all grid data to Excel format
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
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

        /// <summary>
        /// Exports only selected grid data to Excel format
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
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

        /// <summary>
        /// Deletes selected transactions from the data store
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        [RelayCommand]
        public async Task DeleteTransactionAsync()
        {
            try
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
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while deleting transactions", "Okay");
            }
        }

        #endregion

        #region Excel Export Methods

        /// <summary>
        /// Exports transaction data to Excel file
        /// </summary>
        /// <param name="selectedRows">Collection of transaction data to export</param>
        /// <returns>True if export is successful, false otherwise</returns>
        public bool ExportExcel(ObservableCollection<TransactionGridData> selectedRows)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    Syncfusion.XlsIO.IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Xlsx;

                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];

                    worksheet.Range["A1"].Text = "Transaction Date";
                    worksheet.Range["B1"].Text = "Category";
                    worksheet.Range["C1"].Text = "Transaction Type";
                    worksheet.Range["D1"].Text = "Amount";
                    worksheet.Range["E1"].Text = "Remark";

                    worksheet.Range["A1:E1"].CellStyle.Font.Bold = true;

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

        #endregion
    }

    /// <summary>
    /// Represents transaction data formatted for grid display
    /// </summary>
    public class TransactionGridData : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Transaction category
        /// </summary>
        private string? transactionCategory;

        /// <summary>
        /// Transaction name/title
        /// </summary>
        private string? transactionName;

        /// <summary>
        /// Transaction amount with currency symbol
        /// </summary>
        private string? transactionAmount;

        /// <summary>
        /// Transaction description/remarks
        /// </summary>
        private string? transactionDescription;

        /// <summary>
        /// Transaction date
        /// </summary>
        private DateTime transactionDate;

        /// <summary>
        /// Transaction type (Income/Expense)
        /// </summary>
        private string? transactionType;

        /// <summary>
        /// Selection state for grid operations
        /// </summary>
        private bool isSelected;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets whether this transaction is selected in the grid
        /// </summary>
        /// <value>Boolean indicating selection state</value>
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

        /// <summary>
        /// Gets or sets the transaction name
        /// </summary>
        /// <value>String representing the transaction name</value>
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

        /// <summary>
        /// Gets or sets the transaction category
        /// </summary>
        /// <value>String representing the transaction category</value>
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

        /// <summary>
        /// Gets or sets the transaction amount with currency symbol
        /// </summary>
        /// <value>String representing the formatted transaction amount</value>
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

        /// <summary>
        /// Gets or sets the transaction description
        /// </summary>
        /// <value>String representing the transaction description</value>
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

        /// <summary>
        /// Gets or sets the transaction type
        /// </summary>
        /// <value>String representing the transaction type (Income/Expense)</value>
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

        /// <summary>
        /// Gets or sets the transaction date
        /// </summary>
        /// <value>DateTime representing when the transaction occurred</value>
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

        /// <summary>
        /// Gets or sets the unique transaction identifier
        /// </summary>
        /// <value>Double representing the transaction ID</value>
        public double TransactionId { get; set; }

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
}