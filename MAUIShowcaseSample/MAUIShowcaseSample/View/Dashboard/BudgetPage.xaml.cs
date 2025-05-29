using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Buttons;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class BudgetPage : ContentPage
{
    private UserDataService _userCredentials;
    private DataStore _dataStore;
    private BudgetPageViewModel _viewModel;
    //DashboardLayoutPage layoutPage;

    public BudgetPage(BudgetPageViewModel viewmodel, UserDataService dataService, DataStore dataStore)
    {
        string pageTitle = "Budget";
        _userCredentials = dataService;
        _dataStore = dataStore;
        InitializeComponent();
        BindingContext = viewmodel;
        _viewModel = viewmodel;
        var layoutViewModel = new DashboardLayoutPageViewModel(dataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, dataService, dataStore);
        this.BudgetSegment.SelectionChanged += BudgetSegmentChanged;
    }

    private void BudgetSegmentChanged(object? sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    {
        if (sender != null && e.NewValue != null)
        {
            ((BudgetPageViewModel)BindingContext).UpdateBudgetData(e.NewValue.Text);
        }
    }

    private async void ViewDetailsClicked(object sender, EventArgs e)
    {

        if (sender is SfButton button && button.BindingContext is SummarizedBudgetData selectedBudget)
        {
            NavigationDataStore.BudgetDetailPageViewModel = new BudgetDetailPageViewModel(_userCredentials, _dataStore, selectedBudget);

            await Shell.Current.GoToAsync("///budgetdetailpage");
        }
    }

    private void OnPopupClicked(object sender, EventArgs e)
    {
        var button = sender as SfButton;
        if (button?.CommandParameter is int budgetId)
        {
            _viewModel.OpenPopup(budgetId);
        }
    }

    private async void OnAddSelection(object? sender, EventArgs e)
    {
        if (sender is SfButton button && button.BindingContext is SummarizedBudgetData selectedBudget)
        {
            selectedBudget.IsPopupOpen = false;
            ((DashboardLayoutPage)this.contentcontainer.Content).TriggerAddExpensePopup();
        }
    }

    private async void OnEditSelection(object? sender, EventArgs e)
    {
        if (sender is SfButton button && button.BindingContext is SummarizedBudgetData selectedBudget)
        {
            selectedBudget.IsPopupOpen = false;

            if (button?.CommandParameter is int budgetId)
            {
                 ((DashboardLayoutPage)this.contentcontainer.Content).TriggerEditBudgetPopup(budgetId);
            }
        }
    }

    private async void OnDeleteSelection(object? sender, EventArgs e)
    {
        _viewModel.DeleteBudget();
    }
}