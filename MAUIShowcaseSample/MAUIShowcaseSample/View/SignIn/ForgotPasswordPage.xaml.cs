using MAUIShowcaseSample;
using Syncfusion.Maui.DataForm;

namespace MAUIShowcaseSample.View.SignIn;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage()
	{
		InitializeComponent();
    }

    private void OtpTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry currentEntry && e.NewTextValue.Length == 1)
        {
            if (!string.IsNullOrEmpty(currentEntry.Text))
            {
                if(currentEntry.Text != "*")
                {
                    ((ForgotPasswordPageViewModel)this.BindingContext).VerifyOtp += currentEntry.Text;
                }                
                currentEntry.Text = "*"; // Replace with masked character
            }

            if (currentEntry == Otp1) Otp2.Focus();
            else if (currentEntry == Otp2) Otp3.Focus();
            else if (currentEntry == Otp3) Otp4.Focus();
            else if (currentEntry == Otp4) Otp5.Focus();
            else if (currentEntry == Otp5) Otp6.Focus();
            else if (currentEntry == Otp6)
            {
                this.ContinueButton.IsEnabled = true;
            }
        }
    }

}