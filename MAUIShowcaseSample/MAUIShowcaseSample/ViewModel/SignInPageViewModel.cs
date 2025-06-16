using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MAUIShowcaseSample
{
    /// <summary>
    /// ViewModel for the Sign-In Page, handling user sign-in and account setup logic.
    /// </summary>
    public class SignInPageViewModel : INotifyPropertyChanged
    {
        private readonly UserDataService _userDataService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignInPageViewModel"/> class.
        /// </summary>
        /// <param name="userDataService">Optional user data service for user validation and data management.</param>
        public SignInPageViewModel(UserDataService? userDataService = null)
        {
            _userDataService = userDataService ?? new UserDataService();

            SignInFormModel = new SignInDataForm();
            AccountInfoDataFormModel = new AccountInfoDataForm();

            OnSignInClickedCommand = new AsyncRelayCommand(OnSignInClicked);
            OnForgotPasswordTappedCommand = new AsyncRelayCommand(OnForgotPasswordTapped);
            OnSignUpTappedCommand = new AsyncRelayCommand(OnSignUpTapped);
            OnGoogleSignInClickedCommand = new RelayCommand(OnGoogleSignInClicked);
            OnMicrosoftSignInClickedCommand = new RelayCommand(OnMicrosoftSignInClicked);
            OnFinishSetupClickedCommand = new AsyncRelayCommand(OnFinishSetupClicked);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies property changes to the UI.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        /// <summary>
        /// Gets or sets the sign-in form model containing email and password.
        /// </summary>
        public SignInDataForm SignInFormModel { get; set; }

        /// <summary>
        /// Gets or sets the account information form model for user profile setup.
        /// </summary>
        public AccountInfoDataForm AccountInfoDataFormModel { get; set; }

        /// <summary>
        /// Command executed when the user taps the Sign In button.
        /// </summary>
        public ICommand OnSignInClickedCommand { get; }

        /// <summary>
        /// Command executed when the user taps the Forgot Password link.
        /// </summary>
        public ICommand OnForgotPasswordTappedCommand { get; }

        /// <summary>
        /// Command executed when the user taps the Sign Up link.
        /// </summary>
        public ICommand OnSignUpTappedCommand { get; }

        /// <summary>
        /// Command executed when the user taps the Google Sign-In button.
        /// </summary>
        public ICommand OnGoogleSignInClickedCommand { get; }

        /// <summary>
        /// Command executed when the user taps the Microsoft Sign-In button.
        /// </summary>
        public ICommand OnMicrosoftSignInClickedCommand { get; }

        /// <summary>
        /// Command executed when the user taps the Finish Setup button.
        /// </summary>
        public ICommand OnFinishSetupClickedCommand { get; }

        private async Task OnSignInClicked()
        {
            try
            {
                var email = SignInFormModel?.Email;
                var password = SignInFormModel?.Password;

                if (email != null && password != null && _userDataService.ValidateUser(email, password))
                {
                    _userDataService.LoggedInAccount = email;

                    await Application.Current?.Windows.FirstOrDefault()?.Page.DisplayAlert("Sign In", "Sign In Successful", "Okay");

                    var isInfoComplete = _userDataService.ValidateAccountInfo(email);
                    await Shell.Current.GoToAsync("dashboard");
                }
                else
                {
                    await Application.Current?.Windows.FirstOrDefault()?.Page?.DisplayAlert("Sign In failed", "Invalid Username or Password", "Okay");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SignIn Error: {ex.Message}");
            }
        }

        private async Task OnForgotPasswordTapped() =>
            await Shell.Current.GoToAsync("forgotpassword");

        private async Task OnSignUpTapped() =>
            await Shell.Current.GoToAsync("signup");

        private void OnGoogleSignInClicked()
        {
            // Future: Add async Google Sign-In logic here
        }

        private void OnMicrosoftSignInClicked()
        {
            // Future: Add async Microsoft Sign-In logic here
        }

        private async Task OnFinishSetupClicked()
        {
            try
            {
                var user = new UserCredentials
                {
                    Email = _userDataService.LoggedInAccount,
                    FirstName = AccountInfoDataFormModel.FirstName,
                    LastName = AccountInfoDataFormModel.LastName,
                    DOB = AccountInfoDataFormModel.DOB,
                    Gender = AccountInfoDataFormModel.Gender,
                    Currency = AccountInfoDataFormModel.Currency.ToString(),
                    Language = AccountInfoDataFormModel.Language.ToString(),
                    TimeZone = AccountInfoDataFormModel.TimeZone.ToString()
                };

                if (_userDataService.UpdateAccountInfo(user))
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                        Application.Current?.Windows.FirstOrDefault()?.Page?.DisplayAlert("Account Information", "Successfully updated", "Okay"));

                    await Shell.Current.GoToAsync("signin");
                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                        Application.Current?.Windows.FirstOrDefault()?.Page?.DisplayAlert("Enter Account Information", "Enter all fields", "Okay"));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FinishSetup Error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Data model for the Sign-In form containing user credentials.
    /// </summary>
    public class SignInDataForm
    {
        /// <summary>
        /// Gets or sets the email or username for sign-in.
        /// </summary>
        [Display(Name = "Email or Username")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password for sign-in.
        /// </summary>
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    /// <summary>
    /// Data model for collecting account information after sign-in.
    /// </summary>
    public class AccountInfoDataForm
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Display(Name = "First Name")]
        [DefaultValue(null)]
        [DataType(DataType.Text)]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Display(Name = "Last Name")]
        [DefaultValue(null)]
        [DataType(DataType.Text)]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; } = DateTime.Today;

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        [Display(Name = "Gender")]
        [DefaultValue(null)]
        [DataType(DataType.Custom)]
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// Gets or sets the preferred language.
        /// </summary>
        [Display(Name = "Language")]
        [DefaultValue(null)]
        [DataType(DataType.Custom)]
        public LanguageEnum Language { get; set; }

        /// <summary>
        /// Gets or sets the preferred currency.
        /// </summary>
        [Display(Name = "Currency")]
        [DefaultValue(null)]
        [DataType(DataType.Custom)]
        public CurrencyEnum Currency { get; set; }

        /// <summary>
        /// Gets or sets the user's time zone.
        /// </summary>
        [Display(Name = "TimeZone")]
        [DefaultValue(null)]
        [DataType(DataType.Custom)]
        public TimeZoneEnum TimeZone { get; set; }
    }

    /// <summary>
    /// Enumeration for gender values.
    /// </summary>
    public enum GenderEnum
    {
        Male,
        Female,
        Others
    }

    /// <summary>
    /// Enumeration for supported languages.
    /// </summary>
    public enum LanguageEnum
    {
        English,
        Spanish,
        French,
        German,
        Chinese,
        Portuguese,
        Russian,
        Japanese,
        Korean
    }

    /// <summary>
    /// Enumeration for supported currencies.
    /// </summary>
    public enum CurrencyEnum
    {
        USD,
        INR,
        EUR,
        CNY,
        RUB,
        JPY,
        KRW
    }

    /// <summary>
    /// Enumeration for supported time zones.
    /// </summary>
    public enum TimeZoneEnum
    {
        [EnumMember(Value = "UTC-5:00")]
        UTC_5_00,

        [EnumMember(Value = "UTC-6:00")]
        UTC_6_00,

        [EnumMember(Value = "UTC-7:00")]
        UTC_7_00,

        [EnumMember(Value = "UTC-8:00")]
        UTC_8_00,

        [EnumMember(Value = "UTC-9:00")]
        UTC_9_00,

        [EnumMember(Value = "UTC-10:00")]
        UTC_10_00,

        [EnumMember(Value = "IST")]
        IST,

        [EnumMember(Value = "UTC+1")]
        UTC_1,

        [EnumMember(Value = "UTC-6:00")]
        UTC_6_00_Mexico,

        [EnumMember(Value = "UTC-3:00")]
        UTC_3_00,

        [EnumMember(Value = "UTC-5:00")]
        UTC_5_00_Colombia,

        [EnumMember(Value = "UTC+1")]
        UTC_1_France,

        [EnumMember(Value = "UTC-4:00")]
        UTC_4_00,

        [EnumMember(Value = "CST")]
        CST,

        [EnumMember(Value = "UTC+0")]
        UTC_0,

        [EnumMember(Value = "UTC-3:00")]
        UTC_3_00_Russia,

        [EnumMember(Value = "UTC+2")]
        UTC_2,

        [EnumMember(Value = "UTC+3")]
        UTC_3,

        [EnumMember(Value = "UTC+4")]
        UTC_4,

        [EnumMember(Value = "UTC+5")]
        UTC_5,

        [EnumMember(Value = "UTC+6")]
        UTC_6,

        [EnumMember(Value = "UTC+7")]
        UTC_7,

        [EnumMember(Value = "UTC+8")]
        UTC_8,

        [EnumMember(Value = "UTC+9")]
        UTC_9_Japan,

        [EnumMember(Value = "UTC+10")]
        UTC_10,

        [EnumMember(Value = "UTC+11")]
        UTC_11,

        [EnumMember(Value = "UTC+12")]
        UTC_12,

        [EnumMember(Value = "KST")]
        KST
    }
}
