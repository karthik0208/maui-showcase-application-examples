using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUIShowcaseSample
{
    public partial class ForgotPasswordPageViewModel : ObservableObject
    {
        [ObservableProperty] private int currentStep = 1; // Step 1: Forgot Password

        [ObservableProperty] private string email;
        [ObservableProperty] private string otp;
        [ObservableProperty] private string newPassword;
        [ObservableProperty] private string confirmPassword;

        public ICommand NextCommand { get; }
        public ICommand VerifyOtpCommand { get; }
        public ICommand ResetPasswordCommand { get; }
        //public string? Email { get; private set; }
        //public string? Otp { get; private set; }
        //public int CurrentStep { get; private set; }
        //public string? NewPassword { get; private set; }
        //public string ConfirmPassword { get; private set; }

        public ForgotPasswordPageViewModel()
        {
            NextCommand = new RelayCommand(GoToOtpVerification);
            VerifyOtpCommand = new RelayCommand(GoToResetPassword);
            ResetPasswordCommand = new RelayCommand(ResetPassword);
        }

        private void GoToOtpVerification()
        {
            if (!string.IsNullOrEmpty(this.Email))
            {
                this.CurrentStep = 2; // Move to OTP Verification
            }
        }

        private void GoToResetPassword()
        {
            if (!string.IsNullOrEmpty(Otp))
            {
                CurrentStep = 3; // Move to Reset Password
            }
        }

        private void ResetPassword()
        {
            if (!string.IsNullOrEmpty(NewPassword) && NewPassword == ConfirmPassword)
            {
                Application.Current.MainPage.DisplayAlert("Success", "Your password has been reset!", "OK");
                CurrentStep = 1; // Go back to login
            }
        }
    }

    public class StepToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int step && parameter is string targetStep)
            {
                return step == int.Parse(targetStep);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
