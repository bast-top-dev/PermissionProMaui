using PermissionProMaui.Models;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for managing application user settings and preferences
    /// </summary>
    public class AppUserSettingsService
    {
        private readonly DatabaseService _databaseService;

        private AppSettingsTable _appSettings;
        private SystemTable _system;

        public AppUserSettingsService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        /// <summary>
        /// Gets the application settings from the database
        /// </summary>
        /// <returns>The application settings</returns>
        public AppSettingsTable GetAppSettings()
        {
            try
            {
                if (_appSettings == null)
                    _appSettings = _databaseService.AppSettingsRepo.GetAppSettings();
                return _appSettings;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting app settings: {ex.Message}");
                // Return default settings if database access fails
                return new AppSettingsTable
                {
                    EbicsProtocol = "false",
                    MoreInfoProtocol = "false",
                    UsePhoneTime = "false"
                };
            }
        }

        /// <summary>
        /// Updates the application settings in the database
        /// </summary>
        /// <param name="appSettings">The new application settings</param>
        public void UpdateAppSettings(AppSettingsTable appSettings)
        {
            try
            {
                _databaseService.AppSettingsRepo.UpdateAppSettings(appSettings);
                _appSettings = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating app settings: {ex.Message}");
                // Continue without updating if database access fails
            }
        }

        /// <summary>
        /// Checks if the app should use phone time
        /// </summary>
        /// <returns>True if phone time should be used</returns>
        public bool UsePhoneTime()
        {
            return bool.Parse(GetAppSettings().UsePhoneTime);
        }

        /// <summary>
        /// Checks if EBICS protocol is enabled
        /// </summary>
        /// <returns>True if EBICS protocol is enabled</returns>
        public bool EbicsProtocol()
        {
            return bool.Parse(GetAppSettings().EbicsProtocol);
        }

        /// <summary>
        /// Gets the current user information
        /// </summary>
        /// <returns>The user system table</returns>
        public SystemTable GetUser()
        {
            try
            {
                if (_system == null)
                    _system = _databaseService.SystemRepo.GetSystemSettings();
                return _system;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting user settings: {ex.Message}");
                // Return default user if database access fails
                return new SystemTable
                {
                    Username = "admin",
                    Mailadress = "admin@example.com",
                    LoginPassword = "admin123",
                    SavedKeyChain = "true",
                    PwAuthentification = "test123",
                    AuthKey = "default_auth_key",
                    SignKey = "default_sign_key",
                    ContactNumber = "1",
                    ActualVersion = "1.0.0"
                };
            }
        }

        /// <summary>
        /// Gets the user's email address
        /// </summary>
        /// <returns>The user's email address</returns>
        public string GetUserMail()
        {
            return GetUser().Mailadress;
        }

        /// <summary>
        /// Updates the user's email address
        /// </summary>
        /// <param name="mailAdress">The new email address</param>
        public void UpdateUserMail(string mailAdress)
        {
            try
            {
                _databaseService.SystemRepo.UpdateUserMail(mailAdress);
                _system = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating user mail: {ex.Message}");
                // Continue without updating if database access fails
            }
        }

        /// <summary>
        /// Gets the system tab view model test password
        /// </summary>
        /// <returns>The test password</returns>
        public string GetSystemTabViewModelTestPw()
        {
           return GetUser().PwAuthentification;
        }

        /// <summary>
        /// Clears the cached data
        /// </summary>
        public void ClearCache()
        {
            _appSettings = null;
            _system = null;
        }
    }
} 