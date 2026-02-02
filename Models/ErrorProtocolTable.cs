using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for error protocol table entries
    /// </summary>
    public class ErrorProtocolTable : INotifyPropertyChanged
    {
        private string _errorMessage = string.Empty;
        private string _errorType = string.Empty;
        private string _stackTrace = string.Empty;
        private string _timestamp = string.Empty;
        private string _deviceInfo = string.Empty;
        private string _appVersion = string.Empty;

        /// <summary>
        /// Gets or sets the error message
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the error type
        /// </summary>
        public string ErrorType
        {
            get => _errorType;
            set
            {
                _errorType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the stack trace
        /// </summary>
        public string StackTrace
        {
            get => _stackTrace;
            set
            {
                _stackTrace = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the timestamp
        /// </summary>
        public string Timestamp
        {
            get => _timestamp;
            set
            {
                _timestamp = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the device information
        /// </summary>
        public string DeviceInfo
        {
            get => _deviceInfo;
            set
            {
                _deviceInfo = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the app version
        /// </summary>
        public string AppVersion
        {
            get => _appVersion;
            set
            {
                _appVersion = value;
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
