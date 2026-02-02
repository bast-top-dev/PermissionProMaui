using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PermissionProMaui.Models;

namespace PermissionProMaui.ViewModels
{
    /// <summary>
    /// ViewModel for the Bank Accounts page
    /// </summary>
    public partial class BankAccountsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<BankAccountItem> _accounts;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _searchText;

        public BankAccountsViewModel()
        {
            _accounts = new ObservableCollection<BankAccountItem>();
            _searchText = string.Empty;
            
            // Initialize commands
            LoadAccountsCommand = new AsyncRelayCommand(LoadAccounts);
            SearchAccountsCommand = new AsyncRelayCommand(SearchAccounts);
        }

        // Commands
        public IAsyncRelayCommand LoadAccountsCommand { get; }
        public IAsyncRelayCommand SearchAccountsCommand { get; }

        private async Task LoadAccounts()
        {
            try
            {
                IsLoading = true;
                
                // Clear existing accounts
                Accounts.Clear();

                // Add sample accounts (in a real app, this would come from a service)
                Accounts.Add(new BankAccountItem
                {
                    AccountNumber = "DE12345678901234567890",
                    AccountName = "Main Business Account",
                    IBAN = "DE12345678901234567890",
                    Balance = "€25,450.75",
                    Currency = "EUR",
                    Status = "Active"
                });

                Accounts.Add(new BankAccountItem
                {
                    AccountNumber = "DE98765432109876543210",
                    AccountName = "Savings Account",
                    IBAN = "DE98765432109876543210",
                    Balance = "€15,200.00",
                    Currency = "EUR",
                    Status = "Active"
                });

                Accounts.Add(new BankAccountItem
                {
                    AccountNumber = "DE11111111111111111111",
                    AccountName = "Investment Account",
                    IBAN = "DE11111111111111111111",
                    Balance = "€50,000.00",
                    Currency = "EUR",
                    Status = "Active"
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading accounts: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchAccounts()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    await LoadAccounts();
                    return;
                }

                var filteredAccounts = Accounts.Where(a => 
                    a.AccountName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    a.IBAN.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    a.AccountNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();

                Accounts.Clear();
                foreach (var account in filteredAccounts)
                {
                    Accounts.Add(account);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error searching accounts: {ex.Message}");
            }
        }
    }
} 