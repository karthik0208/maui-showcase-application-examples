using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using Syncfusion.Maui.DataForm;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace MAUIShowcaseSample
{
    public class SignInPageViewModel
    {

        #region Fields

        private readonly UserDataService _userDataService;

        private string Email;

        #endregion

        #region Constructor

        public SignInPageViewModel() 
        {
            this.SignInFormModel = new SignInDataForm();

            this.OnSignInClickedCommand = new Command(OnSignInClicked);

            this.OnForgotPasswordTappedCommand = new AsyncRelayCommand(OnForgotPasswordTapped);

            this.OnSignUpTappedCommand = new AsyncRelayCommand(OnSignUpTapped);

            this.OnGoogleSignInClickedCommand = new Command(OnGoogleSignInClicked);

            this.OnMicrosoftSignInClickedCommand = new Command(OnMicrosoftSignInClicked);

            this.AccountInfoDataFormModel = new AccountInfoDataForm();

            this.OnFinishSetupClickedCommand = new Command(OnFinishSetupClicked);
        }

        public SignInPageViewModel(UserDataService userDataService)
        {
            this._userDataService = userDataService;

            this.SignInFormModel = new SignInDataForm();

            this.AccountInfoDataFormModel = new AccountInfoDataForm();

            this.OnSignInClickedCommand = new Command(OnSignInClicked);

            this.OnForgotPasswordTappedCommand = new AsyncRelayCommand(OnForgotPasswordTapped);

            this.OnSignUpTappedCommand = new AsyncRelayCommand(OnSignUpTapped);

            this.OnGoogleSignInClickedCommand = new Command(OnGoogleSignInClicked);

            this.OnMicrosoftSignInClickedCommand = new Command(OnMicrosoftSignInClicked);           

            this.OnFinishSetupClickedCommand = new Command(OnFinishSetupClicked);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the SignIn form model.
        /// </summary>
        public SignInDataForm SignInFormModel { get; set; }

        public ICommand OnSignInClickedCommand { get; }

        public ICommand OnForgotPasswordTappedCommand { get; }

        public ICommand OnSignUpTappedCommand { get; }

        public ICommand OnGoogleSignInClickedCommand { get; }        

        public ICommand OnMicrosoftSignInClickedCommand { get; }

        public AccountInfoDataForm AccountInfoDataFormModel { get; set; }

        public ICommand OnFinishSetupClickedCommand { get; }
        #endregion

        #region Methods

        private async void OnSignInClicked()
        {
            if (this._userDataService.ValidateUser(this.SignInFormModel.Email, this.SignInFormModel.Password))
            {
                this._userDataService.LoggedInAccount = this.SignInFormModel.Email;
                await Application.Current.MainPage.DisplayAlert("Sign In", "Sign In Successful", "Okay");
                if(!this._userDataService.ValidateAccountInfo(this._userDataService.LoggedInAccount))
                {
                    //await Shell.Current.GoToAsync("///updateaccountinfo");
                    await Shell.Current.GoToAsync("///dashboardlayout");
                }
                else
                {
                    await Shell.Current.GoToAsync("///dashboardlayout");
                }                
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Sign In failed", "Invalid Username or Password", "Okay");
            }
        }

        private async Task OnForgotPasswordTapped()
        {
            await Shell.Current.GoToAsync("///forgotpassword");
        }

        private async Task OnSignUpTapped()
        {
            await Shell.Current.GoToAsync("///signup");
        }

        private void OnGoogleSignInClicked()
        {
            // Handle Google sign-in logic
        }

        private void OnMicrosoftSignInClicked()
        {
            // Handle Microsoft sign-in logic
        }

        private async void OnFinishSetupClicked()
        {            
            UserCredentials userCredentials = new UserCredentials();

            userCredentials.Email = this._userDataService.LoggedInAccount;
            userCredentials.FirstName = this.AccountInfoDataFormModel.FirstName;
            userCredentials.LastName = this.AccountInfoDataFormModel.LastName;
            userCredentials.DOB = this.AccountInfoDataFormModel.DOB;
            userCredentials.Gender = this.AccountInfoDataFormModel.Gender.ToString();
            userCredentials.Currency = this.AccountInfoDataFormModel.Currency.ToString();
            userCredentials.Language = this.AccountInfoDataFormModel.Language.ToString();
            userCredentials.TimeZone = this.AccountInfoDataFormModel.TimeZone.ToString();

            if(this._userDataService.AddAccountInfo(userCredentials))
            {
                await Application.Current.MainPage.DisplayAlert("Account Information", "Successfully updated Account information", "Okay");
                await Shell.Current.GoToAsync("///signin");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Enter Account Information", "Enter all fields", "Okay");
            }            

        }
        #endregion
    }

    public class SignInDataForm
    {
        [Display(Name = "Email or Username")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class AccountInfoDataForm
    {
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Display(Name = "Gender")]
        [DefaultValue(null)]
        public GenderEnum Gender { get; set; }

        [Display(Name = "Language")]
        [DefaultValue(null)]
        public LanguageEnum Language { get; set; }
        
        [Display(Name = "Currency")]
        [DefaultValue(null)]
        public CurrencyEnum Currency { get; set; }

        [Display(Name = "TimeZone")]
        [DefaultValue(null)]
        public TimeZoneEnum TimeZone { get; set; }
    }

    public enum GenderEnum
    {
        Male,
        Female,
        Others
    }

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
