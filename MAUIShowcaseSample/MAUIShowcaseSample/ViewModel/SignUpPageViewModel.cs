using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.DataForm;

namespace MAUIShowcaseSample
{
    public partial class SignUpPageViewModel : ObservableObject
    {
        #region Fields

        private readonly UserDataService _userDataService;

        [ObservableProperty]
        private bool isTermsChecked;

        #endregion

        #region Constructor

        public SignUpPageViewModel(UserDataService userDataService)
        {
            this._userDataService = userDataService;

            this.SignUpFormModel = new SignUpDataForm();

            this.OnSignUpClickedCommand = new AsyncRelayCommand(OnSignUpClicked, CanSignUp);

            this.OnGoogleSignUpClickedCommand = new Command(OnGoogleSignUpClicked);

            this.OnMicrosoftSignUpClickedCommand = new Command(OnMicrosoftSignUpClicked);

            this.OnTermsClickedCommand = new Command(OnTermsClicked);

            this.OnSignInTappedCommand = new AsyncRelayCommand(OnSignInTapped);

            this.isTermsChecked = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the SignIn form model.
        /// </summary>
        public SignUpDataForm SignUpFormModel { get; set; }

        public ICommand OnSignUpClickedCommand { get; }

        public ICommand OnGoogleSignUpClickedCommand { get; }

        public ICommand OnMicrosoftSignUpClickedCommand { get; }

        public ICommand OnTermsClickedCommand { get; }

        public ICommand OnSignInTappedCommand { get; }

        #endregion

        #region Methods

        private async Task OnSignUpClicked()
        {
            if (this._userDataService.AddUser(this.SignUpFormModel.Name, this.SignUpFormModel.Email, this.SignUpFormModel.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Signup Alert", "User added successfully", "Okay");
                await Shell.Current.GoToAsync("///signin");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Sign Up Failed", "User Email already exists", "Okay");
            }            
        }

        private bool CanSignUp() => IsTermsChecked; //Enables button only if checked

        // This method is automatically called when IsTermsAccepted changes
        partial void OnIsTermsCheckedChanged(bool value)
        {
            (OnSignUpClickedCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged(); // Refresh Button State
        }

        private void OnGoogleSignUpClicked()
        {
            // Handle Google sign-in logic
        }

        private void OnMicrosoftSignUpClicked()
        {
            // Handle Microsoft sign-in logic
        }

        private async void OnTermsClicked()
        {
            await Application.Current.MainPage.DisplayAlert("Terms and Conditions", "Read the instructions", "Okay");
        }

        private async Task OnSignInTapped()
        {
            await Shell.Current.GoToAsync("///signin");
        }

        #endregion
    }

    public class SignUpDataForm
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter your name")]
        [StringLength(20, ErrorMessage = "Name should not exceed 20 characters")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter your email")]
        public string Email { get; set; }
        

        [Display(Name = "Password")]
        [DataFormDisplayOptions(ColumnSpan = 2, ValidMessage = "Password strength is good")]
        [Required(ErrorMessage = "Please enter the password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])[a-zA-Z\d]{8,}$", ErrorMessage = "A minimum 8-character password should contain a combination of uppercase and lowercase letters.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter the password")]
        public string ConfirmPassword { get; set; }
      
    }
}
