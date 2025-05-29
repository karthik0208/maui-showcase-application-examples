using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Data;
using Syncfusion.XlsIO;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class SavingsPage : ContentPage
{
    //DashboardLayoutPage layoutPage;

    public SavingsPage(SavingsPageViewModel _viewModel, UserDataService dataService, DataStore dataStore)
    {
        string pageTitle = "Savings";
        InitializeComponent();
        BindingContext = _viewModel;
        var layoutViewModel = new DashboardLayoutPageViewModel(dataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, dataService, dataStore);
    }
    
    private void GridSegmentChanged(object? sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        ((SavingsPageViewModel)BindingContext).UpdateGridData();        
    }

    private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        ((SavingsPageViewModel)BindingContext).SelectAllRowsInGrid(e.Value);
    }

    private void OnSingleCheckboxChanged(object? sender, CheckedChangedEventArgs e)
    {

        var selectedCount = ((SavingsPageViewModel)BindingContext).GridData.Where(t => t.IsSelected == true).Count();
        ((SavingsPageViewModel)BindingContext).SelectedRowCount = selectedCount;

        if (selectedCount > 0)
        {
            if (selectedCount > 1)
            {
                this.editbutton.IsVisible = false;
            }
            else
            {
                this.editbutton.IsVisible = true;
            }
            this.segmentcontrol.IsVisible = false;
            this.selectioncontrol.IsVisible = true;
        }
        else
        {
            this.selectioncontrol.IsVisible = false;
            this.segmentcontrol.IsVisible = true;
        }

    }

    private void OnSelectCloseButtonClicked(object sender, EventArgs e)
    {
        this.selectioncontrol.IsVisible = false;
        this.segmentcontrol.IsVisible = true;
    }

    private async void OnEditSelection(object? sender, EventArgs e)
    {
        int transactionId = ((SavingsPageViewModel)BindingContext).GridData.Where(t => t.IsSelected == true).Select(t => t.TransactionId).First();
      //  layoutPage.TriggerEditSavePopup(transactionId);
    }
}