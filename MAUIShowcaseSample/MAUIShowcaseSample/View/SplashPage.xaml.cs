namespace MAUIShowcaseSample.View;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing() // Changed 'private' to 'protected' to match the inherited member's access modifier
    {
        base.OnAppearing();
        await Task.Delay(3000); // Wait for 3 seconds
        await Shell.Current.GoToAsync("signin"); // Replace with your target page
    }
}