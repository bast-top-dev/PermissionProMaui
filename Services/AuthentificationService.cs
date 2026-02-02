using PermissionProMaui.Enums;
using PermissionProMaui.Models;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for handling authentication and biometric login functionality.
    /// </summary>
    public class AuthentificationService
    {
        private readonly DatabaseService _databaseService = new DatabaseService();
        private readonly CryptoService _cryptoService = new CryptoService();

        /// <summary>
        /// Checks if the application is running in demo mode.
        /// </summary>
        /// <returns>True if the app is in demo mode, false otherwise.</returns>
        public bool IsAppInDemoMode()
        {
            var system = _databaseService.SystemRepo.GetSystemSettings();
            return system.Mailadress == Definitions.DemoMailAccount;
        }

        /// <summary>
        /// Validates the login password.
        /// </summary>
        /// <param name="pw">The password to check.</param>
        /// <returns>True if the password is valid, false otherwise.</returns>
        public bool CheckLoginPassword(string pw)
        {
            var system = _databaseService.SystemRepo.GetSystemSettings();

            // The stored password is encrypted with the password itself as the key
            // So we decrypt it using the provided password as the key
            string result = _cryptoService.Decrypt(pw, system.LoginPassword, KeyType.NoMigration);

            return result == pw;
        }

        /// <summary>
        /// Changes the login password and removes biometric login.
        /// </summary>
        /// <param name="newpw">The new password.</param>
        public void ChangeLoginPassword(string newpw)
        {
            var newpwencrypted = _cryptoService.Encrypt(newpw, newpw);
            _databaseService.SystemRepo.UpdateLoginPassword(newpwencrypted);
            _databaseService.SystemRepo.RemoveBiometricLogin();
        }

        /// <summary>
        /// Sets biometric login with encrypted password and initialization vector.
        /// </summary>
        /// <param name="encryptedPw">The encrypted password.</param>
        /// <param name="iv">The initialization vector.</param>
        public void SetBiometricLogin(string encryptedPw, string iv)
        {
            _databaseService.SystemRepo.SetBiometricLogin(encryptedPw + "###" + iv);
        }

        /// <summary>
        /// Removes biometric login configuration.
        /// </summary>
        public void RemoveBiometricLogin()
        {
            _databaseService.SystemRepo.RemoveBiometricLogin();
        }

        /// <summary>
        /// Validates the authentication password.
        /// </summary>
        /// <param name="pw">The password to check.</param>
        /// <returns>True if the password is valid, false otherwise.</returns>
        public bool CheckAuthentificationPassword(string pw)
        {
            var system = _databaseService.SystemRepo.GetSystemSettings();

            string result = _cryptoService.Decrypt(pw, system.PwAuthentification, KeyType.Ebics);

            return result != "Fehlerhaft";
        }

        /// <summary>
        /// Changes the authentication password and updates all EBICS contact keys.
        /// </summary>
        /// <param name="oldpw">The old password.</param>
        /// <param name="newpw">The new password.</param>
        public void ChangeAuthentificationPassword(string oldpw, string newpw)
        {
            var newpwencrypted = _cryptoService.Encrypt(newpw, "FlorianSchuster");
            _databaseService.SystemRepo.UpdateAuthentificationPassword(newpwencrypted);

            var contacts = _databaseService.EbicsContactRepo.GetAllEbicsContacts();

            foreach (EbicsContactModel contact in contacts)
            {
                var newSignKey = _cryptoService.Encrypt(newpw, _cryptoService.Decrypt(oldpw, contact.Contact.SignKey, KeyType.NoMigration));
                var newAuthKey = _cryptoService.Encrypt(newpw, _cryptoService.Decrypt(oldpw, contact.Contact.AuthKey, KeyType.NoMigration));
                var newEncKey = _cryptoService.Encrypt(newpw, _cryptoService.Decrypt(oldpw, contact.Contact.EncKey, KeyType.NoMigration));
                _databaseService.EbicsContactRepo.UpdateKeys(contact.Contact, newSignKey, newAuthKey, newEncKey);
            }

            _databaseService.SystemRepo.UpdateEbicsKeys("", "");
            _databaseService.SystemRepo.RemoveBiometricAuthentification();
        }

        /// <summary>
        /// Sets biometric authentication with encrypted password and initialization vector.
        /// </summary>
        /// <param name="encryptedPw">The encrypted password.</param>
        /// <param name="iv">The initialization vector.</param>
        public void SetBiometricAuthentification(string encryptedPw, string iv)
        {
            _databaseService.SystemRepo.SetBiometricAuthentification(encryptedPw + "###" + iv);
        }

        /// <summary>
        /// Removes biometric authentication configuration.
        /// </summary>
        public void RemoveBiometricAuthentification()
        {
            _databaseService.SystemRepo.RemoveBiometricAuthentification();
        }

        /// <summary>
        /// Gets the biometric authentication key.
        /// </summary>
        /// <returns>The biometric authentication key.</returns>
        public string GetBiometricAuthentificationKey()
        {
            var biometricAuthVal = _databaseService.SystemRepo.GetBiometricAuthentification();
            return biometricAuthVal.Split("###")[0];
        }

        /// <summary>
        /// Gets the biometric authentication initialization vector.
        /// </summary>
        /// <returns>The biometric authentication IV.</returns>
        public string GetBiometricAuthentificationIv()
        {
            var biometricAuthVal = _databaseService.SystemRepo.GetBiometricAuthentification();
            return biometricAuthVal.Split("###")[1];
        }

        /// <summary>
        /// Gets the biometric login key.
        /// </summary>
        /// <returns>The biometric login key.</returns>
        public string GetBiometricLoginKey()
        {
            var biometricAuthVal = _databaseService.SystemRepo.GetBiometricLogin();
            return biometricAuthVal.Split("###")[0];
        }

        /// <summary>
        /// Gets the biometric login initialization vector.
        /// </summary>
        /// <returns>The biometric login IV.</returns>
        public string GetBiometricLoginIv()
        {
            var biometricAuthVal = _databaseService.SystemRepo.GetBiometricLogin();
            return biometricAuthVal.Split("###")[1];
        }

        /// <summary>
        /// Checks if biometric login is configured.
        /// </summary>
        /// <returns>True if biometric login is set, false otherwise.</returns>
        public bool IsBiometricLoginSet()
        {
            return _databaseService.SystemRepo.IsBiometricLoginSet();
        }

        /// <summary>
        /// Checks if biometric authentication is configured.
        /// </summary>
        /// <returns>True if biometric authentication is set, false otherwise.</returns>
        public bool IsBiometricAuthentificationSet()
        {
            return _databaseService.SystemRepo.IsBiometricAuthentificationSet();
        }
    }
} 