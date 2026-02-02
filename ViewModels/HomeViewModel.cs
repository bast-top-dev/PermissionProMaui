using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PermissionProMaui.Models;
using Microsoft.Maui.Controls;

namespace PermissionProMaui.ViewModels
{
    /// <summary>
    /// ViewModel for the Home page
    /// </summary>
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ActivityItem> _recentActivities;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _welcomeMessage;

        public HomeViewModel()
        {
            _recentActivities = new ObservableCollection<ActivityItem>();
            _welcomeMessage = "Welcome back!";
            
            // Initialize commands
            NavigateToBankAccountsCommand = new AsyncRelayCommand(NavigateToBankAccounts);
            NavigateToSinglePaymentsCommand = new AsyncRelayCommand(NavigateToSinglePayments);
            NavigateToAccountStatementsCommand = new AsyncRelayCommand(NavigateToAccountStatements);
            NavigateToSettingsCommand = new AsyncRelayCommand(NavigateToSettings);
            NavigateToOpenOrdersCommand = new AsyncRelayCommand(NavigateToOpenOrders);
        }

        // Commands
        public IAsyncRelayCommand NavigateToBankAccountsCommand { get; }
        public IAsyncRelayCommand NavigateToSinglePaymentsCommand { get; }
        public IAsyncRelayCommand NavigateToAccountStatementsCommand { get; }
        public IAsyncRelayCommand NavigateToSettingsCommand { get; }
        public IAsyncRelayCommand NavigateToOpenOrdersCommand { get; }

        private async Task NavigateToBankAccounts()
        {
            try
            {
                await Shell.Current.GoToAsync("/BankAccounts");
            }
            catch (Exception ex)
            {
                // Handle navigation error
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        private async Task NavigateToSinglePayments()
        {
            try
            {
                await Shell.Current.GoToAsync("/SinglePayments");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        private async Task NavigateToAccountStatements()
        {
            try
            {
                await Shell.Current.GoToAsync("/AccountStatements");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        private async Task NavigateToSettings()
        {
            try
            {
                await Shell.Current.GoToAsync("/Settings");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        private async Task NavigateToOpenOrders()
        {
            try
            {
                await Shell.Current.GoToAsync("/OpenOrders");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads recent activities for display on the home page
        /// </summary>
        public void LoadRecentActivities()
        {
            try
            {
                // Clear existing activities
                RecentActivities.Clear();

                // Add sample activities (in a real app, this would come from a service)
                RecentActivities.Add(new ActivityItem
                {
                    Icon = "payment_icon.png",
                    Title = "Payment Sent",
                    Description = "SEPA transfer to John Doe",
                    Time = "2 hours ago"
                });

                RecentActivities.Add(new ActivityItem
                {
                    Icon = "statement_icon.png",
                    Title = "Statement Received",
                    Description = "CAMT 52 statement for account DE123456789",
                    Time = "1 day ago"
                });

                RecentActivities.Add(new ActivityItem
                {
                    Icon = "account_icon.png",
                    Title = "Account Updated",
                    Description = "New account balance available",
                    Time = "3 days ago"
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading recent activities: {ex.Message}");
            }
        }
    }
} 