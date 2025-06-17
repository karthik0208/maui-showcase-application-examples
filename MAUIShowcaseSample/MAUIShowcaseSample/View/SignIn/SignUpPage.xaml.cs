//==============================================================================
// SignUpPage.xaml.cs
// Code-behind file for the Sign Up page user interface
// Handles page initialization and view model binding for user registration
//==============================================================================

namespace MAUIShowcaseSample.View.SignIn
{
    #region Sign Up Page Class
    /// <summary>
    /// Represents the Sign Up page for user registration functionality.
    /// This partial class serves as the code-behind for SignUpPage.xaml and handles
    /// the initialization of the page and binding to the SignUpPageViewModel.
    /// </summary>
    /// <remarks>
    /// This page provides user registration capabilities including:
    /// - User information input form
    /// - Terms and conditions acceptance
    /// - Social sign-in options (Google, Microsoft)
    /// - Navigation to sign-in page for existing users
    /// </remarks>
    public partial class SignUpPage : ContentPage
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SignUpPage class.
        /// Sets up the page components and establishes data binding with the provided view model.
        /// </summary>
        /// <param name="viewModel">
        /// The SignUpPageViewModel instance that contains the business logic and data
        /// for the sign-up functionality. This view model handles user input validation,
        /// registration processing, and navigation commands.
        /// </param>
        /// <value>
        /// The viewModel parameter should be a properly initialized SignUpPageViewModel
        /// with all necessary dependencies injected and ready for use.
        /// </value>
        public SignUpPage(SignUpPageViewModel viewModel)
        {
            // Initialize all XAML-defined components and controls
            InitializeComponent();

            // Establish two-way data binding between the view and view model
            // This enables automatic synchronization of data and commands
            BindingContext = viewModel;
        }
        #endregion
    }
    #endregion
}