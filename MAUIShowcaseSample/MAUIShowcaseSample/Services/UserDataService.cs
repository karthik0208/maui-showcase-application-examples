using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;

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

        public bool ValidateUser(string email, string password)
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

        public bool UpdatePassword(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            user.Password = password;
            return true;
        }

        public bool AddAccountInfo(UserCredentials userData)
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
            }
            return ValidateAccountInfo(userData.Email);
        }

        public string GetUserCurrencySymbol(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

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
    }

    public class UserCredentials
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DOB { get; set; }

        public string Gender { get; set; }

        public string Language { get; set; }

        public string Currency { get; set; }

        public string TimeZone { get; set; }

    }
}
