using MAUIShowcaseSample.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    /// <summary>
    /// ViewModel for managing settings page operations including profile updates, theme changes, and account management
    /// </summary>
    public partial class SettingsPageViewModel : ObservableObject, INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Service for user data operations
        /// </summary>
        private readonly UserDataService _userDataService;

        /// <summary>
        /// Current password for password change operations
        /// </summary>
        private string currentPassword;

        /// <summary>
        /// New password for password change operations
        /// </summary>
        private string newPassword;

        /// <summary>
        /// Confirmation password for password change operations
        /// </summary>
        private string confirmPassword;

        /// <summary>
        /// Current email address for email change operations
        /// </summary>
        private string currentEmail;

        /// <summary>
        /// New email address for email change operations
        /// </summary>
        private string newEmail;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SettingsPageViewModel class
        /// </summary>
        /// <param name="userDataService">Service for user data operations</param>
        /// <param name="dataStore">Data store service for data operations</param>
        public SettingsPageViewModel(UserDataService userDataService, DataStore dataStore)
        {
            _userDataService = userDataService;
            ProfileAvatar = "profileavatar.png";

            // Initialize main settings options
            SettingsValue = new ObservableCollection<Settings>
            {
                new() { SettingsTitle = "Profile", SettingsDescription = "Name, Date of Birth, Gender", Icon = "\ue726" },
                new() { SettingsTitle = "Personalization", SettingsDescription = "Preferred Language, Currency, Time Zone", Icon = "\ue73d"  },
                new() { SettingsTitle = "Appearance", SettingsDescription = "Light and Dark themes", Icon = "\ue72c"  },
                new() { SettingsTitle = "Notification", SettingsDescription = "Customize Notifications", Icon = "\ue725"  },
                new() { SettingsTitle = "Account", SettingsDescription = "Change Email, Password", Icon = "\ue717"  }
            };

            // Initialize account settings options
            AccountSettingValue = new ObservableCollection<Settings>
            {
                new() {SettingsTitle = "Change Email", SettingsDescription = "Update the email address associated with your account."},
                new() {SettingsTitle = "Change Password", SettingsDescription = "Create a strong password to keep your account secure."},
                new() {SettingsTitle = "Delete Account", SettingsDescription = "Permanently delete your account and data."}
            };
        }

        #endregion

        #region Observable Properties

        /// <summary>
        /// Gets or sets the collection of main settings options
        /// </summary>
        /// <value>Observable collection of Settings objects</value>
        [ObservableProperty] 
        private ObservableCollection<Settings> settingsValue;

        /// <summary>
        /// Gets or sets the collection of account settings options
        /// </summary>
        /// <value>Observable collection of Settings objects for account management</value>
        [ObservableProperty] 
        private ObservableCollection<Settings> accountSettingValue;

        /// <summary>
        /// Gets or sets the user profile information
        /// </summary>
        /// <value>UserCredentials object containing profile data</value>
        [ObservableProperty] 
        private UserCredentials profileValue;

        /// <summary>
        /// Gets or sets the available time zone options
        /// </summary>
        /// <value>Observable collection of time zone strings</value>
        [ObservableProperty] 
        private ObservableCollection<string> timeZoneOptions;

        /// <summary>
        /// Gets or sets the available language options
        /// </summary>
        /// <value>Observable collection of language strings</value>
        [ObservableProperty] 
        private ObservableCollection<string> languageOptions;

        /// <summary>
        /// Gets or sets the available currency options
        /// </summary>
        /// <value>Observable collection of currency strings</value>
        [ObservableProperty] 
        private ObservableCollection<string> currencyOptions;

        /// <summary>
        /// Gets or sets the available theme options
        /// </summary>
        /// <value>Observable collection of AppThemes objects</value>
        [ObservableProperty] 
        private ObservableCollection<AppThemes> themeOptions;

        /// <summary>
        /// Gets or sets the available gender options
        /// </summary>
        /// <value>Observable collection of GenderEnum values</value>
        [ObservableProperty] 
        private ObservableCollection<GenderEnum> genderOptionsList;

        /// <summary>
        /// Gets or sets the profile avatar image path
        /// </summary>
        /// <value>String representing the avatar image path</value>
        [ObservableProperty] 
        private string profileAvatar;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the current password for password change operations
        /// </summary>
        /// <value>String representing the current password</value>
        public string CurrentPassword
        {
            get
            {
                return this.currentPassword;
            }
            set
            {
                this.currentPassword = value;
                OnPropertyChanged("CurrentPassword");
            }
        }

        /// <summary>
        /// Gets or sets the new password for password change operations
        /// </summary>
        /// <value>String representing the new password</value>
        public string NewPassword
        {
            get
            {
                return this.newPassword;
            }
            set
            {
                this.newPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }

        /// <summary>
        /// Gets or sets the confirmation password for password change operations
        /// </summary>
        /// <value>String representing the password confirmation</value>
        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }
            set
            {
                this.confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
            }
        }

        /// <summary>
        /// Gets or sets the current email address for email change operations
        /// </summary>
        /// <value>String representing the current email address</value>
        public string CurrentEmail
        {
            get
            {
                return this.currentEmail;
            }
            set
            {
                this.currentEmail = value;
                OnPropertyChanged("CurrentEmail");
            }
        }

        /// <summary>
        /// Gets or sets the new email address for email change operations
        /// </summary>
        /// <value>String representing the new email address</value>
        public string NewEmail
        {
            get
            {
                return this.newEmail;
            }
            set
            {
                this.newEmail = value;
                OnPropertyChanged("NewEmail");
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the PropertyChanged event for the specified property
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the settings page data asynchronously
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        public async Task InitializeAsync()
        {
            // Load user profile data
            ProfileValue = await _userDataService.GetLoggedInUserProfile(_userDataService.LoggedInAccount);
            
            // Initialize dropdown options from enums
            TimeZoneOptions = new ObservableCollection<string>(Enum.GetNames(typeof(TimeZoneEnum)));
            LanguageOptions = new ObservableCollection<string>(Enum.GetNames(typeof(LanguageEnum)));
            CurrencyOptions = new ObservableCollection<string>(Enum.GetNames(typeof(CurrencyEnum)));
            GenderOptionsList = new ObservableCollection<GenderEnum>(Enum.GetValues(typeof(GenderEnum)).Cast<GenderEnum>());
            ThemeOptions = DataHelper.GetAppThemes();

            // Set current theme selection and subscribe to changes
            foreach (var option in ThemeOptions)
            {
                option.IsSelected = option.Theme == ProfileValue.Theme;
                option.PropertyChanged += ThemeOption_PropertyChanged;
            }
        }

        /// <summary>
        /// Updates the profile value from the data service
        /// </summary>
        /// <returns>True if update is successful, false otherwise</returns>
        public async Task<bool> UpdateProfileValue()
        {
            try
            {
                ProfileValue = await _userDataService.GetLoggedInUserProfile(_userDataService.LoggedInAccount);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Updates the user's basic profile information
        /// </summary>
        /// <returns>True if update is successful, false otherwise</returns>
        [RelayCommand]
        public async Task<bool> UpdateProfileBasicInfo()
        {
            try
            {
                return _userDataService.UpdateBasicInfo(_userDataService.LoggedInAccount, ProfileValue);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the user's personalization settings
        /// </summary>
        /// <returns>True if update is successful, false otherwise</returns>
        [RelayCommand]
        public async Task<bool> UpdateProfilePersonalizationInfo()
        {
            try
            {
                return _userDataService.UpdatePersonalizationInfo(_userDataService.LoggedInAccount, ProfileValue);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Handles notification enable/disable toggle
        /// </summary>
        /// <param name="isNotificationEnabled">Boolean indicating if notifications should be enabled</param>
        /// <returns>True if update is successful, false otherwise</returns>
        [RelayCommand]
        public async Task<bool> OnEnableNotificationClicked(bool isNotificationEnabled)
        {
            try
            {
                return _userDataService.UpdateNotification(_userDataService.LoggedInAccount, isNotificationEnabled);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the application theme based on user selection
        /// </summary>
        [RelayCommand]
        public void UpdateThemeChanges()
        {
            // Apply selected theme to the application
            foreach (var option in ThemeOptions.Where(o => o.IsSelected))
            {
                App.Current.UserAppTheme = (option.Theme == "Light" || option.Theme == "System Default")
                    ? AppTheme.Light
                    : AppTheme.Dark;
            }
        }

        /// <summary>
        /// Updates the user's email address
        /// </summary>
        /// <returns>True if update is successful, false otherwise</returns>
        [RelayCommand]
        public async Task<bool> UpdateEmail()
        {
            try
            {
                // Verify current email matches logged in account
                if (_userDataService.LoggedInAccount == this.CurrentEmail)
                {
                    return _userDataService.UpdateEmail(this.CurrentEmail, this.NewEmail);
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the user's password
        /// </summary>
        /// <returns>True if update is successful, false otherwise</returns>
        [RelayCommand]
        public async Task<bool> UpdateProfilePassword()
        {
            try
            {
                // Validate current password before updating
                if (_userDataService.ValidateUser(_userDataService.LoggedInAccount, this.CurrentPassword))
                {
                    return _userDataService.UpdatePassword(_userDataService.LoggedInAccount, this.NewPassword);
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes the user's account permanently
        /// </summary>
        /// <returns>True if deletion is successful, false otherwise</returns>
        [RelayCommand]
        public async Task<bool> DeleteAccount()
        {
            try
            {
                return _userDataService.DeleteAccount(_userDataService.LoggedInAccount);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles theme option property changes to ensure single selection
        /// </summary>
        /// <param name="sender">The theme option that changed</param>
        /// <param name="e">Property changed event arguments</param>
        private void ThemeOption_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppThemes.IsSelected))
            {
                var selected = sender as AppThemes;
                if (selected?.IsSelected == true)
                {
                    // Deselect all other theme options
                    foreach (var option in ThemeOptions.Where(o => o != selected && o.IsSelected))
                    {
                        option.IsSelected = false;
                    }
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents a settings item with title, description, and icon
    /// </summary>
    public class Settings
    {
        #region Properties

        /// <summary>
        /// Gets or sets the settings title
        /// </summary>
        /// <value>String representing the settings option title</value>
        public string SettingsTitle { get; set; }

        /// <summary>
        /// Gets or sets the settings description
        /// </summary>
        /// <value>String representing the settings option description</value>
        public string SettingsDescription { get; set; }

        /// <summary>
        /// Gets or sets the settings icon
        /// </summary>
        /// <value>String representing the icon character or path</value>
        public string Icon { get; set; }

        #endregion
    }
}