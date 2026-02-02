using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using PermissionProMaui.Models;
using PermissionProMaui.Services;
using PermissionProMaui.Enums;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;

namespace PermissionProMaui.Views
{
    public partial class QrCodeExportPage : ContentPage
    {
        private QrCodeService _qrCodeService;
        private AppUserSettingsService _appUserSettingsService;
        private ApiConnectionService _apiConnectionService;
        private CryptoService _cryptoService;
        private EbicsContactModel _ebicsContact;
        private string _exportFileName;

        public QrCodeExportPage(QrCodeService qrCodeService, AppUserSettingsService appUserSettingsService, 
                               ApiConnectionService apiConnectionService, CryptoService cryptoService, 
                               EbicsContactModel contact)
        {
            _qrCodeService = qrCodeService;
            _appUserSettingsService = appUserSettingsService;
            _apiConnectionService = apiConnectionService;
            _cryptoService = cryptoService;
            InitializeComponent();
            _ebicsContact = contact;
            LoadContactData();
        }

        // Parameterless constructor for XAML instantiation
        public QrCodeExportPage()
        {
            InitializeComponent();
        }

        private void LoadContactData()
        {
            string contactName = $"EBICS-Kontakt: {_ebicsContact.Bank.Bankname} {_ebicsContact.Contact.PartnerId} -- {_ebicsContact.Contact.UserId}";
            ContactNameLabel.Text = contactName;
        }

        private async void OnGenerateQrCodeClicked(object sender, EventArgs e)
        {
            try
            {
                // Check if contact is fully initialized
                if (_ebicsContact.Bank.InitPhase != EbicsContactInitStatus.FullyInitialized)
                {
                    await DisplayAlert("Error", "Contact must be fully initialized before export.", "OK");
                    return;
                }

                // Prompt for EBICS password
                string ebicsPassword = await DisplayPromptAsync("Enter Password", 
                    "Enter EBICS password", 
                    initialValue: string.Empty, 
                    maxLength: 128, 
                    keyboard: Keyboard.Text, 
                    placeholder: "EBICS Password", 
                    accept: "OK", 
                    cancel: "Cancel");

                if (string.IsNullOrWhiteSpace(ebicsPassword))
                    return;

                // Validate password
                var pwTest = _appUserSettingsService.GetSystemTabViewModelTestPw();
                if (_cryptoService.Decrypt(ebicsPassword, pwTest, KeyType.Ebics).Equals("Fehlerhaft"))
                {
                    await DisplayAlert("Warning", "Password is incorrect.", "OK");
                    return;
                }

                // Prompt for transport password
                string transportPassword = await DisplayPromptAsync("Enter Transport Password", 
                    "Enter transport password", 
                    initialValue: string.Empty, 
                    maxLength: 128, 
                    keyboard: Keyboard.Text, 
                    placeholder: "Transport Password", 
                    accept: "OK", 
                    cancel: "Cancel");

                if (string.IsNullOrWhiteSpace(transportPassword))
                    return;

                // Generate export
                await GenerateExport(ebicsPassword, transportPassword);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to generate QR code: {ex.Message}", "OK");
            }
        }

        private async Task GenerateExport(string ebicsPassword, string transportPassword)
        {
            try
            {
                // Generate export JSON
                var contactToExport = _qrCodeService.GenerateExportJson(_ebicsContact, ebicsPassword, transportPassword);

                // Create export file
                _exportFileName = await _apiConnectionService.CreateExport(contactToExport);

                // Generate QR code
                byte[] qrCodeBytes = _qrCodeService.GenerateQrCode(_exportFileName);

                // Convert to image
                var imageSource = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
                QrCodeImage.Source = imageSource;
                QrCodeImage.IsVisible = true;
                QrCodeStatusLabel.Text = "QR code generated successfully";
                QrCodeStatusLabel.TextColor = Colors.Green;

                // Enable share button
                var shareButton = this.FindByName<Button>("ShareButton");
                if (shareButton != null)
                {
                    shareButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to generate export: {ex.Message}", "OK");
            }
        }

        private async void OnShareClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_exportFileName))
                {
                    await DisplayAlert("Error", "No QR code generated yet.", "OK");
                    return;
                }

                // Create a temporary file for sharing
                var tempPath = Path.Combine(FileSystem.CacheDirectory, $"qr_export_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                await File.WriteAllTextAsync(tempPath, _exportFileName);

                // Share the file
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Share QR Code Export",
                    File = new ShareFile(tempPath)
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to share: {ex.Message}", "OK");
            }
        }
    }
}
