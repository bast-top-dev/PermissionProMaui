using Microsoft.Maui.Controls;

namespace PermissionProMaui.Views
{
    public partial class ContactDialog : ContentPage
    {
        public ContactDialog()
        {
            InitializeComponent();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
