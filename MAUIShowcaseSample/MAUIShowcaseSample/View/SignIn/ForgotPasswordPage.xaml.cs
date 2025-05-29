using MAUIShowcaseSample;
using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Toolkit.OtpInput;

namespace MAUIShowcaseSample.View.SignIn;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage()
	{
		InitializeComponent();
    }

    private void OnValueChanged(object sender, OtpInputValueChangedEventArgs e)
    {
        string cleanedValue = e.NewValue?.Replace("\0", string.Empty);
        if (cleanedValue?.Length == 6)
       {
            this.ContinueButton.IsEnabled = true;
       }
       else
       { 
            this.ContinueButton.IsEnabled = false;
       }
    }
}