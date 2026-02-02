using System;
using Microsoft.Maui.Controls;

namespace PermissionProMaui.Views
{
    public partial class LicensePage : ContentPage
    {
        public LicensePage()
        {
            InitializeComponent();
        }

        private void OnActivateClicked(object sender, EventArgs e) { }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("LicensePage: OnBackClicked - Attempting to navigate back");
                
                // First try to pop the navigation stack
                if (Navigation.NavigationStack.Count > 1)
                {
                    System.Diagnostics.Debug.WriteLine("LicensePage: Popping navigation stack");
                    await Navigation.PopAsync();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("LicensePage: Navigation stack empty, going to home");
                    // If no navigation stack, go to home
                    await Shell.Current.GoToAsync("/Home");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LicensePage: Navigation error: {ex.Message}");
                
                // Fallback: try to go back to home
                try
                {
                    System.Diagnostics.Debug.WriteLine("LicensePage: Fallback to home navigation");
                    await Shell.Current.GoToAsync("/Home");
                }
                catch (Exception fallbackEx)
                {
                    System.Diagnostics.Debug.WriteLine($"LicensePage: Fallback navigation failed: {fallbackEx.Message}");
                    // If all else fails, show an error
                    await DisplayAlert("Error", "Could not navigate back", "OK");
                }
            }
        }
    }
}
