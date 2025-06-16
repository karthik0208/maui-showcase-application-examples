using CommunityToolkit.Mvvm.Input;
using MAUIShowcaseSample.View;
using MAUIShowcaseSample.View.Dashboard;
using MAUIShowcaseSample.View.SignIn;
using System.Windows.Input;

namespace MAUIShowcaseSample
{
    public partial class AppShell : Shell
    {
        public ICommand NavigateCommand { get; }

        // Pseudocode plan:  
        // 1. Ensure the initial navigation to the splash screen happens as soon as the AppShell is loaded.  
        // 2. Move the call to InitializeAsync() into the AppShell constructor and make it fire-and-forget.  
        // 3. Optionally, ensure the MainPage in App.xaml.cs is set to AppShell.  

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("splash", typeof(SplashPage));
            Routing.RegisterRoute("signin", typeof(SignInPage));
            Routing.RegisterRoute("signup", typeof(SignUpPage));
            Routing.RegisterRoute("forgotpassword", typeof(ForgotPasswordPage));
            Routing.RegisterRoute("updateaccountinfo", typeof(UpdateAccountInfoPage));
#if WINDOWS

            Routing.RegisterRoute("dashboard", typeof(DashboardPage));
            Routing.RegisterRoute("transaction", typeof(TransactionPage));
            Routing.RegisterRoute("budget", typeof(BudgetPage));
            Routing.RegisterRoute("budgetdetailpage", typeof(BudgetDetailPage));
            Routing.RegisterRoute("savings", typeof(SavingsPage));
            Routing.RegisterRoute("goal", typeof(GoalsPage));
            Routing.RegisterRoute("goaldetailpage", typeof(GoalDetailPage));
            Routing.RegisterRoute("settings", typeof(SettingsPage));
            Routing.RegisterRoute("settingsaccountpage", typeof(SettingsAccountPage));
            Routing.RegisterRoute("settingsappearancepage", typeof(SettingsAppearancePage));
            Routing.RegisterRoute("settingschangeemail", typeof(SettingsChangeEmail));
            Routing.RegisterRoute("settingschangepassword", typeof(SettingsChangePassword));
            Routing.RegisterRoute("settingsnotificationpage", typeof(SettingsNotificationPage));
            Routing.RegisterRoute("settingspersonalizationpage", typeof(SettingsPersonalizationPage));
            Routing.RegisterRoute("settingsprofilepage", typeof(SettingsProfilePage));
            Routing.RegisterRoute("helpandsupport", typeof(HelpAndSupportPage));
#elif ANDROID || IOS               
            Routing.RegisterRoute("dashboard", typeof(DashboardMobilePage));
            Routing.RegisterRoute("transaction", typeof(TransactionMobilePage));
            Routing.RegisterRoute("budget", typeof(BudgetMobilePage));
            //Routing.RegisterRoute("budgetdetailpage", typeof(BudgetDetailMobilePage));
            Routing.RegisterRoute("savings", typeof(SavingsMobilePage));
            Routing.RegisterRoute("goal", typeof(GoalsMobilePage));
            Routing.RegisterRoute("goaldetailpage", typeof(GoalDetailMobilePage));
            //Routing.RegisterRoute("settings", typeof(SettingsPageMobile));
            //Routing.RegisterRoute("settingsaccountpage", typeof(SettingsAccountPageMobile));
            //Routing.RegisterRoute("settingsappearancepage", typeof(SettingsAppearancePageMobile));
            //Routing.RegisterRoute("settingschangeemail", typeof(SettingsChangeEmailMobile));
            //Routing.RegisterRoute("settingschangepassword", typeof(SettingsChangePasswordMobile));
            //Routing.RegisterRoute("settingsnotificationpage", typeof(SettingsNotificationPageMobile));
            //Routing.RegisterRoute("settingspersonalizationpage", typeof(SettingsPersonalizationPageMobile));
            //Routing.RegisterRoute("settingsprofilepage", typeof(SettingsProfilePageMobile));
            //Routing.RegisterRoute("helpandsupport", typeof(HelpAndSupportPageMobile));
#endif

            NavigateCommand = new Command<string>(async (route) =>
            {
                await Shell.Current.GoToAsync(route);
            });

        }
    }
}
