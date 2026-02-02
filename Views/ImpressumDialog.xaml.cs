using Microsoft.Maui.Controls;

namespace PermissionProMaui.Views
{
    public partial class ImpressumDialog : ContentPage
    {
        public ImpressumDialog()
        {
            InitializeComponent();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
