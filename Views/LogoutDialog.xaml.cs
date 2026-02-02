using Microsoft.Maui.Controls;

namespace PermissionProMaui.Views
{
    public partial class LogoutDialog : ContentPage
    {
        public LogoutDialog()
        {
            InitializeComponent();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            try
            {
                // Here you would typically clear user session, clear stored credentials, etc.
                // For now, we'll just show a confirmation and go back
                await DisplayAlert("Abgemeldet", "Sie wurden erfolgreich abgemeldet.", "OK");
                
                                 // Navigate back to home or login page
                 await Shell.Current.GoToAsync("/Home");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Fehler", $"Fehler beim Abmelden: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error during logout: {ex.Message}");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
