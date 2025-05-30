namespace MAUIShowcaseSample.View.SignIn
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage(SignInPageViewModel model)
        {
            InitializeComponent();
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            this.BindingContext = model;          
        }
    }    
}

