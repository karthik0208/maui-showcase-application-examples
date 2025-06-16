using MAUIShowcaseSample.Services;
using MAUIShowcaseSample.View.Dashboard;
using MAUIShowcaseSample.View.SignIn;
using MAUIShowcaseSample.ViewModel;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;

namespace MAUIShowcaseSample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .ConfigureSyncfusionCore()
                .ConfigureSyncfusionToolkit()
                .UseMauiApp<App>()                

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Roboto-Medium.ttf", "Roboto-Medium");
                    fonts.AddFont("Roboto-Regular.ttf", "Roboto-Regular");
                    fonts.AddFont("Roboto-SemiBold.ttf", "Roboto-SemiBold");
                    fonts.AddFont("ExpenseAnalysis.ttf", "FontIcons");                    
                });

            //builder.ConfigureMauiHandlers(handlers =>
            //{
            //    Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping("SplashScreen", (handler, view) =>
            //    {
            //        // optional customizations
            //    });
            //});

            builder.Services.AddSingleton<UserDataService>();
            // Register DataStore Service
            builder.Services.AddSingleton<DataStore>();

            // Register ViewModels (Transient)
            builder.Services.AddTransient<SignInPageViewModel>();
            builder.Services.AddTransient<SignUpPageViewModel>();
            builder.Services.AddTransient<ForgotPasswordPageViewModel>();
            builder.Services.AddTransient<DashboardPageViewModel>();
            builder.Services.AddTransient<TransactionPageViewModel>();
            builder.Services.AddTransient<BudgetPageViewModel>();
            builder.Services.AddTransient<BudgetDetailPageViewModel>();
            builder.Services.AddTransient<GoalsPageViewModel>();
            builder.Services.AddTransient<GoalDetailPageViewModel>();
            builder.Services.AddTransient<SavingsPageViewModel>();
            //builder.Services.AddTransient<DashboardLayoutPageViewModel>();
            builder.Services.AddTransient<SettingsPageViewModel>();
            builder.Services.AddTransient<HelpAndSupportPageViewModel>();

            // Register Views (Ensures DI works)
            builder.Services.AddTransient<SignInPage>();
            builder.Services.AddTransient<SignUpPage>();            
            builder.Services.AddTransient<ForgotPasswordPage>();
           // builder.Services.AddTransient<DashboardLayoutPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<TransactionMobilePage>();
           // builder.Services.AddTransient<BudgetPage>();
          //  builder.Services.AddTransient<BudgetDetailPage>();
            //builder.Services.AddTransient<BudgetDetailMobilePage>();
            builder.Services.AddTransient<GoalsPage>();
            builder.Services.AddTransient<GoalDetailPage>();
            builder.Services.AddTransient<HelpAndSupportPage>();
            builder.Services.AddTransient<SavingsPage>();
            builder.Services.AddTransient<SettingsAccountPage>();
            builder.Services.AddTransient<SettingsAppearancePage>();
            builder.Services.AddTransient<SettingsChangeEmail>();
            builder.Services.AddTransient<SettingsChangePassword>();
            builder.Services.AddTransient<SettingsNotificationPage>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsPersonalizationPage>();
            builder.Services.AddTransient<SettingsProfilePage>();
            builder.Services.AddTransient<TransactionMobilePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
