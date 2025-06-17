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
    /// ViewModel for managing savings page operations including data filtering, export, and deletion
    /// </summary>
    public partial class SavingsPageViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// List of segment titles for savings type filtering
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
        /// Grid data for displaying savings transactions
        /// </summary>
        private ObservableCollection<SavingsChartData> savingsGridData;

        /// <summary>
        /// Data store service for savings operations
        /// </summary>
        private DataStore _dataStore;

        /// <summary>
        /// User data service for user-related operations
        /// </summary>
        private UserDataService _userDataService;

        /// <summary>
        /// Indicates if all rows are selected
        /// </summary>
        private bool isAllRowsSelected;

        /// <summary>
        /// Count of selected rows in the grid
        /// </summary>
        private int selectedRowCount;

        /// <summary>
        /// Total savings cart value display string
        /// </summary>
        private string totalSavingCartValue;

        /// <summary>
        /// Current month savings cart value display string
        /// </summary>
        private string currentMonthSavingsCartValue;

        /// <summary>
        /// Emergency fund cart value display string
        /// </summary>
        private string emergencyFundCartValue;

        /// <summary>
        /// Indicates if the page is enabled for user interaction
        /// </summary>
        private bool isPageEnabled = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the segment titles for savings type filtering
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
        /// Gets or sets the selected segment index for savings filtering
        /// </summary>
        /// <value>Integer representing the selected filter index (0=All, 1=Deposit, 2=Withdrawal)</value>
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
        /// Gets or sets the grid data for savings display
        /// </summary>
        /// <value>Observable collection of SavingsChartData objects</value>
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
                SelectAllRowsInGrid(this.IsAllRowsSelected);
                OnPropertyChanged(nameof(IsAllRowsSelected));
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
        /// Gets or sets the total savings cart value display string
        /// </summary>
        /// <value>String representing the formatted total savings amount</value>
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

        /// <summary>
        /// Gets or sets the current month savings cart value display string
        /// </summary>
        /// <value>String representing the formatted current month savings amount</value>
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

        /// <summary>
        /// Gets or sets the emergency fund cart value display string
        /// </summary>
        /// <value>String representing the formatted emergency fund amount</value>
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
        /// Initializes a new instance of the SavingsPageViewModel class
        /// </summary>
        /// <param name="userDataService">User data service for user operations</param>
        /// <param name="dataStore">Data store service for savings operations</param>
        public SavingsPageViewModel(UserDataService userDataService, DataStore dataStore)
        {
            _dataStore = dataStore;
            _userDataService = userDataService;

            // Initialize segment titles for savings type filtering
            SegmentTitle = new List<SfSegmentItem>()
            {
                new SfSegmentItem() {Text = "All"},
                new SfSegmentItem() {Text = "Deposit"},
                new SfSegmentItem() {Text = "Withdrawal"}
            };

            // Set default segment to "All"
            SelectedSegmentIndex = 0;

            // Initialize date range options
            DateRange = new List<ChartDateRange>
            {
                new ChartDateRange() {RangeType = "This Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6)},
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)},
                new ChartDateRange() {RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31)}
            };

            // Set default date range to "This Week"
            SelectedGridDateRange = DateRange[0];

            // Initialize data
            UpdateGridData();
            UpdateCartValue();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the grid data based on current filters and date range
        /// </summary>
        public void UpdateGridData()
        {
            // Get fresh savings data from data store
            var savingsTransaction = _dataStore.GetSavingsData();

            // Update grid data with filtered results
            GridData = GetGridData(savingsTransaction, SegmentTitle[SelectedSegmentIndex].Text);
        }

        /// <summary>
        /// Updates the cart values for total savings, current month, and emergency fund
        /// </summary>
        public void UpdateCartValue()
        {
            // Get user's currency symbol
            var currencySymbol = _userDataService.GetUserCurrencySymbol(_userDataService.LoggedInAccount);

            // Get savings transaction data
            var savingsTransaction = _dataStore.GetSavingsData();

            // Calculate total savings (deposits - withdrawals)
            var totalDeposit = savingsTransaction.Where(t => t.SavingsType == "Deposit").Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            var totalWithdraw = savingsTransaction.Where(t => t.SavingsType == "Withdrawal").Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            TotalSavingsCartValue = currencySymbol + (totalDeposit - totalWithdraw).ToString();

            // Calculate current month savings
            var currentMonthDeposit = savingsTransaction.Where(t => t.SavingsType == "Deposit" && t.SavingsDate >= DateRange[1].StartDate && t.SavingsDate <= DateRange[1].EndDate).Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            var currentMonthWithdrawal = savingsTransaction.Where(t => t.SavingsType == "Withdrawal" && t.SavingsDate >= DateRange[1].StartDate && t.SavingsDate <= DateRange[1].EndDate).Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            CurrentMonthSavingsCartValue = currencySymbol + (currentMonthDeposit - currentMonthWithdrawal).ToString();

            // Calculate emergency fund balance
            var emergencyFundDeposit = savingsTransaction.Where(t => t.SavingsType == "Deposit" && t.SavingsDescription == "Emergency Fund").Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            var emergencyFundWithdrawal = savingsTransaction.Where(t => t.SavingsType == "Withdrawal" && t.SavingsDescription == "Emergency Access").Sum(t => decimal.TryParse(t.SavingsAmount, out var amount) ? amount : 0);
            EmergencyFundCartValue = currencySymbol + (emergencyFundDeposit - emergencyFundWithdrawal).ToString();
        }

        /// <summary>
        /// Selects or deselects all rows in the grid
        /// </summary>
        /// <param name="value">True to select all rows, false to deselect</param>
        public void SelectAllRowsInGrid(bool value)
        {
            // Start with all grid data
            var selectedRows = GridData;

            // Filter based on selected segment
            if (SelectedSegmentIndex == 1) // Deposit only
            {
                selectedRows = GridData.Where(t => t.SavingsType == "Deposit").ToObservableCollection<SavingsChartData>();
            }
            else if (SelectedSegmentIndex == 2) // Withdrawal only
            {
                selectedRows = GridData.Where(t => t.SavingsType == "Withdrawal").ToObservableCollection<SavingsChartData>();
            }

            // Update selection state for each row
            foreach (var row in selectedRows)
            {
                row.IsSelected = value;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generates grid data from savings transactions based on type filter and date range
        /// </summary>
        /// <param name="transactions">Source savings transaction collection</param>
        /// <param name="transactionType">Transaction type filter (All, Deposit, or Withdrawal)</param>
        /// <returns>Observable collection of SavingsChartData objects</returns>
        private ObservableCollection<SavingsChartData> GetGridData(ObservableCollection<Savings> transactions, string transactionType)
        {
            // Get user's currency symbol
            var currencySymbol = _userDataService.GetUserCurrencySymbol(_userDataService.LoggedInAccount);

            // Apply transaction type and date range filters
            var _transactions = transactionType == SegmentTitle.ElementAt(0).Text ? 
                transactions.Where(t => t.SavingsDate >= SelectedGridDateRange.StartDate && t.SavingsDate <= SelectedGridDateRange.EndDate) :
                transactions.Where(t => t.SavingsType == transactionType).Where(t => t.SavingsDate >= SelectedGridDateRange.StartDate && t.SavingsDate <= SelectedGridDateRange.EndDate);

            // Create grid data collection
            var _gridData = new ObservableCollection<SavingsChartData>();

            // Convert savings transactions to grid data format
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

        #endregion

        #region Excel Export Methods

        /// <summary>
        /// Exports savings data to Excel file
        /// </summary>
        /// <param name="data">Collection of savings data to export</param>
        /// <returns>True if export is successful, false otherwise</returns>
        public bool ExportExcel(ObservableCollection<SavingsChartData> data)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    // Initialize Excel application
                    Syncfusion.XlsIO.IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Xlsx;

                    // Create a workbook and worksheet
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet worksheet = workbook.Worksheets[0];

                    // Add column headers
                    worksheet.Range["A1"].Text = "Transaction Date";
                    worksheet.Range["B1"].Text = "Description";
                    worksheet.Range["C1"].Text = "Transaction Type";
                    worksheet.Range["D1"].Text = "Amount";
                    worksheet.Range["E1"].Text = "Remark";

                    // Apply header styling
                    worksheet.Range["A1:E1"].CellStyle.Font.Bold = true;

                    // Fill data rows from savings collection
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

                    // Save workbook to memory stream
                    MemoryStream stream = new MemoryStream();
                    workbook.SaveAs(stream);

                    // Clean up workbook resources
                    workbook.Close();
                    excelEngine.Dispose();

                    // Save and view the exported file
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

        #region Command Methods

        /// <summary>
        /// Exports all grid data to Excel format
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        [RelayCommand]
        public async Task ExportAllDataAsync()
        {
            // Attempt to export all grid data
            if (ExportExcel(GridData))
            {
                return;
            }
            else
            {
                // Show error message if export fails
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
            // Get only selected rows
            var selectedData = GridData.Where(t => t.IsSelected).ToObservableCollection<SavingsChartData>();

            // Attempt to export selected data
            if (ExportExcel(selectedData))
            {
                return;
            }
            else
            {
                // Show error message if export fails
                await Application.Current.MainPage.DisplayAlert("Failed", "Excel export failed", "Okay");
            }
        }

        /// <summary>
        /// Deletes selected savings transactions from the data store
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        [RelayCommand]
        public async Task DeleteTransactionAsync()
        {
            // Get IDs of selected transactions
            List<double> transactionIds = GridData.Where(t => t.IsSelected == true).Select(t => t.TransactionId).ToList();

            // Attempt to delete transactions
            if (_dataStore.DeleteTransactions(transactionIds, "Savings"))
            {
                // Refresh data after successful deletion
                UpdateGridData();
                UpdateCartValue();
                await Application.Current.MainPage.DisplayAlert("Success", "Transaction deleted successfully", "Okay");
            }
            else
            {
                // Show error message if deletion fails
                await Application.Current.MainPage.DisplayAlert("Failed", "Deleting transaction failed", "Okay");
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents savings data formatted for grid display
    /// </summary>
    public class SavingsChartData : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Transaction identifier
        /// </summary>
        private double transactionId;

        /// <summary>
        /// Transaction date
        /// </summary>
        private DateTime transactionDate;

        /// <summary>
        /// Savings description
        /// </summary>
        private string savingsDescription;

        /// <summary>
        /// Savings type (Deposit/Withdrawal)
        /// </summary>
        private string savingsType;

        /// <summary>
        /// Savings amount with currency symbol
        /// </summary>
        private string savingsAmount;

        /// <summary>
        /// Savings remark/notes
        /// </summary>
        private string savingsRemark;

        /// <summary>
        /// Selection state for grid operations
        /// </summary>
        private bool isSelected;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the unique transaction identifier
        /// </summary>
        /// <value>Double representing the transaction ID</value>
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

        /// <summary>
        /// Gets or sets the transaction date
        /// </summary>
        /// <value>DateTime representing when the savings transaction occurred</value>
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
        /// Gets or sets the savings description
        /// </summary>
        /// <value>String representing the savings description</value>
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

        /// <summary>
        /// Gets or sets the savings type
        /// </summary>
        /// <value>String representing the savings type (Deposit/Withdrawal)</value>
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

        /// <summary>
        /// Gets or sets the savings amount with currency symbol
        /// </summary>
        /// <value>String representing the formatted savings amount</value>
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

        /// <summary>
        /// Gets or sets the savings remark
        /// </summary>
        /// <value>String representing additional notes or remarks</value>
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

        /// <summary>
        /// Gets or sets whether this savings transaction is selected in the grid
        /// </summary>
        /// <value>Boolean indicating selection state</value>
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