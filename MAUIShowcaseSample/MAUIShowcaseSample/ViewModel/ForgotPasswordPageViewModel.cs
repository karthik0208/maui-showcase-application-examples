using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel.Communication;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MAUIShowcaseSample
{
    /// <summary>
    /// ViewModel for managing forgot password page operations including email validation, OTP verification, and password reset
    /// </summary>
    public partial class ForgotPasswordPageViewModel : ObservableObject, INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Indicates if the forgot password view is currently visible
        /// </summary>
        private bool isForgotPasswordView;

        /// <summary>
        /// Indicates if the OTP verification view is currently visible
        /// </summary>
        private bool isOTPVerificationView;

        /// <summary>
        /// Indicates if the reset password view is currently visible
        /// </summary>
        private bool isResetPasswordView;

        /// <summary>
        /// OTP value entered by the user for verification
        /// </summary>
        [ObservableProperty]
        private string verifyOtp;

        /// <summary>
        /// Expected OTP value for verification
        /// </summary>
        private string otpValue;

        /// <summary>
        /// Form model for forgot password data
        /// </summary>
        private ForgotPasswordDataForm forgotPasswordFormModel;

        /// <summary>
        /// Form model for reset password data
        /// </summary>
        private ResetPasswordDataForm resetPasswordFormModel;

        /// <summary>
        /// Service for user data operations
        /// </summary>
        private readonly UserDataService _userDataService;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the command to navigate to the next step in the forgot password process.
        /// </summary>
        /// <value>ICommand for proceeding to OTP verification</value>
        public ICommand NextCommand { get; }

        /// <summary>
        /// Gets the command to verify the OTP entered by the user.
        /// </summary>
        /// <value>ICommand for OTP verification</value>
        public ICommand VerifyOtpCommand { get; }

        /// <summary>
        /// Gets the command to reset the user's password after OTP verification.
        /// </summary>
        /// <value>ICommand for password reset</value>
        public ICommand ResetPasswordCommand { get; }

        /// <summary>
        /// Gets the command to navigate to the sign-in page.
        /// </summary>
        /// <value>ICommand for navigation to sign-in</value>
        public ICommand OnSignInTappedCommand { get; }

        /// <summary>
        /// Gets or sets the ForgotPasswordDataForm model.
        /// </summary>
        /// <value>ForgotPasswordDataForm object containing email information</value>
        public ForgotPasswordDataForm ForgotPasswordFormModel 
        {
            get => forgotPasswordFormModel;
            set
            {
                forgotPasswordFormModel = value;
                OnPropertyChanged("ForgotPasswordFormModel");
            }
        }

        /// <summary>
        /// Gets or sets the ResetPasswordDataForm model.
        /// </summary>
        /// <value>ResetPasswordDataForm object containing password reset information</value>
        public ResetPasswordDataForm ResetPasswordFormModel
        {
            get => resetPasswordFormModel;
            set
            {
                resetPasswordFormModel = value;
                OnPropertyChanged("ResetPasswordFormModel");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Forgot Password view is visible.
        /// </summary>
        /// <value>Boolean indicating if the forgot password view is displayed</value>
        public bool IsForgotPasswordView
        {
            get => this.isForgotPasswordView;
            set
            {
                this.isForgotPasswordView = value;
                OnPropertyChanged("IsForgotPasswordView");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the OTP Verification view is visible.
        /// </summary>
        /// <value>Boolean indicating if the OTP verification view is displayed</value>
        public bool IsOTPVerificationView
        {
            get => this.isOTPVerificationView;
            set
            {
                this.isOTPVerificationView = value;
                OnPropertyChanged("IsOTPVerificationView");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Reset Password view is visible.
        /// </summary>
        /// <value>Boolean indicating if the reset password view is displayed</value>
        public bool IsResetPasswordView
        {
            get => this.isResetPasswordView;
            set
            {
                this.isResetPasswordView = value;
                OnPropertyChanged("IsResetPasswordView");
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ForgotPasswordPageViewModel"/> class.
        /// </summary>
        public ForgotPasswordPageViewModel()
        {
            // Initialize view states
            this.IsForgotPasswordView = true; // Start with forgot password view
            this.IsOTPVerificationView = false;
            this.IsResetPasswordView = false;

            // Initialize services and form models
            this._userDataService = App.Services.GetService<UserDataService>();
            this.ForgotPasswordFormModel = new ForgotPasswordDataForm();
            this.resetPasswordFormModel = new ResetPasswordDataForm();

            // Initialize commands
            this.NextCommand = new Command(GoToOtpVerification);
            this.VerifyOtpCommand = new AsyncRelayCommand(GoToResetPassword);
            this.ResetPasswordCommand = new AsyncRelayCommand(ResetPassword);
            this.OnSignInTappedCommand = new AsyncRelayCommand(OnSignInTapped);

            // Set default OTP value for testing
            this.otpValue = "123456";
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Validates the email and navigates to the OTP verification screen if the email exists.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async void GoToOtpVerification()
        {
            // Validate email input
            if (!string.IsNullOrEmpty(this.ForgotPasswordFormModel.Email))
            {                
                // Check if email exists in the system
                if(this._userDataService.ValidateEmail(this.ForgotPasswordFormModel.Email))
                {
                    // Navigate to OTP verification view
                    this.IsForgotPasswordView = false;
                    this.IsOTPVerificationView = true;
                }
                else
                {
                    // Show error for non-existent email
                    await Application.Current.MainPage.DisplayAlert("Otp Verification failed", "Entered Email does not exist", "Okay");
                }
            }
            else
            {
                // Show error for invalid email
                await Application.Current.MainPage.DisplayAlert("Failed", "Enter valid Email", "Okay");
            }
        }

        /// <summary>
        /// Verifies the entered OTP and navigates to the password reset screen if the OTP is correct.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task GoToResetPassword()
        {
            // Validate OTP format
            if (string.IsNullOrWhiteSpace(this.VerifyOtp) || this.VerifyOtp.Length != 6 || !this.VerifyOtp.All(char.IsDigit))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Enter a valid 6-digit OTP!", "OK");
                return;
            }

            // Verify OTP value
            if (this.VerifyOtp == this.otpValue)
            {
                // Navigate to reset password view
                IsOTPVerificationView = false;
                IsResetPasswordView = true;
            }
            else
            {
                // Show error for incorrect OTP
                await Application.Current.MainPage.DisplayAlert("Otp Failed", "Entered OTP does not match", "OK");
            }
        }

        /// <summary>
        /// Resets the user's password after verifying the input.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ResetPassword()
        {
            // Validate password fields are not empty
            if (string.IsNullOrWhiteSpace(ResetPasswordFormModel.Password) ||
                string.IsNullOrWhiteSpace(ResetPasswordFormModel.ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password fields cannot be empty", "OK");
                return;
            }

            // Validate passwords match
            if (!this.ResetPasswordFormModel.Password.Equals(this.ResetPasswordFormModel.ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Password Mismatch", "Passwords do not match", "OK");
                return;
            }

            try
            {
                // Update password in the system
                _userDataService.UpdatePassword(this.ForgotPasswordFormModel.Email, this.ResetPasswordFormModel.Password);
                await Application.Current.MainPage.DisplayAlert("Success", "Your password has been reset!", "OK");
                
                // Navigate back to sign-in page
                await Shell.Current.GoToAsync("signin");
            }
            catch (Exception ex)
            {
                // Handle any errors during password reset
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Redirects the user to the sign-in page.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task OnSignInTapped()
        {
            await Shell.Current.GoToAsync("signin");
        }

        #endregion        
    }

    /// <summary>
    /// Data model for forgot password form containing email information
    /// </summary>
    public class ForgotPasswordDataForm
    {
        #region Properties

        /// <summary>
        /// Gets or sets the email address for password recovery
        /// </summary>
        /// <value>String representing the user's email or username</value>
        [Display(Name = "Email or Username")]
        public string Email { get; set; }

        #endregion
    }  

    /// <summary>
    /// Data model for reset password form containing new password information
    /// </summary>
    public class ResetPasswordDataForm
    {
        #region Properties

        /// <summary>
        /// Gets or sets the new password
        /// </summary>
        /// <value>String representing the new password</value>
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password confirmation
        /// </summary>
        /// <value>String that should match the Password field</value>
        [Display(Name = "ConfirmPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        #endregion
    }
}