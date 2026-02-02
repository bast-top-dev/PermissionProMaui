using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using PermissionProMaui.Services;
using PermissionProMaui.Models;
using PermissionProMaui.Models.Json;
using PermissionProMaui.Enums;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace PermissionProMaui.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly VeuService _veuService;
        private readonly AuthentificationService _authService;
        private readonly InitialisationManagementService _initService;
        private readonly VeuManagementService _veuManagementService;
        private readonly LicensingService _licensingService;
        private readonly AppUserSettingsService _appUserSettingsService;
        private readonly ErrorProtocolService _errorProtocolService;
        private readonly LicenceService _licenceService;
        private readonly QrCodeService _qrCodeService;
        private readonly CryptoService _cryptoService;
        private readonly ApiConnectionService _apiConnectionService;

        public HomePage(VeuService veuService, InitialisationManagementService initService, LicensingService licensingService, 
                       AuthentificationService authService, VeuManagementService veuManagementService, 
                       AppUserSettingsService appUserSettingsService, ErrorProtocolService errorProtocolService, 
                       LicenceService licenceService, QrCodeService qrCodeService, CryptoService cryptoService, 
                       ApiConnectionService apiConnectionService)
        {
            _veuService = veuService;
            _initService = initService;
            _licensingService = licensingService;
            _authService = authService;
            _veuManagementService = veuManagementService;
            _appUserSettingsService = appUserSettingsService;
            _errorProtocolService = errorProtocolService;
            _licenceService = licenceService;
            _qrCodeService = qrCodeService;
            _cryptoService = cryptoService;
            _apiConnectionService = apiConnectionService;
            InitializeComponent();
            LoadContacts();
        }

        // Default constructor for XAML compatibility
        public HomePage()
        {
            InitializeComponent();
            // Services will be null, but this is only for XAML design-time
            // In runtime, this should not be used as services are injected
        }

        private void LoadContacts()
        {
            try
            {
                if (_veuService == null)
                {
                    System.Diagnostics.Debug.WriteLine("VeuService is null, skipping contact loading");
                    return;
                }
                List<EbicsContactModel> contacts = _veuService.GetAllEbicsContacts();
                ContactsCollectionView.ItemsSource = contacts;
                NoContentLabel.IsVisible = contacts == null || contacts.Count == 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading contacts: {ex.Message}");
            }
        }

        private async void OnContactSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
                return;

            var contact = e.CurrentSelection[0] as EbicsContactModel;
            ((CollectionView)sender).SelectedItem = null;
            if (contact == null) return;

            // Show context menu for the contact
            await ShowContactContextMenu(contact);
        }

        private async Task ShowContactContextMenu(EbicsContactModel contact)
        {
            var action = await DisplayActionSheet(contact.Bank.Bankname, "Cancel", null, 
                "Edit Bank", 
                "Delete Contact", 
                "Edit Contact", 
                "Export Contact",
                "Continue with EBICS");

            switch (action)
            {
                case "Edit Bank":
                    await EditBank(contact);
                    break;
                case "Delete Contact":
                    await DeleteContact(contact);
                    break;
                case "Edit Contact":
                    await EditContact(contact);
                    break;
                case "Export Contact":
                    await ExportContact(contact);
                    break;
                case "Continue with EBICS":
                    await ContinueWithEbics(contact);
                    break;
            }
        }

        private async Task EditBank(EbicsContactModel contact)
        {
            try
            {
                if (_veuService == null)
                {
                    await DisplayAlert("Error", "Services not initialized", "OK");
                    return;
                }
                await Navigation.PushAsync(new EditBankPage(_veuService, contact));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to open Edit Bank page: {ex.Message}", "OK");
            }
        }

        private async Task DeleteContact(EbicsContactModel contact)
        {
            var confirm = await DisplayAlert("Warning", "Are you sure you want to delete this contact?", "Delete", "Cancel");
            if (confirm)
            {
                try
                {
                    var bank = _veuService.GetBank(contact.Bank.Id);
                    _veuService.DeleteEbicsContact(bank);
                    await DisplayAlert("Success", "Contact deleted successfully", "OK");
                    LoadContacts();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete contact: {ex.Message}", "OK");
                }
            }
        }

        private async Task EditContact(EbicsContactModel contact)
        {
            try
            {
                await Navigation.PushAsync(new EditContactPage(_veuService, _initService, _authService, _appUserSettingsService, _errorProtocolService, contact));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to open Edit Contact page: {ex.Message}", "OK");
            }
        }

        private async Task ExportContact(EbicsContactModel contact)
        {
            try
            {
                await Navigation.PushAsync(new QrCodeExportPage(_qrCodeService, _appUserSettingsService, _apiConnectionService, _cryptoService, contact));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to open Export Contact page: {ex.Message}", "OK");
            }
        }

        private async Task ContinueWithEbics(EbicsContactModel contact)
        {
            // Original flow: prefer biometric, else prompt for password
            if (_authService.IsBiometricAuthentificationSet())
            {
                var bio = new BiometricService();
                var (ok, msg) = await bio.AuthenticateAsync("Authenticate", "Use your biometrics");
                if (!ok)
                {
                    await DisplayAlert("Warning", string.IsNullOrWhiteSpace(msg) ? "Biometric authentication failed" : msg, "OK");
                    return;
                }
                await ContinueContactWorkflowAsync(contact, password: null, usedBiometric: true);
            }
            else
            {
                string pw = await DisplayPromptAsync("Password", "Enter password", initialValue: string.Empty, maxLength: 128, keyboard: Keyboard.Text, placeholder: "Password", accept: "OK", cancel: "Cancel");
                if (string.IsNullOrWhiteSpace(pw)) return;
                await ContinueContactWorkflowAsync(contact, pw, usedBiometric: false);
            }
        }

        private async Task ContinueContactWorkflowAsync(EbicsContactModel contact, string password, bool usedBiometric)
        {
            try
            {
                // Get the actual password if biometric was used
                if (usedBiometric)
                {
                    try
                    {
                        // Get the stored biometric authentication key
                        var biometricKey = _authService.GetBiometricAuthentificationKey();
                        var biometricIv = _authService.GetBiometricAuthentificationIv();
                        
                        if (!string.IsNullOrEmpty(biometricKey) && !string.IsNullOrEmpty(biometricIv))
                        {
                            // Decrypt the stored password using the biometric key
                            password = _cryptoService.Decrypt(biometricKey, biometricIv, KeyType.NoMigration);
                            
                            if (password == "Fehlerhaft")
                            {
                                await DisplayAlert("Error", "Failed to decrypt biometric password. Please use password authentication.", "OK");
                                return;
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error", "No biometric password stored. Please use password authentication.", "OK");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Failed to retrieve biometric password: {ex.Message}", "OK");
                        return;
                    }
                }

                var initPhase = contact.Bank.InitPhase;
                EbicsErrorCodeModel result;

                switch (initPhase)
                {
                    case PermissionProMaui.EbicsContactInitStatus.UnInitialized:
                    case PermissionProMaui.EbicsContactInitStatus.IniFailed:
                        // Send INI request
                        result = await _initService.SendInitRequests(contact, password, "ini");
                        if (result.EbicsErrorCode.Equals("000000"))
                        {
                            _veuService.UpdateIniPhase(contact, PermissionProMaui.EbicsContactInitStatus.PartiallyInitialized);
                            
                            // Send HIA request
                            result = await _initService.SendInitRequests(contact, password, "hia");
                            if (result.EbicsErrorCode.Equals("000000"))
                            {
                                _veuService.UpdateIniPhase(contact, PermissionProMaui.EbicsContactInitStatus.Letters);
                                
                                // Send initialization letters
                                result = await _initService.SendInitialisationLetters(contact, password, _appUserSettingsService.GetUserMail());
                                if (result.EbicsErrorCode.Equals("000000"))
                                {
                                    _veuService.UpdateIniPhase(contact, PermissionProMaui.EbicsContactInitStatus.NeedBankKeys);
                                    await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                                }
                                else
                                {
                                    await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                                }
                            }
                            else
                            {
                                await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                        }
                        break;

                    case PermissionProMaui.EbicsContactInitStatus.PartiallyInitialized:
                        result = await _initService.SendInitRequests(contact, password, "hia");
                        if (result.EbicsErrorCode.Equals("000000"))
                        {
                            _veuService.UpdateIniPhase(contact, PermissionProMaui.EbicsContactInitStatus.Letters);
                            await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                        }
                        else
                        {
                            await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                        }
                        break;

                    case PermissionProMaui.EbicsContactInitStatus.Letters:
                        result = await _initService.SendInitialisationLetters(contact, password, _appUserSettingsService.GetUserMail());
                        if (result.EbicsErrorCode.Equals("000000"))
                        {
                            _veuService.UpdateIniPhase(contact, PermissionProMaui.EbicsContactInitStatus.NeedBankKeys);
                            await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                        }
                        else
                        {
                            await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                        }
                        break;

                    case PermissionProMaui.EbicsContactInitStatus.NeedBankKeys:
                        result = await _initService.SendHpbRequest(contact, password, _appUserSettingsService.UsePhoneTime());
                        if (result.EbicsErrorCode.Equals("000000"))
                        {
                            _veuService.UpdateIniPhase(contact, PermissionProMaui.EbicsContactInitStatus.FullyInitialized);
                            await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                        }
                        else
                        {
                            await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                        }
                        break;

                    case PermissionProMaui.EbicsContactInitStatus.FullyInitialized:
                        result = await _veuManagementService.SendHvzRequest(contact, password, _appUserSettingsService.UsePhoneTime());
                        if (result.EbicsErrorCode.Equals("000000"))
                        {
                            // Navigate to open orders or show success
                            await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                        }
                        else
                        {
                            await DisplayAlert(result.MessageBoxTitle, result.EbicsErrorMessage, "OK");
                        }
                        break;
                }

                // Refresh the contacts list
                LoadContacts();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void OnAddContactClicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Create EBICS contact", "Cancel", null, "Manual", "QR Code", "License Code");
            
            switch (action)
            {
                case "Manual":
                    await ShowManualContactCreationDialog();
                    break;
                case "QR Code":
                    await ShowQrCodeImportDialog();
                    break;
                case "License Code":
                    var code = await DisplayPromptAsync("License Code", "Enter license code", initialValue: string.Empty, maxLength: 128, keyboard: Keyboard.Text, placeholder: "License Code", accept: "OK", cancel: "Cancel");
                    if (!string.IsNullOrWhiteSpace(code))
                    {
                        var success = await _licensingService.LicencsingMethod(code);
                        if (success)
                        {
                            await DisplayAlert("Success", "License code processed successfully", "OK");
                            LoadContacts();
                        }
                        else
                        {
                            await DisplayAlert("Error", "Failed to process license code", "OK");
                        }
                    }
                    break;
            }
        }

        private async Task ShowManualContactCreationDialog()
        {
            var bankCode = await DisplayPromptAsync("Bank Code", "Enter bank code (BLZ)", initialValue: string.Empty, maxLength: 8, keyboard: Keyboard.Numeric, placeholder: "Bank Code", accept: "Search", cancel: "Cancel");
            
            if (!string.IsNullOrWhiteSpace(bankCode))
            {
                try
                {
                    // Simplified bank creation - skip server lookup for now
                    var bankName = await DisplayPromptAsync("Bank Name", "Bank name", initialValue: "", maxLength: 128, placeholder: "Bank Name", accept: "OK", cancel: "Cancel");
                    var hostId = await DisplayPromptAsync("Host ID", "Host ID", initialValue: "", maxLength: 128, placeholder: "Host ID", accept: "OK", cancel: "Cancel");
                    var serverUrl = await DisplayPromptAsync("Server URL", "Server URL", initialValue: "", maxLength: 256, placeholder: "Server URL", accept: "OK", cancel: "Cancel");
                    
                    if (!string.IsNullOrWhiteSpace(bankName) && !string.IsNullOrWhiteSpace(hostId) && !string.IsNullOrWhiteSpace(serverUrl))
                    {
                        // Create the bank and contact
                        var bank = new BankTable
                        {
                            Bankname = bankName,
                            HostId = hostId,
                            Uri = serverUrl,
                            InitPhase = PermissionProMaui.EbicsContactInitStatus.UnInitialized,
                            PublicBankKeys = ""
                        };
                        
                        var contact = new ContactTable
                        {
                            PartnerId = "",
                            UserId = "",
                            SignKey = "",
                            AuthKey = ""
                        };
                        
                        _veuService.CreateEbicsContact(bank, contact);
                        await DisplayAlert("Success", "Bank contact created successfully", "OK");
                        LoadContacts();
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to search for bank: {ex.Message}", "OK");
                }
            }
        }

        private async Task ShowQrCodeImportDialog()
        {
            var action = await DisplayActionSheet("QR Code Import", "Cancel", null, 
                "Scan QR Code", 
                "Enter Manually");

            if (action == "Scan QR Code")
            {
                await ScanQrCode();
            }
            else if (action == "Enter Manually")
            {
                await EnterQrCodeManually();
            }
        }

        private async Task ScanQrCode()
        {
            try
            {
                // Check camera permission
                var status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permission Required", "Camera permission is required to scan QR codes.", "OK");
                    return;
                }

                // For now, we'll use a manual input approach since ZXing.Net.Mobile is not included
                // In a real implementation, you would integrate a QR code scanning library
                await DisplayAlert("QR Code Scanning", "QR code scanning functionality will be implemented in a future update. Please use manual entry for now.", "OK");
                await EnterQrCodeManually();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to scan QR code: {ex.Message}", "OK");
            }
        }

        private async Task EnterQrCodeManually()
        {
            var qrContent = await DisplayPromptAsync("QR Code Import", 
                "Enter QR code content manually", 
                initialValue: string.Empty, 
                maxLength: 1000, 
                placeholder: "QR code content", 
                accept: "Import", 
                cancel: "Cancel");

            if (!string.IsNullOrWhiteSpace(qrContent))
            {
                try
                {
                    // Parse the QR code content
                    var exportContact = new ExportContactJson
                    {
                        KontaktFile = qrContent
                    };

                    // Prompt for passwords
                    var ebicsPassword = await DisplayPromptAsync("EBICS Password", 
                        "Enter EBICS password", 
                        initialValue: string.Empty, 
                        maxLength: 128, 
                        placeholder: "EBICS Password", 
                        accept: "OK", 
                        cancel: "Cancel");

                    if (string.IsNullOrWhiteSpace(ebicsPassword))
                        return;

                    var transportPassword = await DisplayPromptAsync("Transport Password", 
                        "Enter transport password", 
                        initialValue: string.Empty, 
                        maxLength: 128, 
                        placeholder: "Transport Password", 
                        accept: "OK", 
                        cancel: "Cancel");

                    if (string.IsNullOrWhiteSpace(transportPassword))
                        return;

                                    // Import the contact
                var importedContact = await _qrCodeService.ImportEbicsContact(exportContact, ebicsPassword, transportPassword);

                    if (importedContact != null)
                    {
                        await DisplayAlert("Success", "Contact imported successfully", "OK");
                        LoadContacts();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to import contact. Please check the QR code content and passwords.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to import contact: {ex.Message}", "OK");
                }
            }
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            try
            {
                Shell.Current.FlyoutIsPresented = true;
            }
            catch
            {
                await ShowFlyoutActions();
            }
        }

        private async void OnMenuTapped(object sender, TappedEventArgs e)
        {
            try
            {
                Shell.Current.FlyoutIsPresented = true;
            }
            catch
            {
                await ShowFlyoutActions();
            }
        }

        // Flyout menu actions - mirroring the original app's hamburger menu
        public async Task ShowFlyoutActions()
        {
            var action = await DisplayActionSheet("Menu", "Cancel", null, 
                "App Settings", 
                "Show Mail", 
                "Change Mail", 
                "Contact & Protocol", 
                "Support Contact", 
                "License", 
                "Error Logs", 
                "App Info", 
                "Impressum", 
                "Logout");

            switch (action)
            {
                case "App Settings":
                    try
                    {
                        await Shell.Current.GoToAsync("/SettingsPage");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Navigation Error", $"Could not navigate to Settings: {ex.Message}", "OK");
                    }
                    break;
                case "Show Mail":
                    await ShowMailDialog();
                    break;
                case "Change Mail":
                    await ChangeMailDialog();
                    break;
                case "Contact & Protocol":
                    await ShowContactAndProtocolDialog();
                    break;
                case "Support Contact":
                    await ShowSupportContactDialog();
                    break;
                case "License":
                    try
                    {
                        await Shell.Current.GoToAsync("/LicensePage");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Navigation Error", $"Could not navigate to License: {ex.Message}", "OK");
                    }
                    break;
                case "Error Logs":
                    await ShowErrorLogsDialog();
                    break;
                case "App Info":
                    await ShowAppInfoDialog();
                    break;
                case "Impressum":
                    await ShowImpressumDialog();
                    break;
                case "Logout":
                    await ShowLogoutDialog();
                    break;
            }
        }

        public async Task ShowMailDialog()
        {
            if (_appUserSettingsService == null)
            {
                await DisplayAlert("Error", "Email service not available. Please restart the app.", "OK");
                return;
            }
            try
            {
                var userMail = _appUserSettingsService.GetUserMail();
                await DisplayAlert("Current Email", $"Your email address:\n\n{userMail}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to get email: {ex.Message}", "OK");
            }
        }

        public async Task ChangeMailDialog()
        {
            if (_appUserSettingsService == null)
            {
                await DisplayAlert("Error", "Email service not available. Please restart the app.", "OK");
                return;
            }
            
            try
            {
                var currentMail = _appUserSettingsService.GetUserMail();
                var newMail = await DisplayPromptAsync("Change Email", "Enter new email address", 
                    initialValue: currentMail, 
                    maxLength: 128, 
                    keyboard: Keyboard.Email, 
                    placeholder: "Email address", 
                    accept: "OK", 
                    cancel: "Cancel");

                if (!string.IsNullOrWhiteSpace(newMail))
                {
                    if (string.IsNullOrWhiteSpace(newMail.Trim()))
                    {
                        await DisplayAlert("Error", "Email address cannot be empty", "OK");
                    }
                    else
                    {
                        _appUserSettingsService.UpdateUserMail(newMail.Trim());
                        await DisplayAlert("Success", "Email address updated successfully", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to change email: {ex.Message}", "OK");
            }
        }

        public async Task ShowContactAndProtocolDialog()
        {
            if (_licenceService == null)
            {
                await DisplayAlert("Error", "License service not available. Please restart the app.", "OK");
                return;
            }
            try
            {
                var supportContact = _licenceService.GetSupportContact();
                if (supportContact != null)
                {
                    var message = $"Support Contact:\n\nName: {supportContact.SupportContactname}\nEmail: {supportContact.SupportMailadress}\nPhone: {supportContact.SupportPhonenumber}";
                    await DisplayAlert("Contact & Protocol", message, "OK");
                }
                else
                {
                    await DisplayAlert("Contact & Protocol", "Support contact information not available.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to get support contact: {ex.Message}", "OK");
            }
        }

        private async Task ShowSupportContactDialog()
        {
            if (_licenceService == null)
            {
                await DisplayAlert("Error", "Services not initialized", "OK");
                return;
            }
            try
            {
                var supportContact = _licenceService.GetSupportContact();
                if (supportContact != null)
                {
                    var message = $"{supportContact.SupportContactname}\nEmail: {supportContact.SupportMailadress}\nTel: {supportContact.SupportPhonenumber}";
                    await DisplayAlert("Support Contact", message, "OK");
                }
                else
                {
                    await DisplayAlert("Support Contact", "Support contact information not available.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to get support contact: {ex.Message}", "OK");
            }
        }

        public async Task ShowErrorLogsDialog()
        {
            if (_errorProtocolService == null || _appUserSettingsService == null)
            {
                await DisplayAlert("Error", "Error logging service not available. Please restart the app.", "OK");
                return;
            }
            try
            {
                var errorLogs = _errorProtocolService.GetErrorProtocols();
                if (errorLogs != null && errorLogs.Count > 0)
                {
                    var message = $"Found {errorLogs.Count} error log(s).\n\nWould you like to send them to support?";
                    var send = await DisplayAlert("Error Logs", message, "Send", "Cancel");
                    if (send)
                    {
                        try
                        {
                            var userMail = _appUserSettingsService.GetUserMail();
                            var appVersion = GetAppVersion();
                            var additionalInfo = $"App Version: {appVersion}\nDevice: {DeviceInfo.Manufacturer} {DeviceInfo.Model}\nOS: {DeviceInfo.Platform} {DeviceInfo.VersionString}";
                            
                            await _errorProtocolService.SendErrorProtocols(userMail, additionalInfo);
                            await DisplayAlert("Success", "Error logs sent successfully to support.", "OK");
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Error", $"Failed to send error logs: {ex.Message}", "OK");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Error Logs", "No error logs found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to get error logs: {ex.Message}", "OK");
            }
        }

        private async Task ShowAppInfoDialog()
        {
            var appVersion = GetAppVersion();
            var message = $"App Version: {appVersion}\n\nThis is the MAUI version of the original Xamarin EBICS application.";
            await DisplayAlert("App Info", message, "OK");
        }

        private string GetAppVersion()
                    {
                        try
                        {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version;
                return version != null ? $"{version.Major}.{version.Minor}.{version.Build}" : "1.0.0";
            }
            catch
            {
                return "1.0.0";
            }
        }

        private async Task ShowImpressumDialog()
        {
            var message = "Impressum information can be found at:\nhttps://www.windata.de/impressum";
            var open = await DisplayAlert("Impressum", message, "Open Website", "Cancel");
            if (open)
            {
                try
                {
                    await Launcher.OpenAsync(new Uri("https://www.windata.de/impressum"));
            }
            catch (Exception ex)
            {
                    await DisplayAlert("Error", $"Could not open website: {ex.Message}", "OK");
                }
            }
        }

        public async Task ShowLogoutDialog()
        {
            if (_authService == null)
            {
                await DisplayAlert("Error", "Services not initialized", "OK");
                return;
            }
            var logout = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (logout)
            {
                try
                {
                    // Clear biometric authentication
                    _authService.RemoveBiometricAuthentification();
                    _authService.RemoveBiometricLogin();
                    
                    // Navigate to login page using relative route for logout
                    await Shell.Current.GoToAsync("/LoginPage");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to logout: {ex.Message}", "OK");
                }
            }
        }

        private async Task CreateManualBank()
        {
            try
            {
                var bankName = await DisplayPromptAsync("Bank Name", "Enter bank name", initialValue: string.Empty, maxLength: 128, keyboard: Keyboard.Text, placeholder: "Bank Name", accept: "OK", cancel: "Cancel");
                if (string.IsNullOrWhiteSpace(bankName))
                    return;

                var hostId = await DisplayPromptAsync("Host ID", "Enter host ID", initialValue: string.Empty, maxLength: 128, keyboard: Keyboard.Text, placeholder: "Host ID", accept: "OK", cancel: "Cancel");
                if (string.IsNullOrWhiteSpace(hostId))
                    return;

                var serverUrl = await DisplayPromptAsync("Server URL", "Enter server URL", initialValue: string.Empty, maxLength: 256, keyboard: Keyboard.Url, placeholder: "Server URL", accept: "OK", cancel: "Cancel");
                if (string.IsNullOrWhiteSpace(serverUrl))
                    return;

                // Create the bank and contact
                var bank = new BankTable
                {
                    Bankname = bankName.Trim(),
                    HostId = hostId.Trim(),
                    Uri = serverUrl.Trim(),
                    InitPhase = PermissionProMaui.EbicsContactInitStatus.UnInitialized,
                    PublicBankKeys = ""
                };

                var contact = new ContactTable
                {
                    PartnerId = "",
                    UserId = "",
                    SignKey = "",
                    AuthKey = ""
                };

                _veuService.CreateEbicsContact(bank, contact);
                await DisplayAlert("Success", "Bank contact created successfully", "OK");
                LoadContacts();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to create manual bank: {ex.Message}", "OK");
            }
        }
    }
}