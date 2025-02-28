using MAUIShowcaseSample.Services;
using MAUIShowcaseSample.View.Dashboard;
using Syncfusion.Maui.Buttons;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class DashboardLayoutPage : ContentPage
{
    private UserDataService _userCredentials;
    private DataStore _dataStore;

    public DashboardLayoutPage(UserDataService userCredentials, DataStore dataStore)
	{
		InitializeComponent();
        _userCredentials = userCredentials;
        _dataStore = dataStore;
        LoadDashboard(null, null); // Load Dashboard by default
    }

    private void LoadDashboard(object sender, EventArgs e)
    {
        var _viewModel = new DashboardPageViewModel(_userCredentials, _dataStore); // Create ViewModel instance
        this.PageTitle.Text = "Dashboard";
        this.ContentContainer.Content = new DashboardPage(_viewModel);
    }

    private void OnNavigationClicked(object sender, EventArgs e)
    {
        if (((SfButton)sender).Text == "Transaction")
        {
            this.PageTitle.Text = "Transaction";
            var _viewModel = new TransactionPageViewModel(_userCredentials, _dataStore);
            this.ContentContainer.Content = new TransactionPage(_viewModel);
        }   
        else if (((SfButton)sender).Text == "Dashboard")
        {
            this.PageTitle.Text = "Dashboard";
            var _viewModel = new DashboardPageViewModel(_userCredentials, _dataStore); // Create ViewModel instance
            this.ContentContainer.Content = new DashboardPage(_viewModel);
        }
        else if(((SfButton)sender).Text == "Budget")
        {
            this.PageTitle.Text = "Budget";
            var _viewModel = new BudgetPageViewModel(_userCredentials, _dataStore);
            this.ContentContainer.Content = new BudgetPage(_viewModel);
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
}
