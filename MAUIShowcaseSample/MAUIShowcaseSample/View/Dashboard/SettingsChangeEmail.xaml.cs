using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Buttons;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class SettingsChangeEmail : ContentPage
{
    SettingsPageViewModel _viewModel;
    UserDataService _userDataService;
    DataStore _dataStore;
    public SettingsChangeEmail(SettingsPageViewModel viewModel, UserDataService dataService, DataStore dataStore)
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
        await Shell.Current.GoToAsync("///settingspage");
        //this.Content = new SettingsPage(_viewModel, _userDataService, _dataStore);
    }

    private async void OnUpdateButtonClicked(object? sender, EventArgs e)
    {
        if (sender is SfButton button1 && button1.BindingContext is SettingsPageViewModel bindingContext && bindingContext.CurrentEmail != null)
        {
            if (bindingContext.NewEmail != null)
            {
                if (_viewModel.UpdateEmail().Result)
                {
                    _viewModel.UpdateProfileValue();
                    await Application.Current.MainPage.DisplayAlert("Success", "Your email has been updated successfully.", "OK");
                    await Shell.Current.GoToAsync("///signin");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Incorrect Email", "The current email you entered is incorrect.", "OK");
                }

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Missing Email", "Please enter your new email.", "OK");
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Missing Email", "Please enter your current email.", "OK");
        }
    }
}