using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Linq;
using PermissionProMaui.Enums;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for encryption and decryption operations.
    /// </summary>
    public class CryptoService
    {
        public DatabaseService DatabaseService = new DatabaseService();

        /// <summary>
        /// Encrypts a message using the provided key.
        /// </summary>
        /// <param name="key">The key (password) to be used for encryption.</param>
        /// <param name="message">The message that should be encrypted.</param>
        /// <returns>The encrypted message as a base64 string.</returns>
        public string Encrypt(string key, string message)
        {
            try
            {
                var aesKey = SHA256.Create().ComputeHash(Encoding.Default.GetBytes(key)).Take(16).ToArray();
                var aesIv = aesKey;
                using var aesProvider = Aes.Create();
                aesProvider.KeySize = 128;
                aesProvider.Key = aesKey;
                aesProvider.IV = aesIv;

                var encryptor = aesProvider.CreateEncryptor(aesProvider.Key, aesProvider.IV);

                byte[] encryptedData;

                using (var input = new MemoryStream(Encoding.Default.GetBytes(message)))
                using (var output = new MemoryStream())
                {
                    using var cryptStream = new CryptoStream(output, encryptor, CryptoStreamMode.Write);
                    
                    var buffer = new byte[1024];
                    var read = input.Read(buffer, 0, buffer.Length);
                    while (read > 0)
                    {
                        cryptStream.Write(buffer, 0, read);
                        read = input.Read(buffer, 0, buffer.Length);
                    }
                    cryptStream.FlushFinalBlock();
                    encryptedData = output.ToArray();
                }

                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// Decrypts a digest to get the secret message using the provided key.
        /// </summary>
        /// <param name="key">The key (password) to be used for decryption.</param>
        /// <param name="digest">The encrypted message that should be decrypted.</param>
        /// <param name="type">The type of key for migration purposes.</param>
        /// <returns>The decrypted message or 'Fehlerhaft' if the key is wrong.</returns>
        public string Decrypt(string key, string digest, KeyType type)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(digest))
                {
                    throw new NullReferenceException("Cannot load RSA-Key");
                }

                digest = MigrateDecryptionIfNeeded(key, digest, type);

                var aesKey = SHA256.Create().ComputeHash(Encoding.Default.GetBytes(key)).Take(16).ToArray();
                var aesIv = aesKey;
                using var aesProvider = Aes.Create();
                aesProvider.KeySize = 128;
                aesProvider.Key = aesKey;
                aesProvider.IV = aesIv;

                var decryptor = aesProvider.CreateDecryptor(aesProvider.Key, aesProvider.IV);

                var output = new MemoryStream(Convert.FromBase64String(digest));

                using var cryptStream = new CryptoStream(output, decryptor, CryptoStreamMode.Read);
                
                var strReader = new StreamReader(cryptStream);
                string strRequest = strReader.ReadToEnd();

                return strRequest;
            }
            catch (Exception)
            {
                return "Fehlerhaft";
            }
        }

        /// <summary>
        /// Migrates decryption if needed based on the key type.
        /// </summary>
        /// <param name="key">The key for decryption.</param>
        /// <param name="digest">The digest to decrypt.</param>
        /// <param name="type">The type of key for migration.</param>
        /// <returns>The migrated digest or original digest if migration fails.</returns>
        private string MigrateDecryptionIfNeeded(string key, string digest, KeyType type)
        {
            try
            {
                var maxPasswordLenght = 16 - key.Length;

                var tmpKey = key;

                for (var i = 0; i < maxPasswordLenght; i++)
                    tmpKey += "0";

                var aesKey = Encoding.UTF8.GetBytes(tmpKey);

                var aesIv = aesKey;
                using var aesProvider = Aes.Create();
                aesProvider.KeySize = 128;
                aesProvider.Key = aesKey;
                aesProvider.IV = aesIv;

                var decryptor = aesProvider.CreateDecryptor(aesProvider.Key, aesProvider.IV);

                var resultData = Convert.FromBase64String(digest);

                var output = new MemoryStream(resultData);

                using var cryptStream = new CryptoStream(output, decryptor, CryptoStreamMode.Read);

                var strReader = new StreamReader(cryptStream);
                string strRequest = strReader.ReadToEnd();

                var migratedDigest = Encrypt(key, strRequest);

                if (type == KeyType.NoMigration) return migratedDigest;

                if (type == KeyType.Login)
                {
                    DatabaseService.SystemRepo.UpdateLoginPassword(migratedDigest);
                    return migratedDigest;
                }

                DatabaseService.SystemRepo.UpdateAuthentificationPassword(migratedDigest);
                return migratedDigest;
            }
            catch
            {
                return digest;
            }
        }
    }
} 