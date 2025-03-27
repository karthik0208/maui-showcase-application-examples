using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Buttons;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class BudgetPage : ContentView
{
    private UserDataService _userCredentials;
    private DataStore _dataStore;

    public BudgetPage(BudgetPageViewModel viewmodel, UserDataService dataService, DataStore dataStore)
	{
        _userCredentials = dataService;
        _dataStore = dataStore;
		InitializeComponent();
		BindingContext = viewmodel;
        BudgetSegment.SelectionChanged += BudgetSegmentChanged;
    }

    private void BudgetSegmentChanged(object? sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        if(sender != null && e.NewValue != null)
        {
            ((BudgetPageViewModel)BindingContext).UpdateBudgetData(e.NewValue.Text);
        }        
    }

    private async void ViewDetailsClicked(object sender, EventArgs e)
    {
        if (sender is SfButton button && button.BindingContext is SummarizedBudgetData selectedBudget)
        {
            var _viewModel = new BudgetDetailPageViewModel(_userCredentials, _dataStore, selectedBudget);
            
            this.Content = new BudgetDetailPage(_viewModel);
            //var _viewModel = new Budget(_userCredentials, _dataStore);
            //this.ContentContainer.Content = new GoalsPage(_viewModel);
            // Now you have the clicked BudgetData item
            //Console.WriteLine($"Clicked Budget: {selectedBudget.BudgetTitle}, Amount: {selectedBudget.BudgetAmount}");

            // You can navigate or update UI accordingly
            // Example: Navigate to details page
            // await Navigation.PushAsync(new BudgetDetailsPage(selectedBudget));
        }
    }
}