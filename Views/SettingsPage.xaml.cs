using System;
using Microsoft.Maui.Controls;

namespace PermissionProMaui.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("SettingsPage: OnBackClicked - Attempting to navigate back");
                
                // First try to pop the navigation stack
                if (Navigation.NavigationStack.Count > 1)
                {
                    System.Diagnostics.Debug.WriteLine("SettingsPage: Popping navigation stack");
                    await Navigation.PopAsync();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("SettingsPage: Navigation stack empty, going to home");
                    // If no navigation stack, go to home
                    await Shell.Current.GoToAsync("/Home");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SettingsPage: Navigation error: {ex.Message}");
                
                // Fallback: try to go back to home
                try
                {
                    System.Diagnostics.Debug.WriteLine("SettingsPage: Fallback to home navigation");
                    await Shell.Current.GoToAsync("/Home");
                }
                catch (Exception fallbackEx)
                {
                    System.Diagnostics.Debug.WriteLine($"SettingsPage: Fallback navigation failed: {fallbackEx.Message}");
                    // If all else fails, show an error
                    await DisplayAlert("Error", "Could not navigate back", "OK");
                }
            }
        }
        private void OnSaveClicked(object sender, EventArgs e) { }
        private void OnLanguageSelected(object sender, EventArgs e) { }
        private void OnChangeLoginPasswordClicked(object sender, EventArgs e) { }
        private void OnChangeKeyPasswordClicked(object sender, EventArgs e) { }
        private void OnClearErrorLogsClicked(object sender, EventArgs e) { }
        private void OnSendDebugDataClicked(object sender, EventArgs e) { }
        private void OnResetToDefaultsClicked(object sender, EventArgs e) { }
        private void OnSaveSettingsClicked(object sender, EventArgs e) { }
    }
}