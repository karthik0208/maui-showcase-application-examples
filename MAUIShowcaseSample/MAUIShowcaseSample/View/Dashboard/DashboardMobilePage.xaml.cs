using MAUIShowcaseSample.Services;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class DashboardMobilePage : ContentPage
{
    public DashboardMobilePage(DashboardPageViewModel viewModel, DataStore dataStore, UserDataService userDataService)
    {
        string pageTitle = "Dashboard";

        InitializeComponent();
        BindingContext = viewModel;
        var layoutViewModel = new DashboardLayoutPageViewModel(userDataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, userDataService, dataStore);
        this.bottomcontainer.Content = new DashboardBottomLayoutPage(layoutViewModel);
        DashboardLayoutPage layoutPage = (DashboardLayoutPage)this.contentcontainer.Content;
        this.addtransactioncontainer.Content = new AddTransactionMobilePage(layoutViewModel,layoutPage, userDataService, dataStore);
        TransactionSegment.SelectionChanged += ChartSegmentChanged;
    }

    private void ChartSegmentChanged(object? sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    {
        ((DashboardPageViewModel)BindingContext).UpdateChartData(e.NewValue.Text);
    }
}