using MAUIShowcaseSample.Services;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardPageViewModel viewModel, DataStore dataStore, UserDataService userDataService)
	{
        string pageTitle = "Dashboard";
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Locked;
        InitializeComponent();
		BindingContext = viewModel;
        var layoutViewModel = new DashboardLayoutPageViewModel(userDataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, userDataService, dataStore);
        TransactionSegment.SelectionChanged += ChartSegmentChanged;
    }

    private void ChartSegmentChanged(object? sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    {
        ((DashboardPageViewModel)BindingContext).UpdateChartData(e.NewValue.Text);
    }
}