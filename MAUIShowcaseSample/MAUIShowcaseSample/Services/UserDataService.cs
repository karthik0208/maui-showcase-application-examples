using System.Collections.ObjectModel;

namespace MAUIShowcaseSample
{
    public class UserDataService
    {
        private ObservableCollection<UserCredentials> _users = new ObservableCollection<UserCredentials>();

        public UserDataService()
        {
            // 🔹 Set default users at startup
            AddUser("admin@syncfusion.com", "Admin@123");
        }

        public bool AddUser(string email, string password)
        {
            if (!_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                _users.Add(new UserCredentials { Email = email, Password = password });
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
    }

    internal class UserCredentials
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

        public TimeZoneInfo TimeZone { get; set; }

    }
}
