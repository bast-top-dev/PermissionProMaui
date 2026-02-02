using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for branding information
    /// </summary>
    public class BrandingTable : INotifyPropertyChanged
    {
        private int _id;
        private string _companyName = string.Empty;
        private string _productName = string.Empty;
        private string _version = string.Empty;
        private string _logoPath = string.Empty;
        private string _primaryColor = string.Empty;
        private string _secondaryColor = string.Empty;
        private string _headerColor = string.Empty;
        private string _headerText = string.Empty;
        private string _headerTextColor = string.Empty;
        private string _headerIcon = string.Empty;
        private string _footerColor = string.Empty;
        private string _footerTextColor = string.Empty;
        private string _splashScreen = string.Empty;

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
        /// Gets or sets the company name
        /// </summary>
        public string CompanyName
        {
            get => _companyName;
            set
            {
                _companyName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the product name
        /// </summary>
        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        public string Version
        {
            get => _version;
            set
            {
                _version = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the logo path
        /// </summary>
        public string LogoPath
        {
            get => _logoPath;
            set
            {
                _logoPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the primary color
        /// </summary>
        public string PrimaryColor
        {
            get => _primaryColor;
            set
            {
                _primaryColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the secondary color
        /// </summary>
        public string SecondaryColor
        {
            get => _secondaryColor;
            set
            {
                _secondaryColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the header color
        /// </summary>
        public string HeaderColor
        {
            get => _headerColor;
            set
            {
                _headerColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the header text
        /// </summary>
        public string HeaderText
        {
            get => _headerText;
            set
            {
                _headerText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the header text color
        /// </summary>
        public string HeaderTextColor
        {
            get => _headerTextColor;
            set
            {
                _headerTextColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the header icon
        /// </summary>
        public string HeaderIcon
        {
            get => _headerIcon;
            set
            {
                _headerIcon = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the footer color
        /// </summary>
        public string FooterColor
        {
            get => _footerColor;
            set
            {
                _footerColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the footer text color
        /// </summary>
        public string FooterTextColor
        {
            get => _footerTextColor;
            set
            {
                _footerTextColor = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the splash screen
        /// </summary>
        public string SplashScreen
        {
            get => _splashScreen;
            set
            {
                _splashScreen = value;
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