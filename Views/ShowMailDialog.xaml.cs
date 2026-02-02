using Microsoft.Maui.Controls;
using PermissionProMaui.Services;

namespace PermissionProMaui.Views
{
    public partial class ShowMailDialog : ContentPage
    {
        private readonly AppUserSettingsService _appUserSettingsService;

        public ShowMailDialog(AppUserSettingsService appUserSettingsService)
        {
            InitializeComponent();
            _appUserSettingsService = appUserSettingsService;
            LoadEmail();
        }

        public ShowMailDialog()
        {
            InitializeComponent();
            // For XAML design time
        }

        private void LoadEmail()
        {
            try
            {
                if (_appUserSettingsService != null)
                {
                    var user = _appUserSettingsService.GetUser();
                    if (user != null && !string.IsNullOrEmpty(user.Mailadress))
                    {
                        EmailLabel.Text = user.Mailadress;
                    }
                    else
                    {
                        EmailLabel.Text = "Keine Emailadresse gesetzt";
                    }
                }
                else
                {
                    EmailLabel.Text = "Service nicht verfügbar";
                }
            }
            catch (Exception ex)
            {
                EmailLabel.Text = "Fehler beim Laden der Emailadresse";
                System.Diagnostics.Debug.WriteLine($"Error loading email: {ex.Message}");
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
