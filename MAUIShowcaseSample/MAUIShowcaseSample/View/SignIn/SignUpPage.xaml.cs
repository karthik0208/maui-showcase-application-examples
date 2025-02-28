namespace MAUIShowcaseSample.View.SignIn
{
public partial class SignUpPage : ContentPage
{
    public SignUpPage(SignUpPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
}
