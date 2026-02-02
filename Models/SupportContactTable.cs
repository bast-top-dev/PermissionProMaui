using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for support contact information
    /// </summary>
    public class SupportContactTable : INotifyPropertyChanged
    {
        private int _id;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private string _website = string.Empty;
        private string _name = string.Empty;
        private string _address = string.Empty;
        private string _hours = string.Empty;
        private string _supportMailadress = string.Empty;
        private string _supportContactname = string.Empty;
        private string _supportPhonenumber = string.Empty;

        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the support email
        /// </summary>
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the support phone
        /// </summary>
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the support website
        /// </summary>
        public string Website
        {
            get => _website;
            set
            {
                _website = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the contact name
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the contact address
        /// </summary>
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the support hours
        /// </summary>
        public string Hours
        {
            get => _hours;
            set
            {
                _hours = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the support mail address
        /// </summary>
        public string SupportMailadress
        {
            get => _supportMailadress;
            set
            {
                _supportMailadress = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the support contact name
        /// </summary>
        public string SupportContactname
        {
            get => _supportContactname;
            set
            {
                _supportContactname = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the support phone number
        /// </summary>
        public string SupportPhonenumber
        {
            get => _supportPhonenumber;
            set
            {
                _supportPhonenumber = value;
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