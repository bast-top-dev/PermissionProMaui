using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PermissionProMaui.Models;
using Microsoft.Maui.Graphics;

namespace PermissionProMaui.ViewModels
{
    /// <summary>
    /// ViewModel for the Single Payments page.
    /// </summary>
    public partial class SinglePaymentsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<SinglePaymentItem> _payments;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private string _selectedFilter;

        public SinglePaymentsViewModel()
        {
            _payments = new ObservableCollection<SinglePaymentItem>();
            _searchText = string.Empty;
            _selectedFilter = "All";
            
            // Initialize commands
            LoadPaymentsCommand = new AsyncRelayCommand(LoadPayments);
            SearchPaymentsCommand = new AsyncRelayCommand(SearchPayments);
            FilterPaymentsCommand = new AsyncRelayCommand<string>(FilterPayments);
        }

        // Commands
        public IAsyncRelayCommand LoadPaymentsCommand { get; }
        public IAsyncRelayCommand SearchPaymentsCommand { get; }
        public IAsyncRelayCommand<string> FilterPaymentsCommand { get; }

        private async Task LoadPayments()
        {
            try
            {
                IsLoading = true;
                
                // Clear existing payments
                Payments.Clear();

                // Add sample payments (in a real app, this would come from a service)
                Payments.Add(new SinglePaymentItem
                {
                    AccountNumber = "DE12345678901234567890",
                    OrderType = "SEPA Transfer",
                    TotalAmount = "€150.00",
                    Status = "Completed",
                    Date = "2024-03-15",
                    RecipientName = "John Doe",
                    IBAN = "DE98765432109876543210",
                    BIC = "COBADEFFXXX"
                });

                Payments.Add(new SinglePaymentItem
                {
                    AccountNumber = "DE12345678901234567890",
                    OrderType = "SEPA Transfer",
                    TotalAmount = "€75.50",
                    Status = "Pending",
                    Date = "2024-03-14",
                    RecipientName = "Jane Smith",
                    IBAN = "DE11111111111111111111",
                    BIC = "DEUTDEFFXXX"
                });

                Payments.Add(new SinglePaymentItem
                {
                    AccountNumber = "DE12345678901234567890",
                    OrderType = "SEPA Transfer",
                    TotalAmount = "€200.00",
                    Status = "Failed",
                    Date = "2024-03-13",
                    RecipientName = "Bob Johnson",
                    IBAN = "DE22222222222222222222",
                    BIC = "COMMDEFFXXX"
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading payments: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchPayments()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    await LoadPayments();
                    return;
                }

                var filteredPayments = Payments.Where(p => 
                    p.RecipientName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    p.IBAN.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    p.TotalAmount.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();

                Payments.Clear();
                foreach (var payment in filteredPayments)
                {
                    Payments.Add(payment);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error searching payments: {ex.Message}");
            }
        }

        private async Task FilterPayments(string filter)
        {
            try
            {
                SelectedFilter = filter;
                
                if (filter == "All")
                {
                    await LoadPayments();
                    return;
                }

                var filteredPayments = Payments.Where(p => p.Status.Equals(filter, StringComparison.OrdinalIgnoreCase)).ToList();
                
                Payments.Clear();
                foreach (var payment in filteredPayments)
                {
                    Payments.Add(payment);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error filtering payments: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the color for a payment status
        /// </summary>
        /// <param name="status">The payment status</param>
        /// <returns>The color for the status</returns>
        public Color GetStatusColor(string status)
        {
            return status?.ToLower() switch
            {
                "completed" => Colors.Green,
                "pending" => Colors.Orange,
                "failed" => Colors.Red,
                _ => Colors.Gray
            };
        }
    }

    /// <summary>
    /// ViewModel for individual single payment items.
    /// </summary>
    public class SinglePaymentViewModel : INotifyPropertyChanged
    {
        private readonly SepaModel _sepaModel;

        public SinglePaymentViewModel(SepaModel sepaModel)
        {
            _sepaModel = sepaModel;
        }

        /// <summary>
        /// Gets the account holder type (Recipient or Payor).
        /// </summary>
        public string AccountHolderType => _sepaModel.AccountHeaderText.Equals("Begünstigte") 
            ? "Recipient of Payment" 
            : "Payor";

        /// <summary>
        /// Gets the account holder name.
        /// </summary>
        public string AccountHolderName => _sepaModel.AccountHeaderText.Equals("Begünstigte") 
            ? _sepaModel.CreditorName 
            : _sepaModel.DebitorName;

        /// <summary>
        /// Gets the account number (IBAN).
        /// </summary>
        public string AccountNumber => _sepaModel.AccountHeaderText.Equals("Begünstigte") 
            ? _sepaModel.CreditorIban 
            : _sepaModel.DebitorIban;

        /// <summary>
        /// Gets the formatted amount with currency and sign.
        /// </summary>
        public string FormattedAmount
        {
            get
            {
                var sign = _sepaModel.AccountHeaderText.Equals("Begünstigte") ? "-" : "+";
                return $"{_sepaModel.Currency} {sign}{_sepaModel.TotalAmount}";
            }
        }

        /// <summary>
        /// Gets the color for the amount display.
        /// </summary>
        public Microsoft.Maui.Graphics.Color AmountColor => _sepaModel.AccountHeaderText.Equals("Begünstigte") 
            ? Colors.Red 
            : Colors.Green;

        /// <summary>
        /// Gets the formatted requested execution date.
        /// </summary>
        public string RequestedExecutionDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sepaModel.RequestedExecutionDate))
                    return string.Empty;

                return DateTime.TryParse(_sepaModel.RequestedExecutionDate, out DateTime parsedDateTime)
                    ? $"Execution Date: {parsedDateTime.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}"
                    : $"Execution Date: {_sepaModel.RequestedExecutionDate}";
            }
        }

        /// <summary>
        /// Gets whether to show the execution date (only for payors).
        /// </summary>
        public bool ShowExecutionDate => !_sepaModel.AccountHeaderText.Equals("Begünstigte");

        /// <summary>
        /// Gets the ESR reference number.
        /// </summary>
        public string EsrReferenceNumber => string.IsNullOrWhiteSpace(_sepaModel.EsrRefNumber) 
            ? string.Empty 
            : $"ESR Reference: {_sepaModel.EsrRefNumber}";

        /// <summary>
        /// Gets whether to show the ESR reference number.
        /// </summary>
        public bool ShowEsrReference => !string.IsNullOrWhiteSpace(_sepaModel.EsrRefNumber);

        /// <summary>
        /// Gets the first description line.
        /// </summary>
        public string Description1 => _sepaModel.RmtInf?.Length > 0 ? _sepaModel.RmtInf[0] : string.Empty;

        /// <summary>
        /// Gets whether the first description line exists.
        /// </summary>
        public bool HasDescription1 => _sepaModel.RmtInf?.Length > 0 && !string.IsNullOrWhiteSpace(_sepaModel.RmtInf[0]);

        /// <summary>
        /// Gets the second description line.
        /// </summary>
        public string Description2 => _sepaModel.RmtInf?.Length > 1 ? _sepaModel.RmtInf[1] : string.Empty;

        /// <summary>
        /// Gets whether the second description line exists.
        /// </summary>
        public bool HasDescription2 => _sepaModel.RmtInf?.Length > 1 && !string.IsNullOrWhiteSpace(_sepaModel.RmtInf[1]);

        /// <summary>
        /// Gets the third description line.
        /// </summary>
        public string Description3 => _sepaModel.RmtInf?.Length > 2 ? _sepaModel.RmtInf[2] : string.Empty;

        /// <summary>
        /// Gets whether the third description line exists.
        /// </summary>
        public bool HasDescription3 => _sepaModel.RmtInf?.Length > 2 && !string.IsNullOrWhiteSpace(_sepaModel.RmtInf[2]);

        /// <summary>
        /// Gets the fourth description line.
        /// </summary>
        public string Description4 => _sepaModel.RmtInf?.Length > 3 ? _sepaModel.RmtInf[3] : string.Empty;

        /// <summary>
        /// Gets whether the fourth description line exists.
        /// </summary>
        public bool HasDescription4 => _sepaModel.RmtInf?.Length > 3 && !string.IsNullOrWhiteSpace(_sepaModel.RmtInf[3]);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 