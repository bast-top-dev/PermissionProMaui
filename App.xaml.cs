using Microsoft.Maui.Controls;
using PermissionProMaui.Services;

namespace PermissionProMaui
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public App()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("App: starting initialization");
                InitializeComponent();
                
                // Initialize database if it doesn't exist
                InitializeDatabase();
                
                System.Diagnostics.Debug.WriteLine("App: creating AppShell and setting MainPage");
                MainPage = new AppShell();
                
                // Ensure Home is selected and visible
                System.Diagnostics.Debug.WriteLine("App: navigating to //Home");
                Shell.Current.GoToAsync("///Home");
                System.Diagnostics.Debug.WriteLine("App: initialization completed successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"App initialization error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Show error page or fallback
                MainPage = new ContentPage
                {
                    Content = new Label
                    {
                        Text = $"App initialization failed: {ex.Message}",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                };
            }
        }

        private void InitializeDatabase()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("App: initializing database");
                var databaseService = new DatabaseService();
                
                // Force recreate database to fix password encryption issue
                if (databaseService.DatabaseExists())
                {
                    System.Diagnostics.Debug.WriteLine("App: removing old database to fix password encryption");
                    databaseService.WhipeDatabase();
                }
                
                System.Diagnostics.Debug.WriteLine("App: creating database with correct password encryption");
                // Create database with default values
                databaseService.CreateDatabase(
                    username: "admin",
                    password: "admin123",
                    mail: "admin@example.com",
                    savePw: "true",
                    authKey: "default_auth_key",
                    signKey: "default_sign_key",
                    contactNumber: "1",
                    actualVersion: "1.0.0",
                    headerText: "PermissionPro MAUI",
                    headerIcon: "icon.png",
                    headerColor: "#2196F3",
                    splashScreen: "splash.png",
                    testPwString: "test123",
                    headerTextColor: "#FFFFFF",
                    footerTextColor: "#FFFFFF"
                );
                System.Diagnostics.Debug.WriteLine("App: database created successfully with correct password encryption");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
                // Continue without database - app will show errors when trying to access it
            }
        }
    }
}