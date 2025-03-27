using Syncfusion.Maui.Buttons;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class GoalsPage : ContentView
{
	public GoalsPage(GoalsPageViewModel _viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel;
	}

    private void GoalSegmentChanged(object? sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        ((GoalsPageViewModel)BindingContext).UpdateGoalsData(e.NewValue.Text);
    }

    private async void ViewDetailsClicked(object sender, EventArgs e)
    {
        if (sender is SfButton button && button.BindingContext is SummarizedGoalData selectedBudget)
        {
            // Now you have the clicked BudgetData item
            //Console.WriteLine($"Clicked Budget: {selectedBudget.BudgetTitle}, Amount: {selectedBudget.BudgetAmount}");

            // You can navigate or update UI accordingly
            // Example: Navigate to details page
            // await Navigation.PushAsync(new BudgetDetailsPage(selectedBudget));
        }
    }
}