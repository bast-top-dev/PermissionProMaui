using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for statement items displayed on the account statements page
    /// </summary>
    public class StatementItem : INotifyPropertyChanged
    {
        private string _accountNumber = string.Empty;
        private string _statementType = string.Empty;
        private string _date = string.Empty;
        private string _balance = string.Empty;
        private string _status = string.Empty;
        private string _fileName = string.Empty;

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
        /// Gets or sets the statement type (CAMT 52, CAMT 53, etc.)
        /// </summary>
        public string StatementType
        {
            get => _statementType;
            set
            {
                _statementType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the statement date
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
        /// Gets or sets the statement status
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
        /// Gets or sets the file name
        /// </summary>
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
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
