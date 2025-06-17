namespace MAUIShowcaseSample.View.SignIn
{
    /// <summary>
    /// Code-behind for the Sign In page that handles user authentication
    /// and manages the Shell flyout behavior for the authentication flow
    /// </summary>
    public partial class SignInPage : ContentPage
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the SignInPage with dependency injection
        /// </summary>
        /// <param name="model">The SignInPageViewModel instance injected for data binding</param>
        public SignInPage(SignInPageViewModel model)
        {
            // Initialize the page components
            InitializeComponent();
            
            // Disable the Shell flyout menu during sign-in process
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            
            // Set the binding context to the injected view model
            this.BindingContext = model;          
        }

        #endregion
    }    
}