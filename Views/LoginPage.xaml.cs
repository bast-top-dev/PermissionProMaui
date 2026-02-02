using System;
using Microsoft.Maui.Controls;
using PermissionProMaui.Services;
using System.Threading.Tasks;

namespace PermissionProMaui.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly AuthentificationService _authService = new AuthentificationService();
        private readonly CryptoService _cryptoService = new CryptoService();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnPasswordCompleted(object sender, EventArgs e) => await TryLoginAsync();
        private async void OnLoginClicked(object sender, EventArgs e) => await TryLoginAsync();
        private async void OnBiometricLoginClicked(object sender, EventArgs e) => await TryBiometricLoginAsync();

        private async Task TryLoginAsync()
        {
            var pw = PasswordEntry?.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(pw))
            {
                await DisplayAlert("Error", "Please enter your password.", "OK");
                return;
            }

            try
            {
                LoadingOverlay.IsVisible = true;
                LoadingText.Text = "Authenticating...";

                System.Diagnostics.Debug.WriteLine($"LoginPage: Attempting login with password: {pw}");
                
                var ok = _authService.CheckLoginPassword(pw);
                System.Diagnostics.Debug.WriteLine($"LoginPage: Password validation result: {ok}");
                
                if (!ok)
                {
                    await DisplayAlert("Error", "Invalid password.", "OK");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("LoginPage: Login successful, navigating to Home");
                await Shell.Current.GoToAsync("///Home");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoginPage: Login error: {ex.Message}");
                await DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
            }
            finally
            {
                LoadingOverlay.IsVisible = false;
            }
        }

        private async Task TryBiometricLoginAsync()
        {
            try
            {
                if (!_authService.IsBiometricLoginSet())
                {
                    await DisplayAlert("Info", "Biometric login is not configured.", "OK");
                    return;
                }

                LoadingOverlay.IsVisible = true;
                LoadingText.Text = "Authenticating...";

                var bioService = new BiometricService();
                var (ok, message) = await bioService.AuthenticateAsync("Login", "Use your biometrics");
                if (!ok)
                {
                    await DisplayAlert("Error", string.IsNullOrWhiteSpace(message) ? "Biometric authentication failed." : message, "OK");
                    return;
                }

                // Decrypt stored login password using biometric key
                var key = _authService.GetBiometricLoginKey();
                var iv = _authService.GetBiometricLoginIv();
                var pw = _cryptoService.Decrypt(key, iv, Enums.KeyType.NoMigration);
                if (pw == "Fehlerhaft")
                {
                    await DisplayAlert("Error", "Could not retrieve biometric password.", "OK");
                    return;
                }

                // Validate and navigate
                if (_authService.CheckLoginPassword(pw))
                {
                    await Shell.Current.GoToAsync("///Home");
                }
                else
                {
                    await DisplayAlert("Error", "Stored biometric password is invalid.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Biometric login failed: {ex.Message}", "OK");
            }
            finally
            {
                LoadingOverlay.IsVisible = false;
            }
        }
    }
}
