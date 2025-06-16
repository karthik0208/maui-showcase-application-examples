using MAUIShowcaseSample.Services;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class SettingsNotificationPage : ContentPage
{
    SettingsPageViewModel _viewModel;
    UserDataService _userDataService;
    DataStore _dataStore;

    public SettingsNotificationPage(SettingsPageViewModel viewModel, UserDataService dataService, DataStore dataStore)
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

    private async void OnDiscardButtonClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///settingspage");
    }

    private async void OnUpdateButtonClicked(object? sender, EventArgs e)
    {
        if (_viewModel.OnEnableNotificationClicked(sfSwitch.IsOn.Value).Result)
        {
            _viewModel.UpdateProfileValue();
            await Application.Current.MainPage.DisplayAlert("Success", "Notification enabled successfully.", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to enable notifications.", "OK");
        }
        await Shell.Current.GoToAsync("///settings");
    }
}