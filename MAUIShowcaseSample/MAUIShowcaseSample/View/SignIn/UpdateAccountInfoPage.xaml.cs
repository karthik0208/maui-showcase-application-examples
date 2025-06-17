//==============================================================================
// UpdateAccountInfoPage.xaml.cs
// Code-behind file for the Update Account Info page user interface
// Handles page initialization, view model binding, and data form customization
//==============================================================================

using Syncfusion.Maui.DataForm;

namespace MAUIShowcaseSample.View.SignIn
{
    #region Update Account Info Page Class
    /// <summary>
    /// Represents the Update Account Info page for user profile setup functionality.
    /// This partial class serves as the code-behind for UpdateAccountInfoPage.xaml and handles
    /// the initialization of the page, binding to the SignInPageViewModel, and customization
    /// of data form items for user account information collection.
    /// </summary>
    /// <remarks>
    /// This page provides account setup capabilities including:
    /// - Personal information collection (First Name, Last Name, Date of Birth)
    /// - User preference selection (Gender, Language, Currency, TimeZone)
    /// - Dynamic form field generation and customization
    /// - Enum-based dropdown population for selection fields
    /// </remarks>
    public partial class UpdateAccountInfoPage : ContentPage
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the UpdateAccountInfoPage class.
        /// Sets up the page components, establishes data binding with the provided view model,
        /// and configures the data form event handlers for dynamic field generation.
        /// </summary>
        /// <param name="model">
        /// The SignInPageViewModel instance that contains the business logic and data
        /// for the account information setup functionality. This view model handles user input validation,
        /// account data processing, and navigation commands.
        /// </param>
        /// <value>
        /// The model parameter should be a properly initialized SignInPageViewModel
        /// with all necessary dependencies injected and ready for use.
        /// </value>
        public UpdateAccountInfoPage(SignInPageViewModel model)
        {
            // Initialize all XAML-defined components and controls
            InitializeComponent();
            
            // Establish two-way data binding between the view and view model
            // This enables automatic synchronization of data and commands
            BindingContext = model;
            
            // Subscribe to the data form item generation event for custom field configuration
            AccountInfoDataForm.GenerateDataFormItem += OnAutoGeneratingDataFormItem;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the auto-generation of data form items and customizes specific fields
        /// based on their field names. This method configures dropdown items sources
        /// from enum values and sets special properties like column span.
        /// </summary>
        /// <param name="sender">
        /// The source of the event, typically the SfDataForm control that is generating the item.
        /// </param>
        /// <param name="e">
        /// Event arguments containing the DataFormItem being generated and allowing
        /// for customization of its properties and behavior.
        /// </param>
        /// <value>
        /// The e parameter provides access to the DataFormItem being generated,
        /// allowing modification of its properties, ItemsSource, and layout settings.
        /// </value>
        private void OnAutoGeneratingDataFormItem(object sender, GenerateDataFormItemEventArgs e)
        {
            // Configure TimeZone dropdown field
            if (e.DataFormItem.FieldName == "TimeZone" && e.DataFormItem is DataFormComboBoxItem comboBoxItem)
            {
                // Populate dropdown with TimeZone enum values
                comboBoxItem.ItemsSource = Enum.GetNames(typeof(TimeZoneEnum)).ToList();
                // Make TimeZone field span across two columns for better layout
                e.DataFormItem.ColumnSpan = 2;
            }
            // Configure Currency dropdown field
            else if(e.DataFormItem.FieldName == "Currency" && e.DataFormItem is DataFormComboBoxItem comboBoxItem1)
            {
                // Populate dropdown with Currency enum values
                comboBoxItem1.ItemsSource = Enum.GetNames(typeof(CurrencyEnum)).ToList();
            }
            // Configure Gender dropdown field
            else if (e.DataFormItem.FieldName == "Gender" && e.DataFormItem is DataFormComboBoxItem comboBoxItem2)
            {
                // Populate dropdown with Gender enum values
                comboBoxItem2.ItemsSource = Enum.GetNames(typeof(GenderEnum)).ToList();
            }
            // Configure Language dropdown field
            else if (e.DataFormItem.FieldName == "Language" && e.DataFormItem is DataFormComboBoxItem comboBoxItem3)
            {
                // Populate dropdown with Language enum values
                // Note: This should likely use LanguageEnum instead of GenderEnum
                comboBoxItem3.ItemsSource = Enum.GetNames(typeof(GenderEnum)).ToList();
            }
        }
        #endregion
    }
    #endregion
}