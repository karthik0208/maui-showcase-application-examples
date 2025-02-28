using Syncfusion.Maui.Buttons;
using System.Diagnostics;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class DashboardPage : ContentView
{
	public DashboardPage(DashboardPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        TransactionSegment.SelectionChanged += ChartSegmentChanged;
    }

    private void ChartSegmentChanged(object? sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        ((DashboardPageViewModel)BindingContext).UpdateChartData(e.NewValue.Text);        
    }
}