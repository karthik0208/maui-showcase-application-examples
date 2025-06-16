using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Toolkit.Buttons;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class BudgetMobilePage : ContentPage
{
    private UserDataService _userCredentials;
    private DataStore _dataStore;
    private BudgetPageViewModel _viewModel;
    //DashboardLayoutPage layoutPage;

    public BudgetMobilePage(BudgetPageViewModel viewmodel, UserDataService dataService, DataStore dataStore)
    {
        string pageTitle = "Budget";
        _userCredentials = dataService;
        _dataStore = dataStore;
        InitializeComponent();
        BindingContext = viewmodel;
        _viewModel = viewmodel;
        var layoutViewModel = new DashboardLayoutPageViewModel(dataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, dataService, dataStore);
        this.bottomcontainer.Content = new DashboardBottomLayoutPage(layoutViewModel);
        DashboardLayoutPage layoutPage = (DashboardLayoutPage)this.contentcontainer.Content;
        this.addtransactioncontainer.Content = new AddTransactionMobilePage(layoutViewModel, layoutPage, dataService, dataStore);
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

            await Navigation.PushAsync(new BudgetDetailMobilePage(_userCredentials, _dataStore));

          //  await Shell.Current.GoToAsync("///budgetdetailpage");
        }
    }

    private void OnPopupClicked(object sender, EventArgs e)
    {
        var button = sender as SfButton;
        if (button?.CommandParameter is double budgetId)
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

            if (button?.CommandParameter is double budgetId)
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