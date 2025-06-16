using MAUIShowcaseSample.Services;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class SettingsAppearancePage : ContentPage
{
    SettingsPageViewModel _viewModel;
    UserDataService _userDataService;
    DataStore _dataStore;

    public SettingsAppearancePage(SettingsPageViewModel viewModel, UserDataService dataService, DataStore dataStore)
    {
        string pageTitle = "Settings";
        InitializeComponent();
        _userDataService = dataService;
        _dataStore = dataStore;
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
        _viewModel.UpdateThemeChanges();
        _viewModel.UpdateProfileValue();
        await Shell.Current.GoToAsync("///settings");
    }
}