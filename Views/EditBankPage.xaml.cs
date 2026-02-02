using Microsoft.Maui.Controls;
using PermissionProMaui.Models;
using PermissionProMaui.Services;
using System;
using System.Threading.Tasks;

namespace PermissionProMaui.Views
{
    public partial class EditBankPage : ContentPage
    {
        private VeuService _veuService;
        private readonly EbicsService _ebicsService = new EbicsService();
        private EbicsContactModel _ebicsContact;

        public EditBankPage(VeuService veuService, EbicsContactModel contact)
        {
            _veuService = veuService;
            InitializeComponent();
            _ebicsContact = contact;
            LoadContactData();
            BindingContext = this;
        }

        // Parameterless constructor for XAML instantiation
        public EditBankPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public string BankName { get; set; }
        public string HostId { get; set; }
        public string Uri { get; set; }
        public bool IsEbics25 { get; set; }
        public bool IsEbics30 { get; set; }
        public bool ShowConnectionCheck { get; set; }

        public EditBankPage(EbicsContactModel contact)
        {
            InitializeComponent();
            _ebicsContact = contact;
            LoadContactData();
            BindingContext = this;
        }

        private void LoadContactData()
        {
            BankName = _ebicsContact.Bank.Bankname;
            HostId = _ebicsContact.Bank.HostId;
            Uri = _ebicsContact.Bank.Uri;

            // Set EBICS version
            if (_ebicsContact.Contact.EbicsVersion == EbicsVersion.EbicsV25)
            {
                IsEbics25 = true;
                IsEbics30 = false;
            }
            else if (_ebicsContact.Contact.EbicsVersion == EbicsVersion.EbicsV30)
            {
                IsEbics25 = false;
                IsEbics30 = true;
            }
            else
            {
                ShowConnectionCheck = true;
                IsEbics25 = false;
                IsEbics30 = false;
            }
        }

        private async void OnConnectionCheckClicked(object sender, EventArgs e)
        {
            try
            {
                await DisplayAlert("Connection Check", "Testing connection to bank server...", "OK");
                
                // Implement actual connection check
                var supportedVersions = await _ebicsService.GetAllSupportedEbicsVersion(HostId, Uri);
                
                if (supportedVersions != null && supportedVersions.Count > 0)
                {
                    var versionMessage = "Supported EBICS versions:\n" + string.Join("\n", supportedVersions);
                    await DisplayAlert("Connection Successful", versionMessage, "OK");
                }
                else
                {
                    await DisplayAlert("Connection Failed", "Could not connect to the bank server or no supported versions found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Connection check failed: {ex.Message}", "OK");
            }
        }

        private async void OnSaveChangesClicked(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(BankName) || string.IsNullOrWhiteSpace(HostId) || string.IsNullOrWhiteSpace(Uri))
                {
                    await DisplayAlert("Validation Error", "Please fill in all required fields.", "OK");
                    return;
                }

                // Update bank information
                _ebicsContact.Bank.Bankname = BankName.Trim();
                _ebicsContact.Bank.HostId = HostId.Trim();
                _ebicsContact.Bank.Uri = Uri.Trim();

                // Update EBICS version
                if (IsEbics25)
                {
                    _ebicsContact.Contact.EbicsVersion = EbicsVersion.EbicsV25;
                }
                else if (IsEbics30)
                {
                    _ebicsContact.Contact.EbicsVersion = EbicsVersion.EbicsV30;
                }

                // Save changes
                _veuService.UpdateBank(_ebicsContact.Bank);
                _veuService.UpdateContact(_ebicsContact.Contact);

                await DisplayAlert("Success", "Bank information updated successfully.", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save changes: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteBankClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Confirm Delete", 
                "Are you sure you want to delete this bank? This action cannot be undone.", 
                "Delete", "Cancel");

            if (confirm)
            {
                try
                {
                    _veuService.DeleteEbicsContact(_ebicsContact.Bank);
                    await DisplayAlert("Success", "Bank deleted successfully.", "OK");
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete bank: {ex.Message}", "OK");
                }
            }
        }
    }
}
