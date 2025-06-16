using MAUIShowcaseSample.Services;
using MAUIShowcaseSample.ViewModel;
using Syncfusion.Maui.Toolkit.Buttons;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class GoalsMobilePage : ContentPage
{
    private UserDataService _userCredentials;
    private DataStore _dataStore;
    private GoalsPageViewModel _viewModel;
   // DashboardLayoutPage layoutPage;

    public GoalsMobilePage(GoalsPageViewModel viewModel, UserDataService userDataService, DataStore dataStore)
    {
        string pageTitle = "Goal";
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _userCredentials = userDataService;
        _dataStore = dataStore;
        var layoutViewModel = new DashboardLayoutPageViewModel(userDataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, userDataService, dataStore);
        this.bottomcontainer.Content = new DashboardBottomLayoutPage(layoutViewModel);
        DashboardLayoutPage layoutPage = (DashboardLayoutPage)this.contentcontainer.Content;
        this.addtransactioncontainer.Content = new AddTransactionMobilePage(layoutViewModel, layoutPage, userDataService, dataStore);        
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
        if (button?.CommandParameter is double goalId)
        {
            _viewModel.OpenPopup(goalId);
        }
    }

    private async void OnAddFund(object? sender, EventArgs e)
    {
        if (sender is SfButton button && button.BindingContext is SummarizedGoalData selectedGoal)
        {
            selectedGoal.IsPopupOpen = false;
            if (button?.CommandParameter is double goalId)
            {
                string fundDescription = _dataStore.GetGoalById(goalId).GoalTitle;
                ((DashboardLayoutPage)this.contentcontainer.Content).TriggerAddFundPopup(goalId, fundDescription);
            }
        }
    }

    private async void OnEditSelection(object? sender, EventArgs e)
    {
        if (sender is SfButton button && button.BindingContext is SummarizedGoalData selectedGoal)
        {
            selectedGoal.IsPopupOpen = false;

            if (button?.CommandParameter is double goalId)
            {
                ((DashboardLayoutPage)this.contentcontainer.Content).TriggerEditGoalPopup(goalId);
            }
        }
    }

    private async void OnDeleteSelection(object? sender, EventArgs e)
    {
        _viewModel.DeleteGoal();
    }
}