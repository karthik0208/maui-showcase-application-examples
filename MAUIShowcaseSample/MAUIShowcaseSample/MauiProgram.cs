﻿using MAUIShowcaseSample.Services;
using MAUIShowcaseSample.View.Dashboard;
using MAUIShowcaseSample.View.SignIn;
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
                    fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                    fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                    fonts.AddFont("ExpenseAnalysis.ttf", "FontIcons");
                });

            builder.Services.AddSingleton<UserDataService>();
            // Register DataStore Service
            builder.Services.AddSingleton<DataStore>();

            // Register ViewModels (Transient)
            builder.Services.AddTransient<SignInPageViewModel>();
            builder.Services.AddTransient<SignUpPageViewModel>();
            builder.Services.AddSingleton<ForgotPasswordPageViewModel>();
            builder.Services.AddTransient<DashboardPageViewModel>();

            // Register Views (Ensures DI works)
            builder.Services.AddTransient<SignInPage>();
            builder.Services.AddTransient<SignUpPage>();            
            builder.Services.AddTransient<ForgotPasswordPage>();
            builder.Services.AddTransient<DashboardLayoutPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
