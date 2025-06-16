using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Buttons;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class SettingsAccountPage : ContentPage
{
    UserDataService _userDataService;
    DataStore _dataStore;
    SettingsPageViewModel _viewModel;
    public SettingsAccountPage(SettingsPageViewModel viewModel, UserDataService dataService, DataStore dataStore)
    {
        string pageTitle = "Settings";
        InitializeComponent();
        _viewModel = viewModel;
        _viewModel.InitializeAsync();
        _userDataService = dataService;
        _dataStore = dataStore;
        BindingContext = _viewModel;
        var layoutViewModel = new DashboardLayoutPageViewModel(dataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, dataService, dataStore);
    }

    private async void OnButtonClicked(object? sender, EventArgs e)
    {
        if (sender is SfButton button1 && button1.BindingContext is Settings settingsButton1 && settingsButton1.SettingsTitle == "Change Email")
        {
            await Shell.Current.GoToAsync("///settingschangeemail");
        }
        else if (sender is SfButton button2 && button2.BindingContext is Settings settingsButton2 && settingsButton2.SettingsTitle == "Change Password")
        {
            await Shell.Current.GoToAsync("///settingschangepassword");
        }
        else if (sender is SfButton button3 && button3.BindingContext is Settings settingsButton3 && settingsButton3.SettingsTitle == "Delete Account")
        {
            DeleteAccountPopup.IsOpen = true;
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        DeleteAccountPopup.IsOpen = false;
        if (_viewModel.DeleteAccount().Result)
        {
            await Application.Current.MainPage.DisplayAlert("Success", "Account deleted successfully.", "OK");
            await Shell.Current.GoToAsync("///signin");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete the account. Please try again.", "OK");
            await Shell.Current.GoToAsync("///settings");
        }
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        DeleteAccountPopup.IsOpen = false;
        await Shell.Current.GoToAsync("///settings");
    }
}