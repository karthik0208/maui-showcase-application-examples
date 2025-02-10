using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace MAUIShowcaseSample
{
    public class SignInPageViewModel
    {

        #region Fields

        private readonly UserDataService _userDataService;

        #endregion

        #region Constructor

        public SignInPageViewModel(UserDataService userDataService) 
        {
            this._userDataService = userDataService;

            this.SignInFormModel = new SignInDataForm();

            this.OnSignInClickedCommand = new Command(OnSignInClicked);

            this.OnForgotPasswordTappedCommand = new Command(OnForgotPasswordTapped);

            this.OnSignUpTappedCommand = new AsyncRelayCommand(OnSignUpTapped);

            this.OnGoogleSignInClickedCommand = new Command(OnGoogleSignInClicked);

            this.OnMicrosoftSignInClickedCommand = new Command(OnMicrosoftSignInClicked);
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

        #endregion

        #region Methods

        private async void OnSignInClicked()
        {
            if (this._userDataService.ValidateUser(this.SignInFormModel.Email, this.SignInFormModel.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Sign In", "Signed In", "Okay");
            }               
        }

        private void OnForgotPasswordTapped()
        {
            // Handle forgot password logic
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
}
