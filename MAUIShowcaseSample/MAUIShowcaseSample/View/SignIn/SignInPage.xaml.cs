namespace MAUIShowcaseSample.View.SignIn
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage(SignInPageViewModel model)
        {
            InitializeComponent();
            this.BindingContext = model;          
        }
    }    
}

