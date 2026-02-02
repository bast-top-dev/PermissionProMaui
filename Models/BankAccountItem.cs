using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for bank account items displayed on the bank accounts page
    /// </summary>
    public class BankAccountItem : INotifyPropertyChanged
    {
        private string _accountNumber = string.Empty;
        private string _accountName = string.Empty;
        private string _iban = string.Empty;
        private string _balance = string.Empty;
        private string _currency = string.Empty;
        private string _status = string.Empty;

        /// <summary>
        /// Gets or sets the account number
        /// </summary>
        public string AccountNumber
        {
            get => _accountNumber;
            set
            {
                _accountNumber = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the account name
        /// </summary>
        public string AccountName
        {
            get => _accountName;
            set
            {
                _accountName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the IBAN
        /// </summary>
        public string IBAN
        {
            get => _iban;
            set
            {
                _iban = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the account balance
        /// </summary>
        public string Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the currency
        /// </summary>
        public string Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the account status
        /// </summary>
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
