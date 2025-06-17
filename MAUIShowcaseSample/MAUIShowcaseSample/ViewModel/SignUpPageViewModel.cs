using System.Windows.Input;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.DataForm;

namespace MAUIShowcaseSample;

/// <summary>
/// ViewModel for managing sign-up page operations including user registration and form validation
/// </summary>
public partial class SignUpPageViewModel : ObservableObject
{
    #region Private Fields

    /// <summary>
    /// Service for user data operations
    /// </summary>
    private readonly UserDataService _userDataService;

    #endregion

    #region Observable Properties

    /// <summary>
    /// Gets or sets whether the terms and conditions checkbox is checked
    /// </summary>
    /// <value>Boolean indicating if terms are accepted</value>
    [ObservableProperty]
    private bool isTermsChecked;

    /// <summary>
    /// Gets or sets the sign-up form data model
    /// </summary>
    /// <value>SignUpDataForm object containing user input data</value>
    [ObservableProperty]
    private SignUpDataForm signUpFormModel;

    #endregion

    #region Commands

    /// <summary>
    /// Command for handling sign-up button click
    /// </summary>
    public ICommand OnSignUpClickedCommand { get; }

    /// <summary>
    /// Command for handling sign-in link tap
    /// </summary>
    public ICommand OnSignInTappedCommand { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the SignUpPageViewModel class
    /// </summary>
    /// <param name="userDataService">Service for user data operations</param>
    public SignUpPageViewModel(UserDataService userDataService)
    {
        _userDataService = userDataService;
        SignUpFormModel = new SignUpDataForm();
        OnSignInTappedCommand = new RelayCommand(OnSignInTapped);
        OnSignUpClickedCommand = new RelayCommand(OnSignUpClicked);
        IsTermsChecked = false;
    }

    #endregion

    #region Command Methods

    /// <summary>
    /// Handles the sign-up button click event
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSignUp))]
    private async void OnSignUpClicked()
    {
        // Validate all required fields are filled
        if (SignUpFormModel.Name != null && SignUpFormModel.Email != null && SignUpFormModel.Password != null && SignUpFormModel.ConfirmPassword != null)
        {
            // Check if passwords match
            if (SignUpFormModel.Password == SignUpFormModel.ConfirmPassword)
            {
                // Attempt to add user to the system
                if (_userDataService.AddUser(SignUpFormModel.Name, SignUpFormModel.Email, SignUpFormModel.Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Signup Alert", "User added successfully", "Okay");
                    await Shell.Current.GoToAsync("///signin");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Sign Up Failed", "User Email already exists", "Okay");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Sign Up Failed", "Password not matching", "Okay");
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Sign Up Failed", "Enter all required fields", "Okay");
        }
    }

    /// <summary>
    /// Handles Google sign-up button click
    /// </summary>
    [RelayCommand]
    private void OnGoogleSignUpClicked()
    {
        // Handle Google sign-in logic
    }

    /// <summary>
    /// Handles Microsoft sign-up button click
    /// </summary>
    [RelayCommand]
    private void OnMicrosoftSignUpClicked()
    {
        // Handle Microsoft sign-in logic
    }

    /// <summary>
    /// Handles terms and conditions link click
    /// </summary>
    [RelayCommand]
    private async void OnTermsClicked()
    {
        await Application.Current.MainPage.DisplayAlert("Terms and Conditions", "Read the instructions", "Okay");
    }

    /// <summary>
    /// Handles sign-in link tap to navigate to sign-in page
    /// </summary>
    private async void OnSignInTapped()
    {
        await Shell.Current.GoToAsync("signin");
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Determines whether the sign-up command can be executed
    /// </summary>
    /// <returns>True if terms are checked, false otherwise</returns>
    private bool CanSignUp()
    {
        try
        {
            return IsTermsChecked;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    #endregion

    #region Property Changed Handlers

    /// <summary>
    /// Handles changes to the IsTermsChecked property
    /// </summary>
    /// <param name="value">New value of the property</param>
    partial void OnIsTermsCheckedChanged(bool value)
    {
        IsTermsChecked = value;
    }

    #endregion
}

/// <summary>
/// Data model for sign-up form with validation attributes
/// </summary>
public class SignUpDataForm
{
    #region Properties

    /// <summary>
    /// Gets or sets the user's name
    /// </summary>
    /// <value>String representing the user's full name</value>
    [Display(Name = "Name")]
    [Required(ErrorMessage = "Please enter your name")]
    [StringLength(20, ErrorMessage = "Name should not exceed 20 characters")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the user's email address
    /// </summary>
    /// <value>String representing a valid email address</value>
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Please enter your email")]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the user's password
    /// </summary>
    /// <value>String representing a secure password meeting complexity requirements</value>
    [Display(Name = "Password")]
    [DataFormDisplayOptions(ColumnSpan = 2, ValidMessage = "Password strength is good")]
    [Required(ErrorMessage = "Please enter the password")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include upper/lowercase letters, a digit, and a special character.")]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the password confirmation
    /// </summary>
    /// <value>String that should match the Password field</value>
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Please enter the password")]
    public string ConfirmPassword { get; set; }

    #endregion
}