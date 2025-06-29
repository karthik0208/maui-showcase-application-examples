using MAUIShowcaseSample.Services;
using MAUIShowcaseSample.View.Dashboard;
using MAUIShowcaseSample.ViewModel;
using Microsoft.Maui.Devices;
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
        this.profilePopup.ShowRelativeToView(this.profileavatar, Syncfusion.Maui.Toolkit.Popup.PopupRelativePosition.AlignBottomRight);
    }

    private void OnViewProfileClicked(object sender, EventArgs e)
    {
        this.settingscontainer.Content = new SettingsMobilePage(_userCredentials, _dataStore);
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        _userCredentials.LoggedInAccount = string.Empty;
        await Shell.Current.GoToAsync("/signin");

    }

    private async void OnHelpAndSupportClicked(object sender, TappedEventArgs e)
    {
        this.settingscontainer.Content = new SettingsMobilePage(_userCredentials, _dataStore);
        ((SettingsMobilePage)this.settingscontainer.Content).OnHelpAndSupportClicked(sender, e);
    }

    private async void OnThemeClicked(object sender, TappedEventArgs e)
    {
        this.settingscontainer.Content = new SettingsMobilePage(_userCredentials, _dataStore);
        ((SettingsMobilePage)this.settingscontainer.Content).OnThemeClicked(sender, e);
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
        this.createtransactionpopup.IsVisible = false;
        this.addexpensepopup.IsVisible = false;
    }

    private async void OnTransactionAddClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        if (viewModel.OnAddTransactionClicked())
        {
            this.createtransactionpopup.IsOpen = false;
            this.addexpensepopup.IsOpen = false;
            this.createtransactionpopup.IsVisible = false;
            this.addexpensepopup.IsVisible = false;
            await Application.Current.MainPage.DisplayAlert("Success", "Transaction added successfully", "OK");
        }
        else
        {
            this.createtransactionpopup.IsOpen = false;
            this.addexpensepopup.IsOpen = false;
            this.createtransactionpopup.IsVisible = false;
            this.addexpensepopup.IsVisible = false;
            await Application.Current.MainPage.DisplayAlert("Failed", "Adding transaction failed", "OK");
        }
        viewModel.TransactionDetails.Clear();
        var currentPage = Shell.Current.CurrentState.Location.ToString();
        if(currentPage == "transaction")
        {
            var transactionViewModel = this.Parent.BindingContext as TransactionPageViewModel;
            transactionViewModel.UpdateGridData();
        }
        else if(currentPage == "dashboard")
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
        this.createbudgetpopup.IsVisible = false;
    }

    private async void OnBudgetAddClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        if (viewModel.OnAddBudgetClicked())
        {
            this.createbudgetpopup.IsOpen = false;
            this.createbudgetpopup.IsVisible = false;
            await Application.Current.MainPage.DisplayAlert("Success", "Budget created successfully", "OK");
        }
        else
        {
            this.createbudgetpopup.IsOpen = false;
            this.createbudgetpopup.IsVisible = false;
            await Application.Current.MainPage.DisplayAlert("Failed", "Budget creation failed", "OK");
        }
        viewModel.BudgetDetails.Clear();
        var currentPage = Shell.Current.CurrentState.Location.ToString();
        if (currentPage == "budget")
        {
            var budgetPageViewModel = this.Parent.BindingContext as BudgetPageViewModel;
            budgetPageViewModel.UpdateBudgetData();
        }
        else if (currentPage == "dashboard")
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
        this.addsavingspopup.IsVisible = false;
    }

    private async void OnAddSavingsClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        if (viewModel.OnAddBSavingsClicked().Result)
        {
            this.addsavingspopup.IsOpen = false;
            this.addsavingspopup.IsVisible = false;
            await Application.Current.MainPage.DisplayAlert("Success", "Added Savings successfully", "OK");
        }
        else
        {
            this.addsavingspopup.IsOpen = false;
            this.addsavingspopup.IsVisible = false;
            await Application.Current.MainPage.DisplayAlert("Failed", "Adding Savings failed", "OK");
        }
        viewModel.SavingDetails.Clear();
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
        this.creategoalpopup.IsVisible = false;
    }

    private async void OnCreateGoalClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        if (viewModel.OnCreateGoalClicked())
        {
            this.creategoalpopup.IsOpen = false;
            this.creategoalpopup.IsVisible = false;
            await Application.Current.MainPage.DisplayAlert("Success", "Goal created successfully", "OK");
        }
        else
        {
            this.creategoalpopup.IsOpen = false;
            this.creategoalpopup.IsVisible = false;
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

    public async void TriggerEditSavePopup(double? transactionId = null)
    {
#if ANDROID
        // Ensure the popup fills the entire window by setting both HeightRequest and VerticalOptions
        this.addsavingspopup.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        this.addsavingspopup.VerticalOptions = LayoutOptions.FillAndExpand;
#endif
        if(transactionId.HasValue)
        {
            layoutViewModel.TriggerEditSavings(transactionId.Value);
            this.addsavingspopup.HeaderTitle = "Edit Savings";
            this.addsavingspopup.AcceptButtonText = "Save";
        }        
        this.addsavingspopup.IsOpen = true;
    }

    public async void TriggerEditTransactionPopup(double? transactionId = null)
    {
#if ANDROID
        // Ensure the popup fills the entire window by setting both HeightRequest and VerticalOptions
        this.createtransactionpopup.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        this.createtransactionpopup.VerticalOptions = LayoutOptions.FillAndExpand;
#endif

        // Ensure the popup is not removed from the visual tree of the parent page
        // and does not set its Parent to null or change the Content of the parent.
        // If using a modal or overlay, ensure it is a child of the current page's visual tree.
        if (transactionId.HasValue)
        {
            layoutViewModel.TriggerEditTransaction(transactionId.Value);
            this.createtransactionpopup.HeaderTitle = "Edit Transaction";
            this.createtransactionpopup.AcceptButtonText = "Save";
        }

        // Make sure IsOpen is set after all properties are set
        this.createtransactionpopup.IsOpen = true;

        // If the parent page vanishes, check your XAML for the popup's placement.
        // The popup should be declared inside the same ContentPage or ContentView as the rest of the UI.
        // If you are dynamically adding/removing the popup, ensure you do not remove the parent content.
    }

    public async void TriggerEditBudgetPopup(double? budgetId = null)
    {
#if ANDROID
        // Ensure the popup fills the entire window by setting both HeightRequest and VerticalOptions
        this.createbudgetpopup.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        this.createbudgetpopup.VerticalOptions = LayoutOptions.FillAndExpand;
#endif
        if(budgetId.HasValue)
        {
            layoutViewModel.TriggerEditBudget(budgetId.Value);
            this.createbudgetpopup.HeaderTitle = "Edit Budget";
            this.createbudgetpopup.AcceptButtonText = "Save";
        }        
        this.createbudgetpopup.IsOpen = true;
    }

    public async void TriggerEditGoalPopup(double? goalId = null)
    {
#if ANDROID
        // Ensure the popup fills the entire window by setting both HeightRequest and VerticalOptions
        this.creategoalpopup.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        this.creategoalpopup.VerticalOptions = LayoutOptions.FillAndExpand;
#endif
        if(goalId.HasValue)
        {
            layoutViewModel.TriggerEditGoal(goalId.Value);
            this.creategoalpopup.HeaderTitle = "Edit Goal";
            this.creategoalpopup.AcceptButtonText = "Save";
        }        
        this.creategoalpopup.IsOpen = true;
    }

    public async void TriggerAddFundPopup(double goalId, string goalDescription)
    {
#if ANDROID
        // Ensure the popup fills the entire window by setting both HeightRequest and VerticalOptions
        this.creategoalpopup.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        this.creategoalpopup.VerticalOptions = LayoutOptions.FillAndExpand;
#endif
        this.addfundpopup.IsOpen = true;
        layoutViewModel.FundDetails.TransactionDescription = goalDescription;
        layoutViewModel.FundDetails.GoalId = goalId.ToString(); // Convert double to string
    }

    public async void OnAddFundClicked(object sender, System.EventArgs e)
    {
        if (layoutViewModel.OnAddFundClicked().Result)
        {
            this.addfundpopup.IsOpen = false;
            this.addfundpopup.IsVisible = false;
            await Application.Current.MainPage.DisplayAlert("Success", "Added fund successfully", "OK");
        }
        else
        {
            this.addfundpopup.IsOpen = false;
            this.addfundpopup.IsVisible = false;
            await Application.Current.MainPage.DisplayAlert("Failed", "Adding fund failed", "OK");
        }
        layoutViewModel.FundDetails.Clear();
    }

    private void OnFundCancelClicked(object sender, System.EventArgs e)
    {
        var viewModel = BindingContext as DashboardLayoutPageViewModel;
        viewModel.FundDetails.Clear();
        this.addfundpopup.IsOpen = false;
        this.addfundpopup.IsVisible = false;
    }

    public async void TriggerAddExpensePopup()
    {
#if ANDROID
        // Ensure the popup fills the entire window by setting both HeightRequest and VerticalOptions
        this.creategoalpopup.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        this.creategoalpopup.VerticalOptions = LayoutOptions.FillAndExpand;
#endif
        layoutViewModel.TransactionDetails.Clear();
        layoutViewModel.TransactionDetails.TransactionType = "Expense";
        this.addexpensepopup.HeaderTitle = "Add Expense";
        this.addexpensepopup.AcceptButtonText = "Add";
        this.addexpensepopup.IsOpen = true;
    }

}
