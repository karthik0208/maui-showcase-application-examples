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
    public partial class ForgotPasswordPageViewModel : ObservableObject, INotifyPropertyChanged
    {
        #region Fields

        private bool isForgotPasswordView;

        private bool isOTPVerificationView;

        private bool isResetPasswordView;

        [ObservableProperty]
        private string verifyOtp;

        private string otpValue;

        private ForgotPasswordDataForm forgotPasswordFormModel;

        private ResetPasswordDataForm resetPasswordFormModel;

        private readonly UserDataService _userDataService;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command to navigate to the next step in the forgot password process.
        /// </summary>
        public ICommand NextCommand { get; }

        /// <summary>
        /// Gets the command to verify the OTP entered by the user.
        /// </summary>
        public ICommand VerifyOtpCommand { get; }

        /// <summary>
        /// Gets the command to reset the user's password after OTP verification.
        /// </summary>
        public ICommand ResetPasswordCommand { get; }

        /// <summary>
        /// Gets the command to navigate to the sign-in page.
        /// </summary>
        public ICommand OnSignInTappedCommand { get; }

        /// <summary>
        /// Gets or sets the ForgotPasswordDataForm model.
        /// </summary>
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
            this.IsForgotPasswordView = true; // Enables ForgotPassword
            this.IsOTPVerificationView = false;
            this.IsResetPasswordView = false;
            this._userDataService = App.Services.GetService<UserDataService>();
            this.ForgotPasswordFormModel = new ForgotPasswordDataForm();
            this.resetPasswordFormModel = new ResetPasswordDataForm();
            this.NextCommand = new Command(GoToOtpVerification);
            this.VerifyOtpCommand = new AsyncRelayCommand(GoToResetPassword);
            this.ResetPasswordCommand = new AsyncRelayCommand(ResetPassword);
            this.OnSignInTappedCommand = new AsyncRelayCommand(OnSignInTapped);
            this.otpValue = "123456";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the email and navigates to the OTP verification screen if the email exists.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async void GoToOtpVerification()
        {
            if (!string.IsNullOrEmpty(this.ForgotPasswordFormModel.Email))
            {                
                if(this._userDataService.ValidateEmail(this.ForgotPasswordFormModel.Email))
                {
                    this.IsForgotPasswordView = false;
                    this.IsOTPVerificationView = true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Otp Verification failed", "Entered Email does not exist", "Okay");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "Enter valid Email", "Okay");
            }
        }

        /// <summary>
        /// Verifies the entered OTP and navigates to the password reset screen if the OTP is correct.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task GoToResetPassword()
        {
            if (string.IsNullOrWhiteSpace(this.VerifyOtp) || this.VerifyOtp.Length != 6 || !this.VerifyOtp.All(char.IsDigit))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Enter a valid 6-digit OTP!", "OK");
                return;
            }

            if (this.VerifyOtp == this.otpValue)
            {
                IsOTPVerificationView = false;
                IsResetPasswordView = true;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Otp Failed", "Entered OTP does not match", "OK");
            }
        }

        /// <summary>
        /// Resets the user's password after verifying the input.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ResetPassword()
        {
            if (string.IsNullOrWhiteSpace(ResetPasswordFormModel.Password) ||
                string.IsNullOrWhiteSpace(ResetPasswordFormModel.ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password fields cannot be empty", "OK");
                return;
            }

            if (!this.ResetPasswordFormModel.Password.Equals(this.ResetPasswordFormModel.ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Password Mismatch", "Passwords do not match", "OK");
                return;
            }

            try
            {
                _userDataService.UpdatePassword(this.ForgotPasswordFormModel.Email, this.ResetPasswordFormModel.Password);
                await Application.Current.MainPage.DisplayAlert("Success", "Your password has been reset!", "OK");
                await Shell.Current.GoToAsync("///signin");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Redirects the user to the sign-in page.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task OnSignInTapped()
        {
            await Shell.Current.GoToAsync("///signin");
        }

        #endregion        
    }

    public class ForgotPasswordDataForm
    {
        [Display(Name = "Email or Username")]
        public string Email { get; set; }
    }  

    public class ResetPasswordDataForm
    {
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

}
