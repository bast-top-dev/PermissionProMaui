using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PermissionProMaui.Models;
using Microsoft.Maui.Graphics;

namespace PermissionProMaui.ViewModels
{
    /// <summary>
    /// ViewModel for the Account Statements page
    /// </summary>
    public partial class AccountStatementsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<StatementItem> _statements;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _selectedStatementType;

        [ObservableProperty]
        private bool _isCamt52Selected;

        [ObservableProperty]
        private bool _isCamt53Selected;

        public AccountStatementsViewModel()
        {
            _statements = new ObservableCollection<StatementItem>();
            _selectedStatementType = "CAMT 52";
            _isCamt52Selected = true;
            _isCamt53Selected = false;
            
            // Initialize commands
            LoadStatementsCommand = new AsyncRelayCommand(LoadStatements);
            SwitchToCamt52Command = new AsyncRelayCommand(SwitchToCamt52);
            SwitchToCamt53Command = new AsyncRelayCommand(SwitchToCamt53);
        }

        // Commands
        public IAsyncRelayCommand LoadStatementsCommand { get; }
        public IAsyncRelayCommand SwitchToCamt52Command { get; }
        public IAsyncRelayCommand SwitchToCamt53Command { get; }

        private async Task LoadStatements()
        {
            try
            {
                IsLoading = true;
                
                // Clear existing statements
                Statements.Clear();

                // Add sample statements (in a real app, this would come from a service)
                Statements.Add(new StatementItem
                {
                    AccountNumber = "DE12345678901234567890",
                    StatementType = SelectedStatementType,
                    Date = "2024-03-15",
                    Balance = "€5,250.75",
                    Status = "Available",
                    FileName = $"statement_{DateTime.Now:yyyyMMdd}.xml"
                });

                Statements.Add(new StatementItem
                {
                    AccountNumber = "DE12345678901234567890",
                    StatementType = SelectedStatementType,
                    Date = "2024-03-14",
                    Balance = "€5,100.25",
                    Status = "Available",
                    FileName = $"statement_{DateTime.Now.AddDays(-1):yyyyMMdd}.xml"
                });

                Statements.Add(new StatementItem
                {
                    AccountNumber = "DE12345678901234567890",
                    StatementType = SelectedStatementType,
                    Date = "2024-03-13",
                    Balance = "€4,950.50",
                    Status = "Processing",
                    FileName = $"statement_{DateTime.Now.AddDays(-2):yyyyMMdd}.xml"
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading statements: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SwitchToCamt52()
        {
            try
            {
                SelectedStatementType = "CAMT 52";
                IsCamt52Selected = true;
                IsCamt53Selected = false;
                await LoadStatements();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error switching to CAMT 52: {ex.Message}");
            }
        }

        private async Task SwitchToCamt53()
        {
            try
            {
                SelectedStatementType = "CAMT 53";
                IsCamt52Selected = false;
                IsCamt53Selected = true;
                await LoadStatements();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error switching to CAMT 53: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the color for a statement status
        /// </summary>
        /// <param name="status">The statement status</param>
        /// <returns>The color for the status</returns>
        public Color GetStatusColor(string status)
        {
            return status?.ToLower() switch
            {
                "available" => Colors.Green,
                "processing" => Colors.Orange,
                "failed" => Colors.Red,
                _ => Colors.Gray
            };
        }

        /// <summary>
        /// Gets the color for CAMT 52 button
        /// </summary>
        /// <returns>The color for CAMT 52 button</returns>
        public Color GetCamt52ButtonColor()
        {
            return IsCamt52Selected ? Colors.Blue : Colors.Gray;
        }

        /// <summary>
        /// Gets the color for CAMT 53 button
        /// </summary>
        /// <returns>The color for CAMT 53 button</returns>
        public Color GetCamt53ButtonColor()
        {
            return IsCamt53Selected ? Colors.Blue : Colors.Gray;
        }
    }
} 