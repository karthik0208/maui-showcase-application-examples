using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.Core;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class SettingsChangePassword : ContentPage
{
    SettingsPageViewModel _viewModel;
    UserDataService _userDataService;
    DataStore _dataStore;
    public SettingsChangePassword(SettingsPageViewModel viewModel, UserDataService dataService, DataStore dataStore)
    {
        string pageTitle = "Settings";
        _userDataService = dataService;
        _dataStore = dataStore;
        _viewModel = viewModel;
        _viewModel.InitializeAsync();
        BindingContext = viewModel;
        InitializeComponent();
        var layoutViewModel = new DashboardLayoutPageViewModel(dataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, dataService, dataStore);
    }

    private async void OnDiscardButtonClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///settingspage");
    }

    private async void OnUpdateButtonClicked(object? sender, EventArgs e)
    {
        if (sender is SfButton button1 && button1.BindingContext is SettingsPageViewModel bindingContext && bindingContext.CurrentPassword != null)
        {
            if (bindingContext.NewPassword == bindingContext.ConfirmPassword)
            {
                if (_viewModel.UpdateProfilePassword().Result)
                {
                    _viewModel.UpdateProfileValue();
                    await Application.Current.MainPage.DisplayAlert("Success", "Your password has been updated successfully.", "OK");
                    await Shell.Current.GoToAsync("///signin");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Incorrect Password", "The current password you entered is incorrect.", "OK");
                }

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Password Mismatch", "New password and confirm password do not match.", "OK");
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Missing Password", "Please enter your current password.", "OK");
        }
    }
}