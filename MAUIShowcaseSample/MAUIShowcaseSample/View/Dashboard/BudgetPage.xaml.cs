namespace MAUIShowcaseSample.View.Dashboard;

public partial class BudgetPage : ContentView
{
	public BudgetPage(BudgetPageViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
        BudgetSegment.SelectionChanged += ChartSegmentChanged;
    }

    private void ChartSegmentChanged(object? sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
       // ((BudgetPageViewModel)BindingContext).UpdateChartData(e.NewValue.Text);
    }
}