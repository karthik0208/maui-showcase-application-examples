using MAUIShowcaseSample.Services;
using MAUIShowcaseSample.View.Dashboard;
using MAUIShowcaseSample.ViewModel;
using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.Inputs;
using Syncfusion.Maui.Popup;
using System.Text.RegularExpressions;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class DashboardLayoutPage : ContentView
{
    private UserDataService _userCredentials;
    private DataStore _dataStore;
    private DashboardLayoutPageViewModel layoutViewModel;
    private SavingsPageViewModel savingsViewModel;
    private TransactionPageViewModel transactionViewModel;
    private BudgetPageViewModel budgetViewModel;
    private GoalsPageViewModel goalViewModel;

    public DashboardLayoutPage(DashboardLayoutPageViewModel viewModel, UserDataService userCredentials, DataStore dataStore)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _userCredentials = userCredentials;
        layoutViewModel = viewModel;
        _dataStore = dataStore;
        LoadDashboard(null, null); // Load Dashboard by default        
    }

    private void LoadDashboard(object sender, EventArgs e)
    {
        //var _viewModel = new DashboardPageViewModel(_userCredentials, _dataStore); // Create ViewModel instance
        //this.PageTitle.Text = "Dashboard";
        //this.ContentContainer.Content = new DashboardPage(_viewModel);
    }

    private async void OnNavigationClicked(object sender, EventArgs e)
    {
        //if (((SfButton)sender).Text == "Transaction")
        //{
        //    this.PageTitle.Text = "Transaction";
        //    transactionViewModel = new TransactionPageViewModel(_userCredentials, _dataStore);
        //    this.ContentContainer.Content = new TransactionPage(transactionViewModel, this);
        //}
        //else if (((SfButton)sender).Text == "Dashboard")
        //{
        //    this.PageTitle.Text = "Dashboard";
        //    var _viewModel = new DashboardPageViewModel(_userCredentials, _dataStore); // Create ViewModel instance
        //    this.ContentContainer.Content = new DashboardPage(_viewModel);
        //}
        //else if (((SfButton)sender).Text == "Budget")
        //{
        //    this.PageTitle.Text = "Budget";
        //    budgetViewModel = new BudgetPageViewModel(_userCredentials, _dataStore);
        //    this.ContentContainer.Content = new BudgetPage(budgetViewModel, _userCredentials, _dataStore, this);
        //}
        //else if (((SfButton)sender).Text == "Savings")
        //{
        //    this.PageTitle.Text = "Savings";
        //    savingsViewModel = new SavingsPageViewModel(_userCredentials, _dataStore);
        //    this.ContentContainer.Content = new SavingsPage(savingsViewModel, this);
        //}
        //else if (((SfButton)sender).Text == "Goal")
        //{
        //    this.PageTitle.Text = "Goal";
        //    goalViewModel = new GoalsPageViewModel(_userCredentials, _dataStore);
        //    this.ContentContainer.Content = new GoalsPage(goalViewModel, _userCredentials, _dataStore, this);
        //}
        //else if (((SfButton)sender).Text == "Settings")
        //{
        //    this.PageTitle.Text = "Settings";
        //    var _viewModel = new SettingsPageViewModel(_userCredentials, _dataStore);
        //    this.ContentContainer.Content = new SettingsPage(_viewModel, _userCredentials, _dataStore);
        //}
        //else if (((SfButton)sender).Text == "Help & Support")
        //{
        //    this.PageTitle.Text = "Help & Support";
        //    var _viewModel = new HelpAndSupportPageViewModel();
        //    this.ContentContainer.Content = new HelpAndSupportPage(_viewModel);
        //}
    }

    private void OnCreateComboBoxSelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        ((SfComboBox)sender).Text = "Create";
        string selectedValue = e.CurrentSelection.ElementAt(0)?.ToString();

        if (selectedValue == "Transaction")
        {
            this.createtransactionpopup.IsOpen = true;
        }
        else if (selectedValue == "Budget")
        {
            this.createbudgetpopup.IsOpen = true;
        }
        else if (selectedValue == "Savings")
        {
            this.addsavingspopup.IsOpen = true;
        }
        else if (selectedValue == "Goal")
        {
            this.creategoalpopup.IsOpen = true;
        }
    }

    private void OnAvatarTapped(object sender, TappedEventArgs e)
    {

    }

    private void OnViewProfileClicked(object sender, EventArgs e)
    {

    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {

    }

    private async void CalendarIconClicked(object sender, System.EventArgs e)
    {
        ((DashboardLayoutPageViewModel)BindingContext).IsPickerOpen = true;
    }

    private void OKButtonClicked(object sender, System.EventArgs e)
    {
        ((DashboardLayoutPageViewModel)BindingContext).IsPickerOpen = false;
    }

    private void CancelButtonClicked(object sender, System.EventArgs e)
    {
        ((DashboardLayoutPageViewModel)BindingContext).IsPickerOpen = false;
    }
    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;

        // Allow only digits
        if (e.NewTextValue != null && !Regex.IsMatch(e.NewTextValue, @"^\d*$"))
        {
            entry.Text = e.OldTextValue; // Revert to old valid value
        }
        //else if (!Regex.IsMatch(e.OldTextValue, @"^\d*$"))
        //{
        //   // entry.Text = null;
        //}
    }

    private void OnTransactionCancelClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        viewModel.TransactionDetails.Clear();
        this.createtransactionpopup.IsOpen = false;
    }

    private async void OnTransactionAddClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        if (viewModel.OnAddTransactionClicked())
        {
            this.createtransactionpopup.IsOpen = false;
            await Application.Current.MainPage.DisplayAlert("Success", "Transaction added successfully", "OK");
        }
        else
        {
            this.createtransactionpopup.IsOpen = false;
            await Application.Current.MainPage.DisplayAlert("Failed", "Adding transaction failed", "OK");
        }
        viewModel.TransactionDetails.Clear();
        var currentPage = Shell.Current.CurrentState.Location.ToString();
        if(currentPage == "//transaction")
        {
            var transactionViewModel = this.Parent.BindingContext as TransactionPageViewModel;
            transactionViewModel.UpdateGridData();
        }
        else if(currentPage == "//dashboard")
        {
            var transactionViewModel = this.Parent.BindingContext as DashboardPageViewModel;
            transactionViewModel.UpdateDashboardPage();            
        }
    }

    private void OnBudgetCancelClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        viewModel.BudgetDetails.Clear();
        this.createbudgetpopup.IsOpen = false;
    }

    private async void OnBudgetAddClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        if (viewModel.OnAddBudgetClicked())
        {
            this.createbudgetpopup.IsOpen = false;
            await Application.Current.MainPage.DisplayAlert("Success", "Budget created successfully", "OK");
        }
        else
        {
            this.createbudgetpopup.IsOpen = false;
            await Application.Current.MainPage.DisplayAlert("Failed", "Budget creation failed", "OK");
        }
        viewModel.BudgetDetails.Clear();
        var currentPage = Shell.Current.CurrentState.Location.ToString();
        if (currentPage == "//budget")
        {
            var budgetPageViewModel = this.Parent.BindingContext as BudgetPageViewModel;
            budgetPageViewModel.UpdateBudgetData();
        }
        else if (currentPage == "//dashboard")
        {
            var transactionViewModel = this.Parent.BindingContext as DashboardPageViewModel;
            transactionViewModel.UpdateDashboardPage();
        }
    }

    private void OnSavingsCancelClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        viewModel.SavingDetails.Clear();
        this.addsavingspopup.IsOpen = false;
    }

    private async void OnAddSavingsClicked(object sender, System.EventArgs e)
    {
        if (layoutViewModel.OnAddBSavingsClicked().Result)
        {
            this.addsavingspopup.IsOpen = false;
            await Application.Current.MainPage.DisplayAlert("Success", "Added Savings successfully", "OK");
        }
        else
        {
            this.addsavingspopup.IsOpen = false;
            await Application.Current.MainPage.DisplayAlert("Failed", "Adding Savings failed", "OK");
        }
        layoutViewModel.SavingDetails.Clear();
        var currentPage = Shell.Current.CurrentState.Location.ToString();
        if (currentPage == "//savings")
        {
            var savingsPageViewModel = this.Parent.BindingContext as SavingsPageViewModel;
            savingsPageViewModel.UpdateGridData();
            savingsPageViewModel.UpdateCartValue();
        }
        else if (currentPage == "//dashboard")
        {
            var transactionViewModel = this.Parent.BindingContext as DashboardPageViewModel;
            transactionViewModel.UpdateDashboardPage();
        }
    }

    private void OnGoalCancelClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        viewModel.GoalDetails.Clear();
        this.creategoalpopup.IsOpen = false;
    }

    private async void OnCreateGoalClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        if (viewModel.OnCreateGoalClicked())
        {
            this.creategoalpopup.IsOpen = false;
            await Application.Current.MainPage.DisplayAlert("Success", "Goal created successfully", "OK");
        }
        else
        {
            this.creategoalpopup.IsOpen = false;
            await Application.Current.MainPage.DisplayAlert("Failed", "Creating Goal failed", "OK");
        }
        viewModel.GoalDetails.Clear();
        var currentPage = Shell.Current.CurrentState.Location.ToString();
        if (currentPage == "//goal")
        {
            var goalsPageViewModel = this.Parent.BindingContext as GoalsPageViewModel;
            goalsPageViewModel.UpdateGoalsData();
            goalsPageViewModel.UpdateCardValues();
        }
        else if (currentPage == "//dashboard")
        {
            var transactionViewModel = this.Parent.BindingContext as DashboardPageViewModel;
            transactionViewModel.UpdateDashboardPage();
        }
    }

    public async void TriggerEditSavePopup(int transactionId)
    {
        layoutViewModel.TriggerEditSavings(transactionId);
        this.addsavingspopup.HeaderTitle = "Edit Savings";
        this.addsavingspopup.AcceptButtonText = "Save";
        this.addsavingspopup.IsOpen = true;
    }

    public async void TriggerEditTransactionPopup(int transactionId)
    {
        layoutViewModel.TriggerEditTransaction(transactionId);
        this.createtransactionpopup.HeaderTitle = "Edit Transaction";
        this.createtransactionpopup.AcceptButtonText = "Save";
        this.createtransactionpopup.IsOpen = true;
    }

    public async void TriggerEditBudgetPopup(int budgetId)
    {
        layoutViewModel.TriggerEditBudget(budgetId);
        this.createbudgetpopup.HeaderTitle = "Edit Budget";
        this.createbudgetpopup.AcceptButtonText = "Save";
        this.createbudgetpopup.IsOpen = true;
    }

    public async void TriggerAddExpensePopup()
    {
        this.addexpensepopup.IsOpen = true;
        layoutViewModel.TransactionDetails.TransactionType = "Expense";
    }

    public async void TriggerEditGoalPopup(int goalId)
    {
        layoutViewModel.TriggerEditGoal(goalId);
        this.creategoalpopup.HeaderTitle = "Edit Goal";
        this.creategoalpopup.AcceptButtonText = "Save";
        this.creategoalpopup.IsOpen = true;
    }

    public async void TriggerAddFundPopup()
    {
        //this.AddExpensePopup.IsOpen = true;
        //layoutViewModel.TransactionDetails.TransactionType = "Expense";
    }

}
