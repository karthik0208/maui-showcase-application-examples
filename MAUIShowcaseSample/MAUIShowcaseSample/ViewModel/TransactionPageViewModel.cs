using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    public class TransactionPageViewModel : INotifyPropertyChanged
    {
        private readonly DataStore _dataStore;

        private readonly UserDataService _userService;

        private ObservableCollection<Transaction> transactions;

        private List<SfSegmentItem> segmentTitle;

        private int selectedSegmentIndex;

        private List<ChartDateRange> dateRange;

        private ChartDateRange selectedGridDateRange;

        private ObservableCollection<TransactionGridData> transactionGridData;

        private bool isAllRowsSelected;

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
                UpdateGridData(SegmentTitle.ElementAt(SelectedSegmentIndex).Text);
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
            UpdateGridData(SegmentTitle.ElementAt(SelectedSegmentIndex).Text);
            IsAllRowsSelected = false;
        }

        public void UpdateGridData(string transactionType)
        {
            var dailyTransaction = _dataStore.GetDailyTransactions();
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

        public void SelectAllRowsInGrid(bool value)
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
            
            foreach (var row in selectedRows)
            {
                row.IsSelected = value;
            }
        }

        //public async void ExportCSV()
        //{
        //    if(GridData.Where(t => t.IsSelected).Count() == 0)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Export failed", "Please select rows to export.", "OK");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            using (ExcelEngine excelEngine = new ExcelEngine())
        //            {
        //                Syncfusion.XlsIO.IApplication application = excelEngine.Excel;
        //                application.DefaultVersion = ExcelVersion.Xlsx;

        //                // Create a workbook and worksheet
        //                IWorkbook workbook = application.Workbooks.Create(1);
        //                IWorksheet worksheet = workbook.Worksheets[0];

        //                string downloadsPath = FileSystem.Current.AppDataDirectory;
                        
        //                // On Windows, set the actual Downloads path
        //                #if WINDOWS ||  MACCATALYST
        //                    downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
        //                #elif ANDROID
        //                    downloadsPath = "/storage/emulated/0/Download";
        //                #elif IOS
        //                    downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //                #endif

        //                string filePath = Path.Combine(downloadsPath, "ExpenseAnalysis.xlsx");

        //                //Saving the workbook as stream
        //                FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
        //                workbook.SaveAs(stream);

        //                //Dispose stream
        //                stream.Dispose();

        //                //// Export SfDataGrid to Excel
        //                //DataGridExcelExportingController excelExport = new DataGridExcelExportingController();
        //                //DataGridExcelExportingOption options = new DataGridExcelExportingOption
        //                //{
        //                //    ExportColumnWidth = true,
        //                //    ExportRowHeight = true
        //                //};

        //                //// Convert DataGrid to Excel Worksheet
        //                //worksheet = excelExport.ExportToExcel(GridData, options, workbook.Worksheets[0]);

        //                //// Save the file
        //                //MemoryStream stream = new MemoryStream();
        //                //workbook.SaveAs(stream);
        //                //workbook.Close();
        //                //stream.Position = 0;

        //                //// Save and Open the Excel File
        //                //await SaveAndOpenExcelFile(stream, "ExportedData.xlsx");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //           // await DisplayAlert("Error", $"Export Failed: {ex.Message}", "OK");
        //        }
        //    }
        //}
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
