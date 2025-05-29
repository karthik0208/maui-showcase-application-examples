using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample
{
    public class UserDataService
    {
        private ObservableCollection<UserCredentials> _users = new ObservableCollection<UserCredentials>();
        private string loggedInAccount;

        public UserDataService()
        {
            // 🔹 Set default users at startup
            AddUser("admin","ad", "ad");
        }

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

        public bool AddUser(string username, string email, string password)
        {
            if (!_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                _users.Add(new UserCredentials { UserName = username, Email = email, Password = password });
                return true;
            }
            return false;
        }

        public bool ValidateUser(string? email, string? password)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                return user.Password.Equals(password);
            }
            return false;
        }

        public bool ValidateEmail(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                return true;
            }
            return false;
        }

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

        public bool UpdatePassword(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            user.Password = password;
            return true;
        }

        public bool UpdateEmail(string currentEmail, string newEmail)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(currentEmail, StringComparison.OrdinalIgnoreCase));
            user.Email = newEmail;
            return true;
        }

        public bool UpdateNotification(string email, bool isNotificationEnabled)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            user.IsNotificationEnabled = isNotificationEnabled;
            return true;
        }

        public bool DeleteAccount(string userMail)
        {
            return _users.Remove(_users.FirstOrDefault(u => u.Email.Equals(userMail, StringComparison.OrdinalIgnoreCase)));
        }
        
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
    }

    public class UserCredentials : INotifyPropertyChanged
    {
        private string? userName;

        private string? email;

        private string? password; 

        private string? firstName;

        private string? lastName;

        private DateTime? dob;

        private GenderEnum? gender;

        private string? language;

        private string? currency;

        private string? timeZone;

        private bool? isNotificationEnabled;

        private string? theme;

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserCredentials()
        {           
            this.IsNotificationEnabled = false;
            this.Theme = DataHelper.GetAppThemes().Where(t => t.IsSelected == true).FirstOrDefault().Theme;
        }

    }
     
    

}


