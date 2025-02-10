using System.ComponentModel.DataAnnotations;

namespace MAUIShowcaseSample.View.SignIn
{
    public partial class SignInPage : ContentPage
    {
        public SignInPage(SignInPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            ShowSplashScreen();           
        }

        #region Methods

        private async void ShowSplashScreen()
        {
            await Task.Delay(3000); // Show splash for 3 seconds
            SplashView.IsVisible = false;  // Hide splash screen
            SignInView.IsVisible = true;    // Show login page        
        }

        #endregion
    }    
}

