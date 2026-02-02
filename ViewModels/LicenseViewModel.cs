using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PermissionProMaui.ViewModels
{
    /// <summary>
    /// ViewModel for the License page
    /// </summary>
    public partial class LicenseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _licenseCode;

        [ObservableProperty]
        private bool _isLicensed;

        [ObservableProperty]
        private string _testEndDate;

        [ObservableProperty]
        private string _companyName;

        [ObservableProperty]
        private string _productName;

        [ObservableProperty]
        private string _version;

        [ObservableProperty]
        private string _supportEmail;

        [ObservableProperty]
        private string _supportPhone;

        [ObservableProperty]
        private string _supportWebsite;

        public LicenseViewModel()
        {
            _licenseCode = "Nicht lizenziert -- Testversion";
            _isLicensed = false;
            _testEndDate = DateTime.Now.AddDays(30).ToShortDateString();
            _companyName = "windata GmbH & Co. KG";
            _productName = "PermissionPro MAUI";
            _version = "1.0.0";
            _supportEmail = "support@windata.de";
            _supportPhone = "+49 123 456789";
            _supportWebsite = "https://www.windata.de";
            
            // Initialize commands
            LoadLicenseInfoCommand = new AsyncRelayCommand(LoadLicenseInfo);
            ContactSupportCommand = new AsyncRelayCommand(ContactSupport);
            RenewLicenseCommand = new AsyncRelayCommand(RenewLicense);
            UpgradeLicenseCommand = new AsyncRelayCommand(UpgradeLicense);
        }

        // Commands
        public IAsyncRelayCommand LoadLicenseInfoCommand { get; }
        public IAsyncRelayCommand ContactSupportCommand { get; }
        public IAsyncRelayCommand RenewLicenseCommand { get; }
        public IAsyncRelayCommand UpgradeLicenseCommand { get; }

        private async Task LoadLicenseInfo()
        {
            try
            {
                // Load license information (in a real app, this would come from a service)
                System.Diagnostics.Debug.WriteLine("License information loaded successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading license info: {ex.Message}");
            }
        }

        private async Task ContactSupport()
        {
            try
            {
                // Contact support functionality (in a real app, this would open email or phone)
                System.Diagnostics.Debug.WriteLine("Contact support functionality will be implemented");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error contacting support: {ex.Message}");
            }
        }

        private async Task RenewLicense()
        {
            try
            {
                // Renew license functionality (in a real app, this would open renewal page)
                System.Diagnostics.Debug.WriteLine("License renewal functionality will be implemented");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error renewing license: {ex.Message}");
            }
        }

        private async Task UpgradeLicense()
        {
            try
            {
                // Upgrade license functionality (in a real app, this would open upgrade page)
                System.Diagnostics.Debug.WriteLine("License upgrade functionality will be implemented");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error upgrading license: {ex.Message}");
            }
        }
    }
} 