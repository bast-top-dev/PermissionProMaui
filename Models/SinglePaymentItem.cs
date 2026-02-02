using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for single payment items displayed on the single payments page
    /// </summary>
    public class SinglePaymentItem : INotifyPropertyChanged
    {
        private string _accountNumber = string.Empty;
        private string _orderType = string.Empty;
        private string _totalAmount = string.Empty;
        private string _status = string.Empty;
        private string _date = string.Empty;
        private string _recipientName = string.Empty;
        private string _iban = string.Empty;
        private string _bic = string.Empty;

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
        /// Gets or sets the order type
        /// </summary>
        public string OrderType
        {
            get => _orderType;
            set
            {
                _orderType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the total amount
        /// </summary>
        public string TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the payment status
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

        /// <summary>
        /// Gets or sets the payment date
        /// </summary>
        public string Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the recipient name
        /// </summary>
        public string RecipientName
        {
            get => _recipientName;
            set
            {
                _recipientName = value;
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
        /// Gets or sets the BIC
        /// </summary>
        public string BIC
        {
            get => _bic;
            set
            {
                _bic = value;
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
