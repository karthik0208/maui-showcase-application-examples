using MAUIShowcaseSample.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    public partial class SettingsPageViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly UserDataService _userDataService;

        private string currentPassword;

        private string newPassword;

        private string confirmPassword;

        private string currentEmail;

        private string newEmail;

        public SettingsPageViewModel(UserDataService userDataService, DataStore dataStore)
        {
            _userDataService = userDataService;
            ProfileAvatar = "profileavatar.png";

            SettingsValue = new ObservableCollection<Settings>
            {
                new() { SettingsTitle = "Profile", SettingsDescription = "Name, Date of Birth, Gender", Icon = "\ue726" },
                new() { SettingsTitle = "Personalization", SettingsDescription = "Preferred Language, Currency, Time Zone", Icon = "\ue73d"  },
                new() { SettingsTitle = "Appearance", SettingsDescription = "Light and Dark themes", Icon = "\ue72c"  },
                new() { SettingsTitle = "Notification", SettingsDescription = "Customize Notifications", Icon = "\ue725"  },
                new() { SettingsTitle = "Account", SettingsDescription = "Change Email, Password", Icon = "\ue717"  }
            };

            AccountSettingValue = new ObservableCollection<Settings>
            {
                new() {SettingsTitle = "Change Email", SettingsDescription = "Update the email address associated with your account."},
                new() {SettingsTitle = "Change Password", SettingsDescription = "Create a strong password to keep your account secure."},
                new() {SettingsTitle = "Delete Account", SettingsDescription = "Permanently delete your account and data."}
            };
        }

        [ObservableProperty] private ObservableCollection<Settings> settingsValue;
        [ObservableProperty] private ObservableCollection<Settings> accountSettingValue;
        [ObservableProperty] private UserCredentials profileValue;
        [ObservableProperty] private ObservableCollection<string> timeZoneOptions;
        [ObservableProperty] private ObservableCollection<string> languageOptions;
        [ObservableProperty] private ObservableCollection<string> currencyOptions;
        [ObservableProperty] private ObservableCollection<AppThemes> themeOptions;
        [ObservableProperty] private ObservableCollection<GenderEnum> genderOptionsList;
        [ObservableProperty] private string profileAvatar;

       // public ObservableCollection<GenderEnum> GenderOptionsList { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public enum GenderOptions
        //{
        //    Male,
        //    Female
        //}

        public async Task InitializeAsync()
        {

            ProfileValue = await _userDataService.GetLoggedInUserProfile(_userDataService.LoggedInAccount);
            TimeZoneOptions = new ObservableCollection<string>(Enum.GetNames(typeof(TimeZoneEnum)));
            LanguageOptions = new ObservableCollection<string>(Enum.GetNames(typeof(LanguageEnum)));
            CurrencyOptions = new ObservableCollection<string>(Enum.GetNames(typeof(CurrencyEnum)));
            GenderOptionsList = new ObservableCollection<GenderEnum>(Enum.GetValues(typeof(GenderEnum)).Cast<GenderEnum>());
            ThemeOptions = DataHelper.GetAppThemes();

            foreach (var option in ThemeOptions)
            {
                option.IsSelected = option.Theme == ProfileValue.Theme;
                option.PropertyChanged += ThemeOption_PropertyChanged;
            }
        }

        public async Task<bool> UpdateProfileValue()
        {
            ProfileValue = await _userDataService.GetLoggedInUserProfile(_userDataService.LoggedInAccount);
            return true;
        }

        [RelayCommand]
        public async Task<bool> UpdateProfileBasicInfo()
        {
            return _userDataService.UpdateBasicInfo(_userDataService.LoggedInAccount, ProfileValue);
        }

        [RelayCommand]
        public async Task<bool> UpdateProfilePersonalizationInfo()
        {
            return _userDataService.UpdatePersonalizationInfo(_userDataService.LoggedInAccount, ProfileValue);            
        }


        [RelayCommand]
        public async Task<bool> OnEnableNotificationClicked(bool isNotificationEnabled)
        {
            return _userDataService.UpdateNotification(_userDataService.LoggedInAccount, isNotificationEnabled);                         
        }

        [RelayCommand]
        public void UpdateThemeChanges()
        {
            foreach (var option in ThemeOptions.Where(o => o.IsSelected))
            {
                App.Current.UserAppTheme = (option.Theme == "Light" || option.Theme == "System Default")
                    ? AppTheme.Light
                    : AppTheme.Dark;
            }
        }

        [RelayCommand]
        public async Task<bool> UpdateEmail()
        {
            if(_userDataService.LoggedInAccount == this.CurrentEmail)
            {
                return _userDataService.UpdateEmail(this.CurrentEmail, this.NewEmail);
            }           
            return false;
        }

        [RelayCommand]
        public async Task<bool> UpdateProfilePassword()
        {
            if (_userDataService.ValidateUser(_userDataService.LoggedInAccount, this.CurrentPassword))
            {
                return _userDataService.UpdatePassword(_userDataService.LoggedInAccount, this.NewPassword);
            }
            return false;
        }

        [RelayCommand]
        public async Task<bool> DeleteAccount()
        {
            return _userDataService.DeleteAccount(_userDataService.LoggedInAccount);
        }

        private void ThemeOption_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppThemes.IsSelected))
            {
                var selected = sender as AppThemes;
                if (selected?.IsSelected == true)
                {
                    foreach (var option in ThemeOptions.Where(o => o != selected && o.IsSelected))
                    {
                        option.IsSelected = false;
                    }
                }
            }
        }
    }

    public class Settings
    {
        public string SettingsTitle { get; set; }
        public string SettingsDescription { get; set; }
        public string Icon { get; set; }
    }
}
