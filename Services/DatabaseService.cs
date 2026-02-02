using System;
using System.Collections.Generic;
using System.IO;
using PermissionProMaui.Models;
using PermissionProMaui.Repos;
using SQLite;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for managing database operations
    /// </summary>
    public class DatabaseService
    {
        public const int TargetDatabaseVersion = 2;

        public string DatabasePath { get; }

        public DatabaseService()
        {
            DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Definitions.DatabaseName);
#if NET8_0
            DatabasePath = DatabasePath.Replace("Documents/", "");
#endif

            // Initialize repositories lazily to avoid SQLite issues during startup
            _appSettingsRepo = new Lazy<AppSettingsRepo>(() => new AppSettingsRepo(DatabasePath));
            _ebicsContactRepo = new Lazy<EbicsContactRepo>(() => new EbicsContactRepo(DatabasePath));
            _brandingRepo = new Lazy<BrandingRepo>(() => new BrandingRepo(DatabasePath));
            _codeLicenceRepo = new Lazy<CodeLicenceRepo>(() => new CodeLicenceRepo(DatabasePath));
            _supportContactRepo = new Lazy<SupportContactRepo>(() => new SupportContactRepo(DatabasePath));
            _systemRepo = new Lazy<SystemRepo>(() => new SystemRepo(DatabasePath));
            _errorProtocolRepo = new Lazy<ErrorProtocolRepo>(() => new ErrorProtocolRepo(DatabasePath));
        }

        /// <summary>
        /// Initializes the database - should be called after DI container is built
        /// </summary>
        public void Initialize()
        {
            try
            {
                if (!DatabaseExists())
                {
                    CreateDatabase(
                        username: "admin",
                        password: "admin123",
                        mail: "admin@example.com",
                        savePw: "true",
                        authKey: "default_auth_key",
                        signKey: "default_sign_key",
                        contactNumber: "1",
                        actualVersion: "1.0.0",
                        headerText: "PermissionPro MAUI",
                        headerIcon: "icon.png",
                        headerColor: "#2196F3",
                        splashScreen: "splash.png",
                        testPwString: "test123",
                        headerTextColor: "#FFFFFF",
                        footerTextColor: "#FFFFFF"
                    );
                }
                else
                {
                    SafeUpdateDatabase();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
            }
        }

        private readonly Lazy<AppSettingsRepo> _appSettingsRepo;
        private readonly Lazy<EbicsContactRepo> _ebicsContactRepo;
        private readonly Lazy<BrandingRepo> _brandingRepo;
        private readonly Lazy<CodeLicenceRepo> _codeLicenceRepo;
        private readonly Lazy<SupportContactRepo> _supportContactRepo;
        private readonly Lazy<SystemRepo> _systemRepo;
        private readonly Lazy<ErrorProtocolRepo> _errorProtocolRepo;

        public AppSettingsRepo AppSettingsRepo => _appSettingsRepo.Value;
        public EbicsContactRepo EbicsContactRepo => _ebicsContactRepo.Value;
        public BrandingRepo BrandingRepo => _brandingRepo.Value;
        public CodeLicenceRepo CodeLicenceRepo => _codeLicenceRepo.Value;
        public SupportContactRepo SupportContactRepo => _supportContactRepo.Value;
        public virtual SystemRepo SystemRepo => _systemRepo.Value;
        public ErrorProtocolRepo ErrorProtocolRepo => _errorProtocolRepo.Value;

        /// <summary>
        /// Creates the database with initial data
        /// </summary>
        public void CreateDatabase(string username, string password, string mail, string savePw, string authKey, string signKey, string contactNumber, string actualVersion, string headerText, string headerIcon, string headerColor, string splashScreen, string testPwString, string headerTextColor, string footerTextColor)
        {
            var databaseConnection = new SQLiteConnection(DatabasePath);
            databaseConnection.CreateTable<SystemTable>();
            databaseConnection.CreateTable<ContactTable>();
            databaseConnection.CreateTable<BankTable>();
            databaseConnection.CreateTable<SettingsTable>();
            databaseConnection.CreateTable<BrandingTable>();
            databaseConnection.CreateTable<CodeLicenceTable>();
            databaseConnection.CreateTable<SupportContactTable>();
            databaseConnection.CreateTable<AppSettingsTable>();

            // Ensure login password is stored encrypted so login validation works
            var crypto = new CryptoService();
            var encryptedLoginPassword = crypto.Encrypt(password, password);
            databaseConnection.RunInTransaction(() => databaseConnection.Insert(new SystemTable { Username = username, Mailadress = mail, LoginPassword = encryptedLoginPassword, SavedKeyChain = savePw, PwAuthentification = testPwString, AuthKey = authKey, SignKey = signKey, ContactNumber = contactNumber, ActualVersion = actualVersion }));
            databaseConnection.RunInTransaction(() => databaseConnection.Insert(new BrandingTable { HeaderColor = headerColor, FooterColor = headerColor, FooterTextColor = footerTextColor, HeaderIcon = headerIcon, HeaderText = headerText, SplashScreen = splashScreen, HeaderTextColor = headerTextColor }));
            // Create a default license without dependencies to avoid circular references
            var defaultLicense = new CodeLicenceTable
            {
                LicenceCode = "default_license_key",
                IsAppLicenced = "true",
                TestEndDate = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd")
            };
            databaseConnection.RunInTransaction(() => databaseConnection.Insert(defaultLicense));
            databaseConnection.RunInTransaction(() => databaseConnection.Insert(new SupportContactTable { SupportContactname = "windata GmbH & Co. KG", SupportMailadress = "info@windata.de", SupportPhonenumber = "+49 7522 9770-0" }));
            databaseConnection.RunInTransaction(() => databaseConnection.Insert(new AppSettingsTable { EbicsProtocol = "false", MoreInfoProtocol = "false", UsePhoneTime = "false" }));

            databaseConnection.Execute($"PRAGMA user_version = {TargetDatabaseVersion};");
        }

        /// <summary>
        /// Checks if the database needs to be updated
        /// </summary>
        /// <returns>True if database needs update</returns>
        public bool DatabaseNeedsUpdate()
        {
            using var databaseConnection = new SQLiteConnection(DatabasePath);
            
            int actualDatabaseVersion = databaseConnection.ExecuteScalar<int>("PRAGMA user_version");

            return actualDatabaseVersion != TargetDatabaseVersion;
        }

        /// <summary>
        /// Updates the database to the latest version
        /// </summary>
        public void UpdateDatabase()
        {
            try
            {
                using var databaseConnection = new SQLiteConnection(DatabasePath);

                int actualDatabaseVersion = databaseConnection.ExecuteScalar<int>("PRAGMA user_version");

                while (actualDatabaseVersion < TargetDatabaseVersion)
                {
                    switch (actualDatabaseVersion)
                    {
                        case 0:
                            databaseConnection.Execute("ALTER TABLE ContactTable ADD SignCert VARCHAR;");
                            databaseConnection.Execute("ALTER TABLE ContactTable ADD AuthCert VARCHAR;");
                            databaseConnection.Execute("ALTER TABLE ContactTable ADD EncCert VARCHAR;");
                            databaseConnection.Execute("ALTER TABLE SystemTable ADD LoginBiometric VARCHAR");
                            databaseConnection.Execute("ALTER TABLE SystemTable ADD PwAuthentificatonBiometric VARCHAR");
                            databaseConnection.Execute("ALTER TABLE ContactTable ADD SignKey VARCHAR;");
                            databaseConnection.Execute("ALTER TABLE ContactTable ADD AuthKey VARCHAR;");
                            databaseConnection.Execute("ALTER TABLE ContactTable ADD EbicsVersion VARCHAR;");

                            var keys = databaseConnection.Table<SystemTable>().FirstOrDefault();
                            //copy the key from the system table
                            if (keys != null)
                            {
                                foreach (ContactTable contact in databaseConnection.Table<ContactTable>())
                                {
                                    contact.SignKey = keys.SignKey;
                                    contact.AuthKey = keys.AuthKey;
                                    databaseConnection.Update(contact);
                                }
                            }
                            actualDatabaseVersion = 1;
                            databaseConnection.Execute("PRAGMA user_version = 1;");
                            break;

                        case 1:
                            databaseConnection.Execute("ALTER TABLE ContactTable ADD EncKey VARCHAR;");
                            actualDatabaseVersion = 2;
                            databaseConnection.Execute("PRAGMA user_version = 2;");
                            break;

                        default:
                            actualDatabaseVersion = TargetDatabaseVersion;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error and continue
                System.Diagnostics.Debug.WriteLine($"Database update failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Safely updates the database with error handling
        /// </summary>
        public void SafeUpdateDatabase()
        {
            try
            {
                if (DatabaseNeedsUpdate())
                {
                    UpdateDatabase();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Safe database update failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the current database version
        /// </summary>
        /// <returns>The database version as string</returns>
        public string GetDatabaseVersion()
        {
            using var databaseConnection = new SQLiteConnection(DatabasePath);
            int version = databaseConnection.ExecuteScalar<int>("PRAGMA user_version");
            return version.ToString();
        }

        /// <summary>
        /// Checks if the database exists
        /// </summary>
        /// <returns>True if database exists</returns>
        public bool DatabaseExists()
        {
            return File.Exists(DatabasePath);
        }

        /// <summary>
        /// Wipes the database (deletes the file)
        /// </summary>
        public void WhipeDatabase()
        {
            if (File.Exists(DatabasePath))
            {
                File.Delete(DatabasePath);
            }
        }
    }
} 