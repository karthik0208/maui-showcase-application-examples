using MAUIShowcaseSample.Services;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class SettingsProfilePage : ContentPage
{
    SettingsPageViewModel _viewModel;
    UserDataService _userDataService;
    DataStore _dataStore;

    public SettingsProfilePage(UserDataService dataService, DataStore dataStore)
    {
        string pageTitle = "Settings";
        InitializeComponent();
        _userDataService = dataService;
        _dataStore = dataStore;
        _viewModel = new SettingsPageViewModel(dataService, dataStore);
        _viewModel.InitializeAsync();
        BindingContext = _viewModel;
        var layoutViewModel = new DashboardLayoutPageViewModel(dataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, dataService, dataStore);
        this.datepicker.OkButtonClicked += this.OKButtonClicked;
        this.datepicker.CancelButtonClicked += this.CancelButtonClicked;
    }

    private async void CalendarIconClicked(object sender, System.EventArgs e)
    {
        this.datepicker.IsOpen = true;

    }

    private void OKButtonClicked(object sender, System.EventArgs e)
    {
        this.datepicker.IsOpen = false;
        this.datepicker.FooterView.ShowOkButton = true;
    }

    private void CancelButtonClicked(object sender, System.EventArgs e)
    {
        this.datepicker.IsOpen = false;
    }

    private async void OnDiscardButtonClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///settings");
    }

    private async void OnUpdateButtonClicked(object? sender, EventArgs e)
    {
        if (_viewModel.UpdateProfileBasicInfo().Result)
        {
            _viewModel.UpdateProfileValue();
            await Application.Current.MainPage.DisplayAlert("Success", "Your basic information has been updated successfully.", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to update your basic information. Please try again later.", "OK");
        }
        await Shell.Current.GoToAsync("///settings");
    }
}