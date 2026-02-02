using System;
using System.Threading.Tasks;
using PermissionProMaui.Models;
using PermissionProMaui.Models.Json;
using PermissionProMaui.Services;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for handling application licensing
    /// </summary>
    public class LicensingService
    {
        private readonly LicenceService _licenceService;

        public LicensingService(VeuService veuService, LicenceService licenceService)
        {
            _veuService = veuService;
            _licenceService = licenceService;
        }
        private readonly ApiConnectionService _apiConnectionService = new ApiConnectionService();
        private readonly VeuService _veuService;

        public LicensingService(VeuService veuService)
        {
            _veuService = veuService;
        }

        public async Task<bool> LicencsingMethod(string code)
        {
            try
            {
                var licenceJson = await _apiConnectionService.GetLicenceJsonWithCode(code);

                if (licenceJson == null)
                {
                    // Show error dialog
                    return false;
                }
                else if (licenceJson.IsUsed)
                {
                    // Show "already used" dialog
                    return false;
                }
                else
                {
                    switch (licenceJson.Code.Substring(0, 1))
                    {
                        case "E":
                            // Contact-only license
                            UpdateBranding(licenceJson);
                            await _apiConnectionService.SetCodeAsUsed(licenceJson.Code);
                            InitializeCodeContact(licenceJson);
                            break;
                        case "C":
                        case "L":
                            LicenceApp(licenceJson);
                            break;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return false;
            }
        }

        private void LicenceApp(LicenceJson licenceJson)
        {
            _licenceService.UpdateLicense(licenceJson.Code);

            if (licenceJson.Code.Substring(0, 1).Equals("C"))
            {
                InitializeCodeContact(licenceJson);
            }
            else
            {
                UpdateBranding(licenceJson);
                _apiConnectionService.SetCodeAsUsed(licenceJson.Code).Wait();
            }
        }

        private void UpdateBranding(LicenceJson licenceJson)
        {
            _licenceService.UpdateBranding(licenceJson);
        }

        private void InitializeCodeContact(LicenceJson licenceJson)
        {
            bool contactExists = _veuService.GetEbicsContact(licenceJson.UserId, licenceJson.KundenId)?.Contact != null;
            bool bankExists = _veuService.GetBankByHostId(licenceJson.HostId) != null;

            if (contactExists && bankExists)
            {
                // Contact and bank already exist
                _apiConnectionService.SetCodeAsUsed(licenceJson.Code).Wait();
                UpdateBranding(licenceJson);
                return;
            }

            if (!contactExists && bankExists)
            {
                licenceJson.Institute = licenceJson.Institute + " #" + _veuService.GetBankCount(licenceJson.Institute);
            }

            CreateBankAndContactWithLicence(licenceJson);
        }

        private void CreateBankAndContactWithLicence(LicenceJson licenceJson)
        {
            var bank = new BankTable
            {
                Bankname = licenceJson.Institute,
                Uri = licenceJson.ServerUrl,
                HostId = licenceJson.HostId,
                InitPhase = PermissionProMaui.EbicsContactInitStatus.UnInitialized
            };

            var contact = new ContactTable
            {
                PartnerId = licenceJson.KundenId,
                UserId = licenceJson.UserId
            };

            _veuService.CreateEbicsContact(bank, contact);
        }
    }
}

