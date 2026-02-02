using Microsoft.Maui.Controls;
using PermissionProMaui.Models;
using PermissionProMaui.Services;
using PermissionProMaui.Enums;
using System;
using System.Threading.Tasks;

namespace PermissionProMaui.Views
{
    public partial class EditContactPage : ContentPage
    {
        private VeuService _veuService;
        private InitialisationManagementService _initService;
        private AuthentificationService _authService;
        private AppUserSettingsService _appUserSettingsService;
        private ErrorProtocolService _errorProtocolService;
        private EbicsContactModel _ebicsContact;

        public string PartnerId { get; set; }
        public string UserId { get; set; }

        public EditContactPage(VeuService veuService, InitialisationManagementService initService, 
                             AuthentificationService authService, AppUserSettingsService appUserSettingsService,
                             ErrorProtocolService errorProtocolService, EbicsContactModel contact)
        {
            _veuService = veuService;
            _initService = initService;
            _authService = authService;
            _appUserSettingsService = appUserSettingsService;
            _errorProtocolService = errorProtocolService;
            InitializeComponent();
            _ebicsContact = contact;
            LoadContactData();
            BindingContext = this;
        }

        // Parameterless constructor for XAML instantiation
        public EditContactPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void LoadContactData()
        {
            PartnerId = _ebicsContact.Contact.PartnerId;
            UserId = _ebicsContact.Contact.UserId;
        }

        private async void OnGetBankKeysClicked(object sender, EventArgs e)
        {
            try
            {
                // Check if biometric authentication is set
                if (_authService.IsBiometricAuthentificationSet())
                {
                    var bio = new BiometricService();
                    var (ok, msg) = await bio.AuthenticateAsync("Authenticate", "Use your biometrics");
                    if (!ok)
                    {
                        await DisplayAlert("Warning", string.IsNullOrWhiteSpace(msg) ? "Biometric authentication failed" : msg, "OK");
                        return;
                    }
                    await GetBankKeys(null, true);
                }
                else
                {
                    string pw = await DisplayPromptAsync("Password", "Enter password", initialValue: string.Empty, maxLength: 128, keyboard: Keyboard.Text, placeholder: "Password", accept: "OK", cancel: "Cancel");
                    if (!string.IsNullOrWhiteSpace(pw))
                    {
                        await GetBankKeys(pw, false);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to get bank keys: {ex.Message}", "OK");
            }
        }

        private async Task GetBankKeys(string password, bool usedBiometric)
        {
            try
            {
                if (usedBiometric)
                {
                    var biometricKey = _authService.GetBiometricAuthentificationKey();
                    var biometricIv = _authService.GetBiometricAuthentificationIv();
                    
                    if (!string.IsNullOrEmpty(biometricKey) && !string.IsNullOrEmpty(biometricIv))
                    {
                        var cryptoService = new CryptoService();
                        password = cryptoService.Decrypt(biometricKey, biometricIv, KeyType.NoMigration);
                        
                        if (password == "Fehlerhaft")
                        {
                            await DisplayAlert("Error", "Failed to decrypt biometric password.", "OK");
                            return;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No biometric password stored.", "OK");
                        return;
                    }
                }

                var result = await _initService.SendHpbRequest(_ebicsContact, password, _appUserSettingsService.UsePhoneTime());
                
                if (result.EbicsErrorCode.Equals("000000"))
                {
                    _veuService.UpdateIniPhase(_ebicsContact, EbicsContactInitStatus.FullyInitialized);
                    await DisplayAlert("Success", "Bank keys downloaded successfully.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", result.EbicsErrorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                _errorProtocolService.WriteErrorProtocol("HPB Download EditContact", ex);
                await DisplayAlert("Error", $"Failed to get bank keys: {ex.Message}", "OK");
            }
        }

        private async void OnSendLettersClicked(object sender, EventArgs e)
        {
            try
            {
                // Check if biometric authentication is set
                if (_authService.IsBiometricAuthentificationSet())
                {
                    var bio = new BiometricService();
                    var (ok, msg) = await bio.AuthenticateAsync("Authenticate", "Use your biometrics");
                    if (!ok)
                    {
                        await DisplayAlert("Warning", string.IsNullOrWhiteSpace(msg) ? "Biometric authentication failed" : msg, "OK");
                        return;
                    }
                    await SendLetters(null, true);
                }
                else
                {
                    string pw = await DisplayPromptAsync("Password", "Enter password", initialValue: string.Empty, maxLength: 128, keyboard: Keyboard.Text, placeholder: "Password", accept: "OK", cancel: "Cancel");
                    if (!string.IsNullOrWhiteSpace(pw))
                    {
                        await SendLetters(pw, false);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to send letters: {ex.Message}", "OK");
            }
        }

        private async Task SendLetters(string password, bool usedBiometric)
        {
            try
            {
                if (usedBiometric)
                {
                    var biometricKey = _authService.GetBiometricAuthentificationKey();
                    var biometricIv = _authService.GetBiometricAuthentificationIv();
                    
                    if (!string.IsNullOrEmpty(biometricKey) && !string.IsNullOrEmpty(biometricIv))
                    {
                        var cryptoService = new CryptoService();
                        password = cryptoService.Decrypt(biometricKey, biometricIv, KeyType.NoMigration);
                        
                        if (password == "Fehlerhaft")
                        {
                            await DisplayAlert("Error", "Failed to decrypt biometric password.", "OK");
                            return;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No biometric password stored.", "OK");
                        return;
                    }
                }

                var result = await _initService.SendInitialisationLetters(_ebicsContact, password, _appUserSettingsService.GetUserMail());
                
                if (result.EbicsErrorCode.Equals("000000"))
                {
                    _veuService.UpdateIniPhase(_ebicsContact, EbicsContactInitStatus.NeedBankKeys);
                    await DisplayAlert("Success", "Initialization letters sent successfully.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", result.EbicsErrorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                _errorProtocolService.WriteErrorProtocol("Send Letters EditContact", ex);
                await DisplayAlert("Error", $"Failed to send letters: {ex.Message}", "OK");
            }
        }

        private async void OnSaveChangesClicked(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(PartnerId) || string.IsNullOrWhiteSpace(UserId))
                {
                    await DisplayAlert("Validation Error", "Please fill in all required fields.", "OK");
                    return;
                }

                // Check if values have changed
                if (PartnerId.Equals(_ebicsContact.Contact.PartnerId) && UserId.Equals(_ebicsContact.Contact.UserId))
                {
                    await DisplayAlert("Info", "No changes detected.", "OK");
                    return;
                }

                // Update contact information
                _ebicsContact.Contact.PartnerId = PartnerId.Trim();
                _ebicsContact.Contact.UserId = UserId.Trim();

                // Save changes
                _veuService.UpdateContact(_ebicsContact.Contact);

                // Reset initialization phase if contact details changed
                if (!PartnerId.Equals(_ebicsContact.Contact.PartnerId) || !UserId.Equals(_ebicsContact.Contact.UserId))
                {
                    _veuService.UpdateIniPhase(_ebicsContact, EbicsContactInitStatus.UnInitialized);
                }

                await DisplayAlert("Success", "Contact information updated successfully.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save changes: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
