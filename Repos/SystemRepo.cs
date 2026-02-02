using PermissionProMaui.Models;
using SQLite;

namespace PermissionProMaui.Repos
{
    /// <summary>
    /// Repository for managing system settings and user information in the database.
    /// </summary>
    public class SystemRepo
    {
        private readonly string _databasePath;

        public SystemRepo() { }

        public SystemRepo(string databasePath)
        {
            _databasePath = databasePath;
        }

        /// <summary>
        /// Gets the system settings from the database.
        /// </summary>
        /// <returns>The SystemTable entry with Id = 1, or null if not found.</returns>
        public SystemTable GetSystemSettings()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            return databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
        }

        /// <summary>
        /// Updates the login password in the database.
        /// </summary>
        /// <param name="newpwencrypted">The new encrypted password.</param>
        public virtual void UpdateLoginPassword(string newpwencrypted)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            SystemTable system = databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
            system.LoginPassword = newpwencrypted;
            databaseConnection.Update(system);
        }

        /// <summary>
        /// Checks if biometric login is set.
        /// </summary>
        /// <returns>True if biometric login is configured.</returns>
        public bool IsBiometricLoginSet()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            SystemTable system = databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
            return !string.IsNullOrWhiteSpace(system.LoginBiometric) && system.LoginBiometric != "NULL";
        }

        /// <summary>
        /// Gets the biometric login secret.
        /// </summary>
        /// <returns>The biometric login secret.</returns>
        public string GetBiometricLogin()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            SystemTable system = databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
            return system.LoginBiometric;
        }

        /// <summary>
        /// Sets the biometric login secret.
        /// </summary>
        /// <param name="biometricLoginSecret">The biometric login secret.</param>
        public void SetBiometricLogin(string biometricLoginSecret)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            SystemTable system = databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
            system.LoginBiometric = biometricLoginSecret;
            databaseConnection.Update(system);
        }

        /// <summary>
        /// Removes the biometric login configuration.
        /// </summary>
        public void RemoveBiometricLogin()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            SystemTable system = databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
            system.LoginBiometric = string.Empty;
            databaseConnection.Update(system);
        }

        /// <summary>
        /// Updates the EBICS keys in the database.
        /// </summary>
        /// <param name="newAuthKey">The new authentication key.</param>
        /// <param name="newSignKey">The new signing key.</param>
        public virtual void UpdateEbicsKeys(string newAuthKey, string newSignKey)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            SystemTable system = databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
            system.AuthKey = newAuthKey;
            system.SignKey = newSignKey;
            databaseConnection.Update(system);
        }

        /// <summary>
        /// Updates the authentication password in the database.
        /// </summary>
        /// <param name="newpwencrypted">The new encrypted authentication password.</param>
        public virtual void UpdateAuthentificationPassword(string newpwencrypted)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            SystemTable system = databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
            system.PwAuthentification = newpwencrypted;
            databaseConnection.Update(system);
        }

        /// <summary>
        /// Checks if biometric authentication is set.
        /// </summary>
        /// <returns>True if biometric authentication is configured.</returns>
        public bool IsBiometricAuthentificationSet()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            SystemTable system = databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
            return !string.IsNullOrWhiteSpace(system.PwAuthentificatonBiometric) && system.PwAuthentificatonBiometric != "NULL";
        }

        /// <summary>
        /// Gets the biometric authentication secret.
        /// </summary>
        /// <returns>The biometric authentication secret.</returns>
        public string GetBiometricAuthentification()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            SystemTable system = databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
            return system.PwAuthentificatonBiometric;
        }

        /// <summary>
        /// Sets the biometric authentication secret.
        /// </summary>
        /// <param name="biometricAuthSecret">The biometric authentication secret.</param>
        public void SetBiometricAuthentification(string biometricAuthSecret)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            SystemTable system = databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
            system.PwAuthentificatonBiometric = biometricAuthSecret;
            databaseConnection.Update(system);
        }

        /// <summary>
        /// Removes the biometric authentication configuration.
        /// </summary>
        public void RemoveBiometricAuthentification()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            SystemTable system = databaseConnection.Table<SystemTable>().FirstOrDefault(c => c.Id == 1);
            system.PwAuthentificatonBiometric = string.Empty;
            databaseConnection.Update(system);
        }

        /// <summary>
        /// Updates the user's email address in the database.
        /// </summary>
        /// <param name="mailAdress">The new email address.</param>
        public void UpdateUserMail(string mailAdress)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            var system = databaseConnection.Table<SystemTable>().First(c => c.Id == 1);
            system.Mailadress = mailAdress;
            databaseConnection.Update(system);
        }
    }
} 