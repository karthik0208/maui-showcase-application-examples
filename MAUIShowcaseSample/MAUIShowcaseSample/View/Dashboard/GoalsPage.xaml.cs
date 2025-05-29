using MAUIShowcaseSample.Services;
using MAUIShowcaseSample.ViewModel;
using Syncfusion.Maui.Buttons;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class GoalsPage : ContentPage
{
    private UserDataService _userCredentials;
    private DataStore _dataStore;
    private GoalsPageViewModel _viewModel;
   // DashboardLayoutPage layoutPage;

    public GoalsPage(GoalsPageViewModel viewModel, UserDataService userDataService, DataStore dataStore)
    {
        string pageTitle = "Goal";
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        var layoutViewModel = new DashboardLayoutPageViewModel(userDataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, userDataService, dataStore);
        _userCredentials = userDataService;
        _dataStore = dataStore;
    }

    private void GoalSegmentChanged(object? sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        if (sender != null && e.NewValue != null)
        {
            ((GoalsPageViewModel)BindingContext).UpdateGoalsData(e.NewValue.Text);
        }
    }

    private async void ViewDetailsClicked(object sender, EventArgs e)
    {
        if (sender is SfButton button && button.BindingContext is SummarizedGoalData selectedGoalData)
        {
            NavigationDataStore.GoalDetailPageViewModel = new GoalDetailPageViewModel(_userCredentials, _dataStore, selectedGoalData);

            await Shell.Current.GoToAsync("///goaldetailpage");
           
        }
    }

    private void OnPopupClicked(object sender, EventArgs e)
    {
        var button = sender as SfButton;
        if (button?.CommandParameter is int goalId)
        {
            _viewModel.OpenPopup(goalId);
        }
    }

    private async void OnAddSelection(object? sender, EventArgs e)
    {
        if (sender is SfButton button && button.BindingContext is SummarizedBudgetData selectedBudget)
        {
            selectedBudget.IsPopupOpen = false;
           // layoutPage.TriggerAddExpensePopup();
        }
    }

    private async void OnEditSelection(object? sender, EventArgs e)
    {
        if (sender is SfButton button && button.BindingContext is SummarizedGoalData selectedGoal)
        {
            selectedGoal.IsPopupOpen = false;

            if (button?.CommandParameter is int goalId)
            {
               // layoutPage.TriggerEditGoalPopup(goalId);
            }
        }
    }

    private async void OnDeleteSelection(object? sender, EventArgs e)
    {
        _viewModel.DeleteGoal();
    }
}