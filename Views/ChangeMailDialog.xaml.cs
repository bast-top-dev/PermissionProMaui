using Microsoft.Maui.Controls;
using PermissionProMaui.Services;

namespace PermissionProMaui.Views
{
    public partial class ChangeMailDialog : ContentPage
    {
        private readonly AppUserSettingsService _appUserSettingsService;

        public ChangeMailDialog(AppUserSettingsService appUserSettingsService)
        {
            InitializeComponent();
            _appUserSettingsService = appUserSettingsService;
            LoadCurrentEmail();
        }

        public ChangeMailDialog()
        {
            InitializeComponent();
            // For XAML design time
        }

        private void LoadCurrentEmail()
        {
            try
            {
                if (_appUserSettingsService != null)
                {
                    var user = _appUserSettingsService.GetUser();
                    if (user != null && !string.IsNullOrEmpty(user.Mailadress))
                    {
                        EmailEntry.Text = user.Mailadress;
                        ConfirmEmailEntry.Text = user.Mailadress;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading current email: {ex.Message}");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                var newEmail = EmailEntry.Text?.Trim();
                var confirmEmail = ConfirmEmailEntry.Text?.Trim();

                if (string.IsNullOrEmpty(newEmail))
                {
                    await DisplayAlert("Fehler", "Bitte geben Sie eine Emailadresse ein.", "OK");
                    return;
                }

                if (newEmail != confirmEmail)
                {
                    await DisplayAlert("Fehler", "Die Emailadressen stimmen nicht überein.", "OK");
                    return;
                }

                if (_appUserSettingsService != null)
                {
                    _appUserSettingsService.UpdateUserMail(newEmail);
                    await DisplayAlert("Erfolg", "Emailadresse wurde erfolgreich geändert.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await DisplayAlert("Fehler", "Service nicht verfügbar.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fehler", $"Fehler beim Speichern der Emailadresse: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error saving email: {ex.Message}");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
