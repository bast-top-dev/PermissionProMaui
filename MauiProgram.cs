using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using SQLitePCL;

namespace PermissionProMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        System.Diagnostics.Debug.WriteLine("MauiProgram.CreateMauiApp started");
        
        // Initialize SQLite - use the correct provider for MAUI
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        
        try
        {
            var builder = MauiApp.CreateBuilder();
            System.Diagnostics.Debug.WriteLine("MauiApp.CreateBuilder completed");
            
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Services
            builder.Services.AddSingleton<Services.DatabaseService>();
            builder.Services.AddSingleton<Services.VeuService>();
            builder.Services.AddSingleton<Services.InitialisationManagementService>();
            builder.Services.AddSingleton<Services.LicensingService>();
            builder.Services.AddSingleton<Services.CryptoService>();
            builder.Services.AddSingleton<Services.QrCodeService>();
            builder.Services.AddSingleton<Services.BiometricService>();
            builder.Services.AddSingleton<Services.ApiConnectionService>();
            builder.Services.AddSingleton<Services.AppUserSettingsService>();
            builder.Services.AddSingleton<Services.ErrorProtocolService>();
            builder.Services.AddSingleton<Services.FileSystemService>();
            builder.Services.AddSingleton<Services.ResourcesMigrationService>();
            builder.Services.AddSingleton<Services.TestingService>();
            builder.Services.AddSingleton<Services.LicenceService>();
            builder.Services.AddSingleton<Services.AuthentificationService>();
            builder.Services.AddSingleton<Services.VeuManagementService>();
            builder.Services.AddSingleton<Services.EbicsService>();

            // Repositories
            builder.Services.AddSingleton<Repos.AppSettingsRepo>();
            builder.Services.AddSingleton<Repos.BrandingRepo>();
            builder.Services.AddSingleton<Repos.CodeLicenceRepo>();
            builder.Services.AddSingleton<Repos.EbicsContactRepo>();
            builder.Services.AddSingleton<Repos.SystemRepo>();

            // ViewModels
            builder.Services.AddTransient<ViewModels.HomeViewModel>();
            builder.Services.AddTransient<ViewModels.BankAccountsViewModel>();
            builder.Services.AddTransient<ViewModels.AccountStatementsViewModel>();
            builder.Services.AddTransient<ViewModels.SinglePaymentsViewModel>();
            builder.Services.AddTransient<ViewModels.SettingsViewModel>();
            builder.Services.AddTransient<ViewModels.LicenseViewModel>();

            // Views
            builder.Services.AddTransient<Views.HomePage>();
            builder.Services.AddTransient<Views.LoginPage>();
            builder.Services.AddTransient<Views.BankAccountsPage>();
            builder.Services.AddTransient<Views.AccountStatementsPage>();
            builder.Services.AddTransient<Views.SinglePaymentsPage>();
            builder.Services.AddTransient<Views.SettingsPage>();
            builder.Services.AddTransient<Views.LicensePage>();
            builder.Services.AddTransient<Views.EditBankPage>();
            builder.Services.AddTransient<Views.EditContactPage>();
            builder.Services.AddTransient<Views.QrCodeExportPage>();
            
            // Sidebar dialog views
            builder.Services.AddTransient<Views.ShowMailDialog>();
            builder.Services.AddTransient<Views.ChangeMailDialog>();
            builder.Services.AddTransient<Views.ContactDialog>();
            builder.Services.AddTransient<Views.ErrorLogsDialog>();
            builder.Services.AddTransient<Views.ImpressumDialog>();
            builder.Services.AddTransient<Views.LogoutDialog>();

            System.Diagnostics.Debug.WriteLine("Builder configuration completed");

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            System.Diagnostics.Debug.WriteLine("MauiApp built successfully");

            // Initialize database after building the app
            try
            {
                var databaseService = app.Services.GetService<Services.DatabaseService>();
                if (databaseService != null)
                {
                    databaseService.Initialize();
                    System.Diagnostics.Debug.WriteLine("Database initialized successfully");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("DatabaseService not found in DI container");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
            }

            return app;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MauiProgram.CreateMauiApp error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }
} 