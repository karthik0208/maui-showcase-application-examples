using MAUIShowcaseSample.Services;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class SettingsPersonalizationPage : ContentPage
{
    SettingsPageViewModel _viewModel;
    UserDataService _userDataService;
    DataStore _dataStore;

    public SettingsPersonalizationPage(SettingsPageViewModel viewModel, UserDataService dataService, DataStore dataStore)
    {
        string pageTitle = "Settings";
        InitializeComponent();
        _viewModel = viewModel;
        _viewModel.InitializeAsync();
        BindingContext = _viewModel;
        var layoutViewModel = new DashboardLayoutPageViewModel(dataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, dataService, dataStore);
    }

    private async void OnDiscardButtonClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///settings");
    }

    private async void OnUpdateButtonClicked(object? sender, EventArgs e)
    {
        if (_viewModel.UpdateProfilePersonalizationInfo().Result)
        {
            await _viewModel.UpdateProfileValue();
            await Application.Current.MainPage.DisplayAlert("Success", "Your personalization settings have been saved successfully.", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to update personalization settings. Please try again later.", "OK");
        }
        await Shell.Current.GoToAsync("///settings");
    }

}