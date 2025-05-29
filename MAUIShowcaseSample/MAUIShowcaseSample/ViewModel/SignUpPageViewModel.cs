using System.Windows.Input;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.DataForm;

namespace MAUIShowcaseSample;

public partial class SignUpPageViewModel : ObservableObject
{
    private readonly UserDataService _userDataService;

    [ObservableProperty]
    private bool isTermsChecked;

    [ObservableProperty]
    private SignUpDataForm signUpFormModel;

    public ICommand OnSignUpClickedCommand { get; }
    public ICommand OnSignInTappedCommand { get; }

    public SignUpPageViewModel(UserDataService userDataService)
    {
        _userDataService = userDataService;
        SignUpFormModel = new SignUpDataForm();
        OnSignInTappedCommand = new RelayCommand(OnSignInTapped);
        OnSignUpClickedCommand = new RelayCommand(OnSignUpClicked);
        IsTermsChecked = false;
    }

    // Command auto-generated: OnSignUpClickedCommand
    [RelayCommand(CanExecute = nameof(CanSignUp))]
    private async void OnSignUpClicked()
    {
        if(SignUpFormModel.Name != null && SignUpFormModel.Email != null && SignUpFormModel.Password != null && SignUpFormModel.ConfirmPassword != null)
        {
            if(SignUpFormModel.Password == SignUpFormModel.ConfirmPassword)
            {
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

    private bool CanSignUp() => IsTermsChecked;

    partial void OnIsTermsCheckedChanged(bool value)
    {
        IsTermsChecked = value;
       // (OnSignUpClickedCommand as AsyncRelayCommand).NotifyCanExecuteChanged();
    }

    // Command auto-generated: OnGoogleSignUpClickedCommand
    [RelayCommand]
    private void OnGoogleSignUpClicked()
    {
        // Handle Google sign-in logic
    }

    // Command auto-generated: OnMicrosoftSignUpClickedCommand
    [RelayCommand]
    private void OnMicrosoftSignUpClicked()
    {
        // Handle Microsoft sign-in logic
    }

    // Command auto-generated: OnTermsClickedCommand
    [RelayCommand]
    private async void OnTermsClicked()
    {
        await Application.Current.MainPage.DisplayAlert("Terms and Conditions", "Read the instructions", "Okay");
    }

    // Command auto-generated: OnSignInTappedCommand
    private async void OnSignInTapped()
    {
        await Shell.Current.GoToAsync("///signin");
    }
}

public class SignUpDataForm
{
    [Display(Name = "Name")]
    [Required(ErrorMessage = "Please enter your name")]
    [StringLength(20, ErrorMessage = "Name should not exceed 20 characters")]
    public string Name { get; set; }

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Please enter your email")]
    public string Email { get; set; }

    [Display(Name = "Password")]
    [DataFormDisplayOptions(ColumnSpan = 2, ValidMessage = "Password strength is good")]
    [Required(ErrorMessage = "Please enter the password")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and include upper/lowercase letters, a digit, and a special character.")]
    public string Password { get; set; }

    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Please enter the password")]
    public string ConfirmPassword { get; set; }
}