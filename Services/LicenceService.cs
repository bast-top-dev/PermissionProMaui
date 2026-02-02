using PermissionProMaui.Models;
using System.Globalization;
using System;
using System.Threading.Tasks;
using PermissionProMaui.Models.Json;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for managing application licensing
    /// </summary>
    public class LicenceService
    {
        private readonly DatabaseService _databaseService;
        private readonly ApiConnectionService _apiConnectionService;

        public LicenceService(DatabaseService databaseService, ApiConnectionService apiConnectionService)
        {
            _databaseService = databaseService;
            _apiConnectionService = apiConnectionService;
        }

        /// <summary>
        /// Creates a default license for testing
        /// </summary>
        /// <returns>The default license</returns>
        public CodeLicenceTable CreateDefaultLicence()
        {
            string berlinTimeStampString = _apiConnectionService.GetBerlinTimeStamp().Result;
            DateTime berlinTimeStamp = DateTime.ParseExact(berlinTimeStampString, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
            berlinTimeStamp = berlinTimeStamp.AddDays(30);

            return new CodeLicenceTable
            {
                IsAppLicenced = "False",
                LicenceCode = "Nicht lizenziert -- Testversion",
                TestEndDate = berlinTimeStamp.ToShortDateString()
            };
        }

        /// <summary>
        /// Checks if the license has expired
        /// </summary>
        /// <returns>True if license is expired</returns>
        public async Task<bool> IsLicenceExpired()
        {
            var actualLicense = _databaseService.CodeLicenceRepo.GetCodeLicense();

            string berlinTimeStampString = await _apiConnectionService.GetBerlinTimeStamp();
            var berlinTimeStamp = DateTime.ParseExact(berlinTimeStampString, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
            
            if (!bool.Parse(actualLicense.IsAppLicenced))
            {
                return DateTime.Parse(actualLicense.TestEndDate) < berlinTimeStamp;
            }

            return !(await _apiConnectionService.GetLicenseStatus(actualLicense.LicenceCode));
        }

        /// <summary>
        /// Gets the current code license
        /// </summary>
        /// <returns>The code license</returns>
        public CodeLicenceTable GetCodeLicense()
        {
            try
            {
                return _databaseService.CodeLicenceRepo.GetCodeLicense();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting code license: {ex.Message}");
                // Return default license if database access fails
                return CreateDefaultLicence();
            }
        }

        /// <summary>
        /// Updates the license with a new code
        /// </summary>
        /// <param name="licenseCode">The new license code</param>
        public void UpdateLicense(string licenseCode)
        {
            _databaseService.CodeLicenceRepo.UpdateCodeLicense(licenseCode);
        }

        /// <summary>
        /// Gets the branding information
        /// </summary>
        /// <returns>The branding table</returns>
        public BrandingTable GetBranding()
        {
            try
            {
                return _databaseService.BrandingRepo.GetBranding();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting branding: {ex.Message}");
                // Return default branding if database access fails
                return new BrandingTable
                {
                    HeaderColor = "#2196F3",
                    FooterColor = "#2196F3",
                    FooterTextColor = "#FFFFFF",
                    HeaderIcon = "icon.png",
                    HeaderText = "PermissionPro MAUI",
                    SplashScreen = "splash.png",
                    HeaderTextColor = "#FFFFFF"
                };
            }
        }

        /// <summary>
        /// Updates the branding with new information
        /// </summary>
        /// <param name="newBranding">The new branding information</param>
        public void UpdateBranding(LicenceJson newBranding)
        {
            _databaseService.BrandingRepo.UpdateBranding(newBranding);
        }

        /// <summary>
        /// Gets the support contact information
        /// </summary>
        /// <returns>The support contact table</returns>
        public SupportContactTable GetSupportContact()
        {
            try
            {
                return _databaseService.SupportContactRepo.GetSupportContact();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting support contact: {ex.Message}");
                // Return default support contact if database access fails
                return new SupportContactTable
                {
                    SupportContactname = "windata GmbH & Co. KG",
                    SupportMailadress = "info@windata.de",
                    SupportPhonenumber = "+49 7522 9770-0"
                };
            }
        }
    }
} 