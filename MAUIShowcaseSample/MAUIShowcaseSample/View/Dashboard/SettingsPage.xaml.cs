using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Toolkit.Buttons;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class SettingsPage : ContentPage
{
    SettingsPageViewModel _viewModel;
    UserDataService _userDataService;
    DataStore _dataStore;

    public SettingsPage(SettingsPageViewModel viewModel, UserDataService dataService, DataStore dataStore)
    {
        string pageTitle = "Settings";
        _userDataService = dataService;
        _dataStore = dataStore;
        _viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();
        var layoutViewModel = new DashboardLayoutPageViewModel(dataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, dataService, dataStore);
    }

    private async void OnButtonClicked(object? sender, EventArgs e)
    {
        if (sender is SfButton button1 && button1.BindingContext is Settings settingsButton1 && settingsButton1.SettingsTitle == "Profile")
        {
            //// Wait for the UI to stabilize (important for Windows)
            //await Task.Delay(100);
            //Dispatcher.Dispatch(() =>
            //{
            //    button1.Text = "\ue701"; // fallback rendering - reapply icon
            //    button1.FontFamily = "FontIcons"; // make sure this matches your font alias
            //});
            //var _viewModel = new SettingsPageViewModel(_userDataService, _dataStore);
            //await _viewModel.InitializeAsync();  // Wait for data to load

            //this.Content = new SettingsProfilePage(_viewModel, _userDataService, _dataStore);

            await Shell.Current.GoToAsync("///settingsprofilepage");
        }
        else if (sender is SfButton button2 && button2.BindingContext is Settings settingsButton2 && settingsButton2.SettingsTitle == "Personalization")
        {
            //// Wait for the UI to stabilize (important for Windows)
            //await Task.Delay(100);
            //Dispatcher.Dispatch(() =>
            //{
            //    button2.Text = "\ue701"; // fallback rendering - reapply icon
            //    button2.FontFamily = "FontIcons"; // make sure this matches your font alias
            //});
            //var _viewModel = new SettingsPageViewModel(_userDataService, _dataStore);
            //await _viewModel.InitializeAsync();  // Wait for data to load
            // this.Content = new SettingsPersonalizationPage(_viewModel);
            await Shell.Current.GoToAsync("///settingspersonalizationpage");
        }
        else if (sender is SfButton button3 && button3.BindingContext is Settings settingsButton3 && settingsButton3.SettingsTitle == "Appearance")
        {
            //// Wait for the UI to stabilize (important for Windows)
            //await Task.Delay(100);
            //Dispatcher.Dispatch(() =>
            //{
            //    button3.Text = "\ue701"; // fallback rendering - reapply icon
            //    button3.FontFamily = "FontIcons"; // make sure this matches your font alias
            //});
            //var _viewModel = new SettingsPageViewModel(_userDataService, _dataStore);
            //await _viewModel.InitializeAsync();  // Wait for data to load
            //this.Content = new SettingsAppearancePage(_viewModel, _userDataService, _dataStore);
            await Shell.Current.GoToAsync("///settingsappearancepage");
        }
        else if (sender is SfButton button4 && button4.BindingContext is Settings settingsButton4 && settingsButton4.SettingsTitle == "Notification")
        {
            //// Wait for the UI to stabilize (important for Windows)
            //await Task.Delay(100);
            //Dispatcher.Dispatch(() =>
            //{
            //    button4.Text = "\eE701"; // fallback rendering - reapply icon
            //    button4.FontFamily = "FontIcons"; // make sure this matches your font alias
            //});
            //var _viewModel = new SettingsPageViewModel(_userDataService, _dataStore);
            //await _viewModel.InitializeAsync();  // Wait for data to load
            //this.Content = new SettingsNotificationPage(_viewModel, _userDataService, _dataStore);
            await Shell.Current.GoToAsync("///settingsnotificationpage");
        }
        else if (sender is SfButton button5 && button5.BindingContext is Settings settingsButton5 && settingsButton5.SettingsTitle == "Account")
        {
            //// Wait for the UI to stabilize (important for Windows)
            //await Task.Delay(100);
            //Dispatcher.Dispatch(() =>
            //{
            //    button5.Text = "\ue701"; // fallback rendering - reapply icon
            //    button5.FontFamily = "FontIcons"; // make sure this matches your font alias
            //});
            //var _viewModel = new SettingsPageViewModel(_userDataService, _dataStore);
            //await _viewModel.InitializeAsync();  // Wait for data to load
            //this.Content = new SettingsAccountPage(_viewModel, _userDataService, _dataStore);
            await Shell.Current.GoToAsync("///settingsaccountpage");
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is SettingsPageViewModel vm)
        {
            _ = vm.InitializeAsync(); // Fire and forget
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        _userDataService.LoggedInAccount = string.Empty;
        await Shell.Current.GoToAsync("///signin");
    }
}