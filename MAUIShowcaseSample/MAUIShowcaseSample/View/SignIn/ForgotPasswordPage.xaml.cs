using MAUIShowcaseSample;
using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Toolkit.OtpInput;

namespace MAUIShowcaseSample.View.SignIn;

/// <summary>
/// Code-behind for the Forgot Password page that handles the password reset flow
/// including email input, OTP verification, and new password creation
/// </summary>
public partial class ForgotPasswordPage : ContentPage
{
    #region Constructor

    /// <summary>
    /// Initializes a new instance of the ForgotPasswordPage
    /// </summary>
    public ForgotPasswordPage()
    {
        InitializeComponent();
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handles the OTP input value changed event to enable/disable the Continue button
    /// based on whether a complete 6-digit OTP has been entered
    /// </summary>
    /// <param name="sender">The OTP input control that triggered the event</param>
    /// <param name="e">Event arguments containing the new OTP value</param>
    private void OnValueChanged(object sender, OtpInputValueChangedEventArgs e)
    {
        // Remove null characters from the OTP input value
        string cleanedValue = e.NewValue?.Replace("\0", string.Empty);
        
        // Enable Continue button only when a complete 6-digit OTP is entered
        if (cleanedValue?.Length == 6)
        {
            this.ContinueButton.IsEnabled = true;
        }
        else
        {
            this.ContinueButton.IsEnabled = false;
        }
    }

    #endregion
}