using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Toolkit.Buttons;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class SettingsMobilePage : ContentView
{
    private readonly UserDataService _userDataService;
    private readonly DataStore _dataStore;
    private readonly SettingsPageViewModel _viewModel;
    // Constructor for SettingsMobilePage
    public SettingsMobilePage(UserDataService dataService, DataStore dataStore)
	{
		InitializeComponent();
		var viewModel = new SettingsPageViewModel(dataService, dataStore);
        viewModel.InitializeAsync(); // Initialize the view model
        _viewModel = viewModel;
        BindingContext = viewModel;
        _userDataService = dataService;
        _dataStore = dataStore;
        this.SettingsMobilePopup.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        this.SettingsMobilePopup.VerticalOptions = LayoutOptions.FillAndExpand;
        this.SettingsMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        this.SettingsMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
        this.SettingsMobilePopup.IsOpen = true;
        this.HelpAndSupportMobilePopup.BindingContext = new HelpAndSupportPageViewModel();
    }

    private async void OnEditSettingsClicked(object? sender, EventArgs e)
    {
        if (sender is SfButton button1 && button1.BindingContext is Settings settingsButton1 && settingsButton1.SettingsTitle == "Profile")
        {
            this.ProfileMobilePopup.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            this.ProfileMobilePopup.VerticalOptions = LayoutOptions.FillAndExpand;
            this.ProfileMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            this.ProfileMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
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

            this.ProfileMobilePopup.IsOpen = true;
        }
        else if (sender is SfButton button2 && button2.BindingContext is Settings settingsButton2 && settingsButton2.SettingsTitle == "Personalization")
        {
            this.PersonalizationMobilePopup.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            this.PersonalizationMobilePopup.VerticalOptions = LayoutOptions.FillAndExpand;
            this.PersonalizationMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            this.PersonalizationMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
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
            this.PersonalizationMobilePopup.IsOpen = true;
        }
        else if (sender is SfButton button3 && button3.BindingContext is Settings settingsButton3 && settingsButton3.SettingsTitle == "Appearance")
        {
            this.AppearanceMobilePopup.HeightRequest = 270;
            this.AppearanceMobilePopup.Show(0, (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) - 270);
            this.AppearanceMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            this.AppearanceMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
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
            this.AppearanceMobilePopup.IsOpen = true;
        }
        else if (sender is SfButton button4 && button4.BindingContext is Settings settingsButton4 && settingsButton4.SettingsTitle == "Notification")
        {
            this.NotificationMobilePopup.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            this.NotificationMobilePopup.VerticalOptions = LayoutOptions.FillAndExpand;
            this.NotificationMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            this.NotificationMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
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
            this.NotificationMobilePopup.IsOpen = true;
        }
        else if (sender is SfButton button5 && button5.BindingContext is Settings settingsButton5 && settingsButton5.SettingsTitle == "Account")
        {
            this.AccountMobilePopup.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            this.AccountMobilePopup.VerticalOptions = LayoutOptions.FillAndExpand;
            this.AccountMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            this.AccountMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
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
            this.AccountMobilePopup.IsOpen = true;
        }

    }

    public async void OnHelpAndSupportClicked(object sender, EventArgs e)
    {
        this.HelpAndSupportMobilePopup.IsFullScreen = true;
        this.HelpAndSupportMobilePopup.IsOpen = true;
        this.HelpAndSupportMobilePopup.IsVisible = true;
    }

    public async void OnThemeClicked(object sender, EventArgs e)
    {
        this.AppearanceMobilePopup.HeightRequest = 270;
        this.AppearanceMobilePopup.Show(0, (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) - 270);
        this.AppearanceMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        this.AppearanceMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
        this.AppearanceMobilePopup.IsOpen = true;
    }

    private async void OnHelpAndSupportCancelClicked(object sender, EventArgs e)
    {
        this.HelpAndSupportMobilePopup.IsOpen = false;
        this.HelpAndSupportMobilePopup.IsVisible = false;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        _userDataService.LoggedInAccount = string.Empty;
        await Shell.Current.GoToAsync("/signin");
    }

    private async void OnUpdateAccountClicked(object sender, EventArgs e)
    {
        if (sender is SfButton button && button.CommandParameter == "Change Email")
        {
            this.ChangeEmailMobilePopup.HeightRequest = 373;
            this.ChangeEmailMobilePopup.Show(0, (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) - 373);
            this.ChangeEmailMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            this.ChangeEmailMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
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
            this.ChangeEmailMobilePopup.IsOpen = true;
        }

        else if (sender is SfButton button1 && button1.CommandParameter == "Verfication Code")
        {
            this.ChangeEmailMobilePopup.HeightRequest = 373;
            this.ChangeEmailMobilePopup.Show(0, (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) - 373);
            this.ChangeEmailMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            this.ChangeEmailMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
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
            this.ChangeEmailMobilePopup.IsOpen = true;
        }
        else if (sender is SfButton button2 && button2.CommandParameter == "Change Password")
        {
            this.ChangePasswordMobilePopup.HeightRequest = 420;
            this.ChangePasswordMobilePopup.Show(0, (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) - 420);
            this.ChangePasswordMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            this.ChangePasswordMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
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
            this.ChangePasswordMobilePopup.IsOpen = true;
        }
        else if (sender is SfButton button3 && button3.CommandParameter == "Delete Account")
        {
            this.DeleteAccountMobilePopup.HeightRequest = 300;
            this.DeleteAccountMobilePopup.Show(0, (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) - 300);
            this.DeleteAccountMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            this.DeleteAccountMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
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
            this.DeleteAccountMobilePopup.IsOpen = true;
        }
    }

    private async void OnSettingsCancelClicked(object sender, EventArgs e)
    {
        this.SettingsMobilePopup.IsOpen = false;
        this.SettingsMobilePopup.IsVisible = false;
    }

    private async void OnProfileCancelClicked(object sender, EventArgs e)
    {
        this.ProfileMobilePopup.IsOpen = false;
        this.ProfileMobilePopup.IsVisible = false;
    }

    private async void OnPersonalizationCancelClicked(object sender, EventArgs e)
    {
        this.PersonalizationMobilePopup.IsOpen = false;
        this.PersonalizationMobilePopup.IsVisible = false;
    }

    private async void OnAppearanceCancelClicked(object sender, EventArgs e)
    {
        this.AppearanceMobilePopup.IsOpen = false;
        this.AppearanceMobilePopup.IsVisible = false;
    }
    private async void OnNotificationCancelClicked(object sender, EventArgs e)
    {
        this.NotificationMobilePopup.IsOpen = false;
        this.NotificationMobilePopup.IsVisible = false;
    }

    private async void OnAccountCancelClicked(object sender, EventArgs e)
    {
        this.AccountMobilePopup.IsOpen = false;
        this.AccountMobilePopup.IsVisible = false;
    }

    private async void OnChangeEmailCancelClicked(object sender, EventArgs e)
    {
        this.ChangeEmailMobilePopup.IsOpen = false;
        this.ChangeEmailMobilePopup.IsVisible = false;
    }

    private async void OnChangePasswordCancelClicked(object sender, EventArgs e)
    {
        this.ChangePasswordMobilePopup.IsOpen = false;
        this.ChangePasswordMobilePopup.IsVisible = false;
    }

    private async void OnDeleteAccountCancelClicked(object sender, EventArgs e)
    {
        this.DeleteAccountMobilePopup.IsOpen = false;
        this.DeleteAccountMobilePopup.IsVisible = false;
    }

    private async void OnPasswordUpdatedCancelClicked(object sender, EventArgs e)
    {
        this.PasswordUpdatedMobilePopup.IsOpen = false;
        this.PasswordUpdatedMobilePopup.IsVisible = false;
        await Shell.Current.GoToAsync("/signin");
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        this.PasswordUpdatedMobilePopup.IsOpen = false;
        this.PasswordUpdatedMobilePopup.IsVisible = false;
        await Shell.Current.GoToAsync("/signin");
    }

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        if (sender is SfButton button1 && button1.BindingContext is SettingsPageViewModel bindingContext && bindingContext.CurrentPassword != null)
        {
            if (bindingContext.NewPassword == bindingContext.ConfirmPassword)
            {
                if (_viewModel.UpdateProfilePassword().Result)
                {
                    _viewModel.UpdateProfileValue();
                    this.PasswordUpdatedMobilePopup.HeightRequest = 300;
                    this.PasswordUpdatedMobilePopup.Show(0, (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density) - 300);
                    this.PasswordUpdatedMobilePopup.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
                    this.PasswordUpdatedMobilePopup.HorizontalOptions = LayoutOptions.FillAndExpand;
                    this.PasswordUpdatedMobilePopup.IsOpen = true;
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

    private async void OnDeleteAccountClicked(object sender, EventArgs e)
    {
        _viewModel.DeleteAccount();
       await Shell.Current.GoToAsync("/signin");
    }   

    private async void CalendarIconClicked(object sender, System.EventArgs e)
    {
       // this.datepicker.IsOpen = true;

    }

    private void OKButtonClicked(object sender, System.EventArgs e)
    {
        //this.datepicker.IsOpen = false;
        //this.datepicker.FooterView.ShowOkButton = true;
    }

    private void CancelButtonClicked(object sender, System.EventArgs e)
    {
       // this.datepicker.IsOpen = false;
    }

    private async void OnDiscardButtonClicked(object? sender, EventArgs e)
    {
       // await Shell.Current.GoToAsync("///settings");
    }

    private async void OnUpdateButtonClicked(object? sender, EventArgs e)
    {
        //if (_viewModel.UpdateProfileBasicInfo().Result)
        //{
        //    _viewModel.UpdateProfileValue();
        //    await Application.Current.MainPage.DisplayAlert("Success", "Your basic information has been updated successfully.", "OK");
        //}
        //else
        //{
        //    await Application.Current.MainPage.DisplayAlert("Error", "Failed to update your basic information. Please try again later.", "OK");
        //}
        //await Shell.Current.GoToAsync("///settings");
    }
}
