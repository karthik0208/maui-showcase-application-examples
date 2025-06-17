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
        #region Private Fields

        /// <summary>
        /// Service for user data operations and validation
        /// </summary>
        private readonly UserDataService _userDataService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SignInPageViewModel"/> class.
        /// </summary>
        /// <param name="userDataService">Optional user data service for user validation and data management.</param>
        public SignInPageViewModel(UserDataService? userDataService = null)
        {
            _userDataService = userDataService ?? new UserDataService();

            // Initialize form models
            SignInFormModel = new SignInDataForm();
            AccountInfoDataFormModel = new AccountInfoDataForm();

            // Initialize commands
            OnSignInClickedCommand = new AsyncRelayCommand(OnSignInClicked);
            OnForgotPasswordTappedCommand = new AsyncRelayCommand(OnForgotPasswordTapped);
            OnSignUpTappedCommand = new AsyncRelayCommand(OnSignUpTapped);
            OnGoogleSignInClickedCommand = new RelayCommand(OnGoogleSignInClicked);
            OnMicrosoftSignInClickedCommand = new RelayCommand(OnMicrosoftSignInClicked);
            OnFinishSetupClickedCommand = new AsyncRelayCommand(OnFinishSetupClicked);
        }

        #endregion

        #region Events

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Notifies property changes to the UI.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the sign-in form model containing email and password.
        /// </summary>
        /// <value>SignInDataForm object containing user credentials</value>
        public SignInDataForm SignInFormModel { get; set; }

        /// <summary>
        /// Gets or sets the account information form model for user profile setup.
        /// </summary>
        /// <value>AccountInfoDataForm object containing user profile information</value>
        public AccountInfoDataForm AccountInfoDataFormModel { get; set; }

        #endregion

        #region Commands

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

        #endregion

        #region Command Methods

        /// <summary>
        /// Handles the sign-in process when user clicks the sign-in button
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        private async Task OnSignInClicked()
        {
            try
            {
                var email = SignInFormModel?.Email;
                var password = SignInFormModel?.Password;

                // Validate user credentials
                if (email != null && password != null && _userDataService.ValidateUser(email, password))
                {
                    // Set logged in account
                    _userDataService.LoggedInAccount = email;

                    // Show success message
                    await Application.Current?.Windows.FirstOrDefault()?.Page.DisplayAlert("Sign In", "Sign In Successful", "Okay");

                    // Validate account information and navigate to dashboard
                    var isInfoComplete = _userDataService.ValidateAccountInfo(email);
                    await Shell.Current.GoToAsync("dashboard");
                }
                else
                {
                    // Show error message for invalid credentials
                    await Application.Current?.Windows.FirstOrDefault()?.Page?.DisplayAlert("Sign In failed", "Invalid Username or Password", "Okay");
                }
            }
            catch (Exception ex)
            {
                // Log error for debugging
                System.Diagnostics.Debug.WriteLine($"SignIn Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Navigates to the forgot password page
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        private async Task OnForgotPasswordTapped() =>
            await Shell.Current.GoToAsync("forgotpassword");

        /// <summary>
        /// Navigates to the sign-up page
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        private async Task OnSignUpTapped() =>
            await Shell.Current.GoToAsync("signup");

        /// <summary>
        /// Handles Google sign-in authentication
        /// </summary>
        private void OnGoogleSignInClicked()
        {
            // Future: Add async Google Sign-In logic here
        }

        /// <summary>
        /// Handles Microsoft sign-in authentication
        /// </summary>
        private void OnMicrosoftSignInClicked()
        {
            // Future: Add async Microsoft Sign-In logic here
        }

        /// <summary>
        /// Completes the account setup process with user information
        /// </summary>
        /// <returns>Task representing the asynchronous operation</returns>
        private async Task OnFinishSetupClicked()
        {
            try
            {
                // Create user credentials object with account information
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

                // Update account information
                if (_userDataService.UpdateAccountInfo(user))
                {
                    // Show success message
                    await MainThread.InvokeOnMainThreadAsync(() =>
                        Application.Current?.Windows.FirstOrDefault()?.Page?.DisplayAlert("Account Information", "Successfully updated", "Okay"));

                    // Navigate back to sign-in page
                    await Shell.Current.GoToAsync("signin");
                }
                else
                {
                    // Show error message for incomplete information
                    await MainThread.InvokeOnMainThreadAsync(() =>
                        Application.Current?.Windows.FirstOrDefault()?.Page?.DisplayAlert("Enter Account Information", "Enter all fields", "Okay"));
                }
            }
            catch (Exception ex)
            {
                // Log error for debugging
                System.Diagnostics.Debug.WriteLine($"FinishSetup Error: {ex.Message}");
            }
        }

        #endregion
    }

    /// <summary>
    /// Data model for the Sign-In form containing user credentials.
    /// </summary>
    public class SignInDataForm
    {
        #region Properties

        /// <summary>
        /// Gets or sets the email or username for sign-in.
        /// </summary>
        /// <value>String representing the user's email or username</value>
        [Display(Name = "Email or Username")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password for sign-in.
        /// </summary>
        /// <value>String representing the user's password</value>
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        #endregion
    }

    /// <summary>
    /// Data model for collecting account information after sign-in.
    /// </summary>
    public class AccountInfoDataForm
    {
        #region Properties

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>String representing the user's first name</value>
        [Display(Name = "First Name")]
        [DefaultValue(null)]
        [DataType(DataType.Text)]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>String representing the user's last name</value>
        [Display(Name = "Last Name")]
        [DefaultValue(null)]
        [DataType(DataType.Text)]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>DateTime representing the user's birth date</value>
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; } = DateTime.Today;

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>GenderEnum value representing the user's gender</value>
        [Display(Name = "Gender")]
        [DefaultValue(null)]
        [DataType(DataType.Custom)]
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// Gets or sets the preferred language.
        /// </summary>
        /// <value>LanguageEnum value representing the user's preferred language</value>
        [Display(Name = "Language")]
        [DefaultValue(null)]
        [DataType(DataType.Custom)]
        public LanguageEnum Language { get; set; }

        /// <summary>
        /// Gets or sets the preferred currency.
        /// </summary>
        /// <value>CurrencyEnum value representing the user's preferred currency</value>
        [Display(Name = "Currency")]
        [DefaultValue(null)]
        [DataType(DataType.Custom)]
        public CurrencyEnum Currency { get; set; }

        /// <summary>
        /// Gets or sets the user's time zone.
        /// </summary>
        /// <value>TimeZoneEnum value representing the user's time zone</value>
        [Display(Name = "TimeZone")]
        [DefaultValue(null)]
        [DataType(DataType.Custom)]
        public TimeZoneEnum TimeZone { get; set; }

        #endregion
    }

    #region Enumerations

    /// <summary>
    /// Enumeration for gender values.
    /// </summary>
    public enum GenderEnum
    {
        /// <summary>
        /// Male gender option
        /// </summary>
        Male,

        /// <summary>
        /// Female gender option
        /// </summary>
        Female,

        /// <summary>
        /// Other gender options
        /// </summary>
        Others
    }

    /// <summary>
    /// Enumeration for supported languages.
    /// </summary>
    public enum LanguageEnum
    {
        /// <summary>
        /// English language
        /// </summary>
        English,

        /// <summary>
        /// Spanish language
        /// </summary>
        Spanish,

        /// <summary>
        /// French language
        /// </summary>
        French,

        /// <summary>
        /// German language
        /// </summary>
        German,

        /// <summary>
        /// Chinese language
        /// </summary>
        Chinese,

        /// <summary>
        /// Portuguese language
        /// </summary>
        Portuguese,

        /// <summary>
        /// Russian language
        /// </summary>
        Russian,

        /// <summary>
        /// Japanese language
        /// </summary>
        Japanese,

        /// <summary>
        /// Korean language
        /// </summary>
        Korean
    }

    /// <summary>
    /// Enumeration for supported currencies.
    /// </summary>
    public enum CurrencyEnum
    {
        /// <summary>
        /// US Dollar
        /// </summary>
        USD,

        /// <summary>
        /// Indian Rupee
        /// </summary>
        INR,

        /// <summary>
        /// Euro
        /// </summary>
        EUR,

        /// <summary>
        /// Chinese Yuan
        /// </summary>
        CNY,

        /// <summary>
        /// Russian Ruble
        /// </summary>
        RUB,

        /// <summary>
        /// Japanese Yen
        /// </summary>
        JPY,

        /// <summary>
        /// Korean Won
        /// </summary>
        KRW
    }

    /// <summary>
    /// Enumeration for supported time zones.
    /// </summary>
    public enum TimeZoneEnum
    {
        /// <summary>
        /// UTC-5:00 time zone
        /// </summary>
        [EnumMember(Value = "UTC-5:00")]
        UTC_5_00,

        /// <summary>
        /// UTC-6:00 time zone
        /// </summary>
        [EnumMember(Value = "UTC-6:00")]
        UTC_6_00,

        /// <summary>
        /// UTC-7:00 time zone
        /// </summary>
        [EnumMember(Value = "UTC-7:00")]
        UTC_7_00,

        /// <summary>
        /// UTC-8:00 time zone
        /// </summary>
        [EnumMember(Value = "UTC-8:00")]
        UTC_8_00,

        /// <summary>
        /// UTC-9:00 time zone
        /// </summary>
        [EnumMember(Value = "UTC-9:00")]
        UTC_9_00,

        /// <summary>
        /// UTC-10:00 time zone
        /// </summary>
        [EnumMember(Value = "UTC-10:00")]
        UTC_10_00,

        /// <summary>
        /// Indian Standard Time
        /// </summary>
        [EnumMember(Value = "IST")]
        IST,

        /// <summary>
        /// UTC+1 time zone
        /// </summary>
        [EnumMember(Value = "UTC+1")]
        UTC_1,

        /// <summary>
        /// UTC-6:00 Mexico time zone
        /// </summary>
        [EnumMember(Value = "UTC-6:00")]
        UTC_6_00_Mexico,

        /// <summary>
        /// UTC-3:00 time zone
        /// </summary>
        [EnumMember(Value = "UTC-3:00")]
        UTC_3_00,

        /// <summary>
        /// UTC-5:00 Colombia time zone
        /// </summary>
        [EnumMember(Value = "UTC-5:00")]
        UTC_5_00_Colombia,

        /// <summary>
        /// UTC+1 France time zone
        /// </summary>
        [EnumMember(Value = "UTC+1")]
        UTC_1_France,

        /// <summary>
        /// UTC-4:00 time zone
        /// </summary>
        [EnumMember(Value = "UTC-4:00")]
        UTC_4_00,

        /// <summary>
        /// China Standard Time
        /// </summary>
        [EnumMember(Value = "CST")]
        CST,

        /// <summary>
        /// UTC+0 time zone
        /// </summary>
        [EnumMember(Value = "UTC+0")]
        UTC_0,

        /// <summary>
        /// UTC-3:00 Russia time zone
        /// </summary>
        [EnumMember(Value = "UTC-3:00")]
        UTC_3_00_Russia,

        /// <summary>
        /// UTC+2 time zone
        /// </summary>
        [EnumMember(Value = "UTC+2")]
        UTC_2,

        /// <summary>
        /// UTC+3 time zone
        /// </summary>
        [EnumMember(Value = "UTC+3")]
        UTC_3,

        /// <summary>
        /// UTC+4 time zone
        /// </summary>
        [EnumMember(Value = "UTC+4")]
        UTC_4,

        /// <summary>
        /// UTC+5 time zone
        /// </summary>
        [EnumMember(Value = "UTC+5")]
        UTC_5,

        /// <summary>
        /// UTC+6 time zone
        /// </summary>
        [EnumMember(Value = "UTC+6")]
        UTC_6,

        /// <summary>
        /// UTC+7 time zone
        /// </summary>
        [EnumMember(Value = "UTC+7")]
        UTC_7,

        /// <summary>
        /// UTC+8 time zone
        /// </summary>
        [EnumMember(Value = "UTC+8")]
        UTC_8,

        /// <summary>
        /// UTC+9 Japan time zone
        /// </summary>
        [EnumMember(Value = "UTC+9")]
        UTC_9_Japan,

        /// <summary>
        /// UTC+10 time zone
        /// </summary>
        [EnumMember(Value = "UTC+10")]
        UTC_10,

        /// <summary>
        /// UTC+11 time zone
        /// </summary>
        [EnumMember(Value = "UTC+11")]
        UTC_11,

        /// <summary>
        /// UTC+12 time zone
        /// </summary>
        [EnumMember(Value = "UTC+12")]
        UTC_12,

        /// <summary>
        /// Korea Standard Time
        /// </summary>
        [EnumMember(Value = "KST")]
        KST
    }

    #endregion
}