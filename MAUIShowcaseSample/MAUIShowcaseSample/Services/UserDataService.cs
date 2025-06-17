using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    /// <summary>
    /// Service for managing user data including authentication, profile management, and user preferences
    /// </summary>
    public class UserDataService
    {
        #region Private Fields

        /// <summary>
        /// Collection of registered users in the system
        /// </summary>
        private ObservableCollection<UserCredentials> _users = new ObservableCollection<UserCredentials>();

        /// <summary>
        /// Email address of the currently logged-in user
        /// </summary>
        private string loggedInAccount;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the UserDataService class with default admin user
        /// </summary>
        public UserDataService()
        {
            // Set default users at startup
            AddUser("admin","ad", "ad");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the email address of the currently logged-in user
        /// </summary>
        /// <value>The logged-in user's email address</value>
        public string LoggedInAccount
        {
            get
            {
                return this.loggedInAccount;
            }
            set
            {
                this.loggedInAccount = value;
            }
        }

        #endregion

        #region User Management Methods

        /// <summary>
        /// Adds a new user to the system if the email doesn't already exist
        /// </summary>
        /// <param name="username">The username for the new user</param>
        /// <param name="email">The email address for the new user</param>
        /// <param name="password">The password for the new user</param>
        /// <returns>True if user was added successfully, false if email already exists</returns>
        public bool AddUser(string username, string email, string password)
        {
            if (!_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                _users.Add(new UserCredentials 
                { 
                    UserName = username, 
                    Email = email, 
                    Password = password, 
                    Currency = CurrencyEnum.USD.ToString(), 
                    DOB = DateTime.Today, 
                    FirstName = "admin", 
                    LastName = "ad", 
                    Gender = GenderEnum.Male, 
                    IsNotificationEnabled = false, 
                    Language = LanguageEnum.English.ToString(), 
                    Theme = "Light", 
                    TimeZone = TimeZoneEnum.UTC_1.ToString()
                });
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes a user account from the system
        /// </summary>
        /// <param name="userMail">Email address of the user to delete</param>
        /// <returns>True if account was deleted successfully, false otherwise</returns>
        public bool DeleteAccount(string userMail)
        {
            return _users.Remove(_users.FirstOrDefault(u => u.Email.Equals(userMail, StringComparison.OrdinalIgnoreCase)));
        }

        #endregion

        #region Authentication Methods

        /// <summary>
        /// Validates user credentials for login
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="password">User's password</param>
        /// <returns>True if credentials are valid, false otherwise</returns>
        public bool ValidateUser(string? email, string? password)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                return user.Password.Equals(password);
            }
            return false;
        }

        /// <summary>
        /// Validates if an email address exists in the system
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True if email exists, false otherwise</returns>
        public bool ValidateEmail(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Profile Update Methods

        /// <summary>
        /// Updates basic user information (name, gender, date of birth)
        /// </summary>
        /// <param name="email">Email address of the user to update</param>
        /// <param name="profileValue">UserCredentials object containing updated basic information</param>
        /// <returns>True if update was successful, false if user not found</returns>
        public bool UpdateBasicInfo(string email, UserCredentials profileValue)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                user.FirstName = profileValue.FirstName;
                user.LastName = profileValue.LastName;
                user.Gender = profileValue.Gender;
                user.DOB = profileValue.DOB;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates user personalization settings (language, currency, timezone)
        /// </summary>
        /// <param name="email">Email address of the user to update</param>
        /// <param name="profileValue">UserCredentials object containing updated personalization settings</param>
        /// <returns>True if update was successful, false if user not found</returns>
        public bool UpdatePersonalizationInfo(string email, UserCredentials profileValue)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                user.Language = profileValue.Language;
                user.Currency = profileValue.Currency;
                user.TimeZone = profileValue.TimeZone;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates user's password
        /// </summary>
        /// <param name="email">Email address of the user</param>
        /// <param name="password">New password</param>
        /// <returns>True if password was updated successfully</returns>
        public bool UpdatePassword(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            user.Password = password;
            return true;
        }

        /// <summary>
        /// Updates user's email address
        /// </summary>
        /// <param name="currentEmail">Current email address</param>
        /// <param name="newEmail">New email address</param>
        /// <returns>True if email was updated successfully</returns>
        public bool UpdateEmail(string currentEmail, string newEmail)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(currentEmail, StringComparison.OrdinalIgnoreCase));
            user.Email = newEmail;
            return true;
        }

        /// <summary>
        /// Updates user's notification preference
        /// </summary>
        /// <param name="email">Email address of the user</param>
        /// <param name="isNotificationEnabled">Notification preference setting</param>
        /// <returns>True if notification setting was updated successfully</returns>
        public bool UpdateNotification(string email, bool isNotificationEnabled)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            user.IsNotificationEnabled = isNotificationEnabled;
            return true;
        }

        /// <summary>
        /// Updates comprehensive account information for a user
        /// </summary>
        /// <param name="userData">UserCredentials object containing all updated user information</param>
        /// <returns>True if account information was updated and validated successfully</returns>
        public bool UpdateAccountInfo(UserCredentials userData)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(userData.Email, StringComparison.OrdinalIgnoreCase));

            if (user != null)
            {
                user.FirstName = userData.FirstName;
                user.LastName = userData.LastName;
                user.Currency = userData.Currency;
                user.DOB = userData.DOB;
                user.Gender = userData.Gender;
                user.Language = userData.Language;
                user.TimeZone = userData.TimeZone;
                user.IsNotificationEnabled = userData.IsNotificationEnabled;
                user.Theme = userData.Theme;
            }
            return ValidateAccountInfo(userData.Email);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Gets the currency symbol for a user based on their currency preference
        /// </summary>
        /// <param name="email">Email address of the user</param>
        /// <returns>Currency symbol string, defaults to "₹" if user not found or currency not recognized</returns>
        public string GetUserCurrencySymbol(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            string currencySymbol = string.Empty;
            if (user != null)
            {
                var currency = user.Currency;

                switch (currency)
                {
                    case "USD": return "$";
                    case "EUR": return "€";
                    case "CNY": return "¥";
                    case "JPY": return "¥";
                    case "RUB": return "₽";
                    case "KRW": return "₩";
                    default: return "₹";
                }
            }
            return currencySymbol;
        }

        /// <summary>
        /// Validates that all required account information fields are populated for a user
        /// </summary>
        /// <param name="email">Email address of the user to validate</param>
        /// <returns>True if all required fields are populated, false otherwise</returns>
        public bool ValidateAccountInfo(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null) return false;

            var properties = typeof(UserCredentials).GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(user);

                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)) ||
                    (value is DateTime dt && dt == default))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Retrieves the complete profile information for a logged-in user
        /// </summary>
        /// <param name="email">Email address of the user</param>
        /// <returns>UserCredentials object containing the user's profile information</returns>
        public async Task<UserCredentials> GetLoggedInUserProfile(string? email)
        {
            UserCredentials user = new UserCredentials();
            if(email != null)
            {
                var loggedUser = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
                if(loggedUser != null)
                {
                    user.FirstName = loggedUser.FirstName;
                    user.LastName = loggedUser.LastName;
                    user.DOB = loggedUser.DOB;
                    user.Language = loggedUser.Language;
                    user.Gender = loggedUser.Gender;
                    user.Currency = loggedUser.Currency;
                    user.TimeZone = loggedUser.TimeZone;
                    user.Email = loggedUser.Email;
                    user.IsNotificationEnabled = loggedUser.IsNotificationEnabled;
                    user.Theme = loggedUser.Theme;
                }               
            }      
            return user;
        }

        #endregion
    }

    /// <summary>
    /// Represents user credentials and profile information with property change notification support
    /// </summary>
    public class UserCredentials : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Backing field for UserName property
        /// </summary>
        private string? userName;

        /// <summary>
        /// Backing field for Email property
        /// </summary>
        private string? email;

        /// <summary>
        /// Backing field for Password property
        /// </summary>
        private string? password; 

        /// <summary>
        /// Backing field for FirstName property
        /// </summary>
        private string? firstName;

        /// <summary>
        /// Backing field for LastName property
        /// </summary>
        private string? lastName;

        /// <summary>
        /// Backing field for DOB property
        /// </summary>
        private DateTime? dob;

        /// <summary>
        /// Backing field for Gender property
        /// </summary>
        private GenderEnum? gender;

        /// <summary>
        /// Backing field for Language property
        /// </summary>
        private string? language;

        /// <summary>
        /// Backing field for Currency property
        /// </summary>
        private string? currency;

        /// <summary>
        /// Backing field for TimeZone property
        /// </summary>
        private string? timeZone;

        /// <summary>
        /// Backing field for IsNotificationEnabled property
        /// </summary>
        private bool? isNotificationEnabled;

        /// <summary>
        /// Backing field for Theme property
        /// </summary>
        private string? theme;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the username for the user account
        /// </summary>
        /// <value>The username string</value>
        public string? UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        /// <summary>
        /// Gets or sets the email address for the user account
        /// </summary>
        /// <value>The email address string</value>
        public string? Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        /// <summary>
        /// Gets or sets the password for the user account
        /// </summary>
        /// <value>The password string</value>
        public string? Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        /// <summary>
        /// Gets or sets the first name of the user
        /// </summary>
        /// <value>The first name string</value>
        public string? FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        /// <summary>
        /// Gets or sets the last name of the user
        /// </summary>
        /// <value>The last name string</value>
        public string? LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        /// <summary>
        /// Gets or sets the date of birth for the user
        /// </summary>
        /// <value>The date of birth as DateTime</value>
        public DateTime? DOB
        {
            get
            {
                return this.dob;
            }
            set
            {
                this.dob = value;
                OnPropertyChanged(nameof(DOB));
            }
        }

        /// <summary>
        /// Gets or sets the gender of the user
        /// </summary>
        /// <value>The gender as GenderEnum</value>
        public GenderEnum? Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                this.gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        /// <summary>
        /// Gets or sets the preferred language for the user
        /// </summary>
        /// <value>The language preference string</value>
        public string? Language
        {
            get
            {
                return this.language;
            }
            set
            {
                this.language = value;
                OnPropertyChanged(nameof(Language));
            }
        }

        /// <summary>
        /// Gets or sets the preferred currency for the user
        /// </summary>
        /// <value>The currency preference string</value>
        public string? Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = value;
                OnPropertyChanged(nameof(Currency));
            }
        }

        /// <summary>
        /// Gets or sets the preferred timezone for the user
        /// </summary>
        /// <value>The timezone preference string</value>
        public string? TimeZone
        {
            get
            {
                return this.timeZone;
            }
            set
            {
                this.timeZone = value;
                OnPropertyChanged(nameof(TimeZone));
            }
        }

        /// <summary>
        /// Gets or sets whether notifications are enabled for the user
        /// </summary>
        /// <value>True if notifications are enabled, false otherwise</value>
        public bool? IsNotificationEnabled
        {
            get
            {
                return this.isNotificationEnabled;
            }
            set
            {
                this.isNotificationEnabled = value;
                OnPropertyChanged(nameof(IsNotificationEnabled));
            }
        }

        /// <summary>
        /// Gets or sets the preferred theme for the user interface
        /// </summary>
        /// <value>The theme preference string</value>
        public string? Theme
        {
            get
            {
                return this.theme;
            }
            set
            {
                this.theme = value;
                OnPropertyChanged(nameof(Theme));
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

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the UserCredentials class with default values
        /// </summary>
        public UserCredentials()
        {           
            this.IsNotificationEnabled = false;
            this.Theme = DataHelper.GetAppThemes().Where(t => t.IsSelected == true).FirstOrDefault().Theme;
        }

        #endregion
    }
}