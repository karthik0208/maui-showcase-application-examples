using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Data;
using Syncfusion.Maui.DataGrid.DataPager;
using System.ComponentModel;
using System.Diagnostics;
using PageChangingEventArgs = Syncfusion.Maui.DataGrid.DataPager.PageChangingEventArgs;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class TransactionMobilePage : ContentPage
{
    //DashboardLayoutPage layoutPage;
    public TransactionMobilePage(TransactionPageViewModel viewModel, UserDataService dataService, DataStore dataStore)
    {
        string pageTitle = "Transaction";
        InitializeComponent();
        BindingContext = viewModel;
        var layoutViewModel = new DashboardLayoutPageViewModel(dataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, dataService, dataStore);
        this.bottomcontainer.Content = new DashboardBottomLayoutPage(layoutViewModel);
        DashboardLayoutPage layoutPage = (DashboardLayoutPage)this.contentcontainer.Content;
        this.addtransactioncontainer.Content = new AddTransactionMobilePage(layoutViewModel,layoutPage, dataService, dataStore);
        TransactionSegment.SelectionChanged += GridSegmentChanged;
    }

    private void GridSegmentChanged(object? sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    {
        ((TransactionPageViewModel)BindingContext).UpdateGridData();
    }

    private async void OnPageChanged(object? sender, PageChangingEventArgs e)
    {
        await UpdateGridData(e.NewPageIndex);
    }

    private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        ((TransactionPageViewModel)BindingContext).SelectAllRowsInGrid(e.Value, this.dataPager.PageIndex, this.dataPager.PageSize);
    }

    private async void OnSingleCheckboxChanged(object? sender, CheckedChangedEventArgs e)
    {
        await UpdateGridData(this.dataPager.PageIndex);
    }

    private async Task<bool> UpdateGridData(int pageIndex)
    {
        var selectedCount = ((TransactionPageViewModel)BindingContext).GridData.Skip(pageIndex * this.dataPager.PageSize).Take(this.dataPager.PageSize).Where(t => t.IsSelected == true).Count();
        ((TransactionPageViewModel)BindingContext).SelectedRowCount = selectedCount;

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
            this.TransactionSegment.IsVisible = false;
            this.selectioncontrol.IsVisible = true;
        }
        else
        {
            this.selectioncontrol.IsVisible = false;
            this.TransactionSegment.IsVisible = true;
        }
        if (selectedCount < this.dataPager.PageSize)
        {
            ((TransactionPageViewModel)BindingContext).IsAllRowsSelected = false;
        }

        return true;
    }

    private void OnSelectCloseButtonClicked(object sender, EventArgs e)
    {
        this.selectioncontrol.IsVisible = false;
        this.TransactionSegment.IsVisible = true;
    }

    private async void OnEditSelection(object? sender, EventArgs e)
    {
        double transactionId = ((TransactionPageViewModel)BindingContext).GridData.Where(t => t.IsSelected == true).Select(t => t.TransactionId).First();
        ((DashboardLayoutPage)this.contentcontainer.Content).TriggerEditTransactionPopup(transactionId);
    }
}