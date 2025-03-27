using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Data;
using Syncfusion.XlsIO;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class SavingsPage : ContentView
{
	public SavingsPage(SavingsPageViewModel _viewModel)
	{
		InitializeComponent();
        BindingContext = _viewModel;
	}

    private void GridSegmentChanged(object? sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        ((SavingsPageViewModel)BindingContext).UpdateGridData(e.NewValue.Text);
    }

    private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        ((SavingsPageViewModel)BindingContext).SelectAllRowsInGrid(e.Value);
    }
    private async void OnExportClicked(object sender, EventArgs e)
    {
        var selectedData = ((SavingsPageViewModel)BindingContext).GridData.Where(t => t.IsSelected).ToObservableCollection<SavingsChartData>();
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
                    worksheet.Range["B1"].Text = "Description";
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
            }
            catch (Exception ex)
            {
            }
        }
    }
}