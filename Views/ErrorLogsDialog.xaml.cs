using Microsoft.Maui.Controls;

namespace PermissionProMaui.Views
{
    public partial class ErrorLogsDialog : ContentPage
    {
        public ErrorLogsDialog()
        {
            InitializeComponent();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
