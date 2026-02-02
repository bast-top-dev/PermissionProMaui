using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace PermissionProMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            
            try
            {
                // Get HomePage from DI container and set it as the main content
                var homePage = Handler?.MauiContext?.Services.GetService<Views.HomePage>();
                if (homePage != null)
                {
                    // Replace the default HomePage with the injected one
                    var shellContent = this.FindByName<ShellContent>("HomeShellContent");
                    if (shellContent != null)
                    {
                        shellContent.Content = homePage;
                        System.Diagnostics.Debug.WriteLine("HomePage successfully injected from DI");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("HomePage not found in DI container");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting up HomePage from DI: {ex.Message}");
                // Continue with default HomePage if DI fails
            }
        }

        protected override bool OnBackButtonPressed()
        {
            // Allow back button functionality for proper navigation
            return false; // Return false to allow default back navigation
        }

        private void RegisterRoutes()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("RegisterRoutes: Starting route registration...");
                
            Routing.RegisterRoute("Home", typeof(Views.HomePage));
                System.Diagnostics.Debug.WriteLine("RegisterRoutes: Home route registered");
                
                Routing.RegisterRoute("LoginPage", typeof(Views.LoginPage));
                System.Diagnostics.Debug.WriteLine("RegisterRoutes: LoginPage route registered");
                
                Routing.RegisterRoute("BankAccountsPage", typeof(Views.BankAccountsPage));
                Routing.RegisterRoute("SinglePaymentsPage", typeof(Views.SinglePaymentsPage));
                Routing.RegisterRoute("AccountStatementsPage", typeof(Views.AccountStatementsPage));
                
                Routing.RegisterRoute("SettingsPage", typeof(Views.SettingsPage));
                System.Diagnostics.Debug.WriteLine("RegisterRoutes: SettingsPage route registered");
                
                Routing.RegisterRoute("LicensePage", typeof(Views.LicensePage));
                System.Diagnostics.Debug.WriteLine("RegisterRoutes: LicensePage route registered");
                
                Routing.RegisterRoute("EditBankPage", typeof(Views.EditBankPage));
                Routing.RegisterRoute("EditContactPage", typeof(Views.EditContactPage));
                Routing.RegisterRoute("QrCodeExportPage", typeof(Views.QrCodeExportPage));
                
                // Sidebar dialog routes
                Routing.RegisterRoute("ShowMailDialog", typeof(Views.ShowMailDialog));
                Routing.RegisterRoute("ChangeMailDialog", typeof(Views.ChangeMailDialog));
                Routing.RegisterRoute("ContactDialog", typeof(Views.ContactDialog));
                Routing.RegisterRoute("ErrorLogsDialog", typeof(Views.ErrorLogsDialog));
                Routing.RegisterRoute("ImpressumDialog", typeof(Views.ImpressumDialog));
                Routing.RegisterRoute("LogoutDialog", typeof(Views.LogoutDialog));
                
                System.Diagnostics.Debug.WriteLine("RegisterRoutes: All routes registered successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RegisterRoutes: Error registering routes: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"RegisterRoutes: Stack trace: {ex.StackTrace}");
            }
        }

        // Handlers for custom flyout taps
        private async void OnFlyoutNavigate(object sender, TappedEventArgs e)
        {
            if (e?.Parameter is string route && !string.IsNullOrWhiteSpace(route))
            {
                try 
                { 
                    System.Diagnostics.Debug.WriteLine($"FlyoutNavigate: Attempting to navigate to route: {route}");
                    
                    // Use relative route for navigation from sidebar (this fixes the MAUI Shell routing issue)
                    var navigationRoute = route.StartsWith("/") ? route : $"/{route}";
                    System.Diagnostics.Debug.WriteLine($"FlyoutNavigate: Using relative navigation route: {navigationRoute}");
                    
                    await Shell.Current.GoToAsync(navigationRoute, animate: true);
                    System.Diagnostics.Debug.WriteLine($"FlyoutNavigate: Navigation completed successfully");
                }
                catch (Exception ex) 
                { 
                    System.Diagnostics.Debug.WriteLine($"Flyout navigation error: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Flyout navigation stack trace: {ex.StackTrace}");
                    
                    // Show user-friendly error message
                    try
                    {
                        await Shell.Current.DisplayAlert("Navigation Error", $"Could not navigate to {route}: {ex.Message}", "OK");
                    }
                    catch { }
                }
                finally { Shell.Current.FlyoutIsPresented = false; }
            }
        }

        private async void OnFlyoutShowMail(object sender, TappedEventArgs e)
        {
            try 
            { 
                var homePage = Shell.Current.CurrentPage as Views.HomePage;
                if (homePage != null)
                {
                    await homePage.ShowMailDialog();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "HomePage not available", "OK");
                }
            }
            catch (Exception ex) 
            { 
                System.Diagnostics.Debug.WriteLine($"ShowMail error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"Failed to show mail: {ex.Message}", "OK");
            }
            finally { Shell.Current.FlyoutIsPresented = false; }
        }

        private async void OnFlyoutChangeMail(object sender, TappedEventArgs e)
        {
            try 
            { 
                var homePage = Shell.Current.CurrentPage as Views.HomePage;
                if (homePage != null)
                {
                    await homePage.ChangeMailDialog();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "HomePage not available", "OK");
                }
            }
            catch (Exception ex) 
            { 
                System.Diagnostics.Debug.WriteLine($"ChangeMail error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"Failed to change mail: {ex.Message}", "OK");
            }
            finally { Shell.Current.FlyoutIsPresented = false; }
        }

        private async void OnFlyoutContactProtocol(object sender, TappedEventArgs e)
        {
            try 
            { 
                var homePage = Shell.Current.CurrentPage as Views.HomePage;
                if (homePage != null)
                {
                    await homePage.ShowContactAndProtocolDialog();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "HomePage not available", "OK");
                }
            }
            catch (Exception ex) 
            { 
                System.Diagnostics.Debug.WriteLine($"ContactProtocol error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"Failed to show contact: {ex.Message}", "OK");
            }
            finally { Shell.Current.FlyoutIsPresented = false; }
        }

        private async void OnFlyoutErrorLogs(object sender, TappedEventArgs e)
        {
            try 
            { 
                var homePage = Shell.Current.CurrentPage as Views.HomePage;
                if (homePage != null)
                {
                    await homePage.ShowErrorLogsDialog();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "HomePage not available", "OK");
                }
            }
            catch (Exception ex) 
            { 
                System.Diagnostics.Debug.WriteLine($"ErrorLogs error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"Failed to show error logs: {ex.Message}", "OK");
            }
            finally { Shell.Current.FlyoutIsPresented = false; }
        }

        private async void OnFlyoutLogout(object sender, TappedEventArgs e)
        {
            try 
            { 
                var homePage = Shell.Current.CurrentPage as Views.HomePage;
                if (homePage != null)
                {
                    await homePage.ShowLogoutDialog();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "HomePage not available", "OK");
                }
            }
            catch (Exception ex) 
            { 
                System.Diagnostics.Debug.WriteLine($"Logout error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"Failed to logout: {ex.Message}", "OK");
            }
            finally { Shell.Current.FlyoutIsPresented = false; }
        }
    }
}