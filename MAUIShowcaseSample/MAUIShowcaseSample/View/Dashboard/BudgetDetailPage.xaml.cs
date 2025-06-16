using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Data;
using Syncfusion.XlsIO;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class BudgetDetailPage : ContentPage
{
    public BudgetDetailPage(UserDataService dataService, DataStore dataStore)
    {
        string pageTitle = "Budget";
        InitializeComponent();
        BindingContext = NavigationDataStore.BudgetDetailPageViewModel;       
    }

    private void ChartSegmentChanged(object? sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        ((BudgetPageViewModel)BindingContext).UpdateChartData(e.NewValue.Text);
    }

    private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        ((BudgetDetailPageViewModel)BindingContext).SelectAllRowsInGrid(e.Value);
    }
    private async void OnExportClicked(object sender, EventArgs e)
    {
        var selectedData = ((BudgetDetailPageViewModel)BindingContext).TransactionGridData.Where(t => t.IsSelected).ToObservableCollection<TransactionGridData>();
        if (selectedData.Count() == 0)
        {
            await Application.Current.MainPage.DisplayAlert("Export failed", "Please select rows to export.", "OK");
        }
        else
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
                    foreach (var transaction in selectedData)
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
            }
            catch (Exception ex)
            {
            }
        }
    }
}