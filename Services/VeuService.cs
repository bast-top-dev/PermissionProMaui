using System.Collections.Generic;
using PermissionProMaui.Models;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for handling VEÜ (Verification of Electronic Usage) operations and EBICS contact management.
    /// </summary>
    public class VeuService
    {
        private readonly DatabaseService _databaseService;

        public VeuService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public DatabaseService DatabaseService => _databaseService;

        /// <summary>
        /// Gets all EBICS contacts from the database.
        /// </summary>
        /// <returns>List of all EBICS contacts.</returns>
        public List<EbicsContactModel> GetAllEbicsContacts()
        {
            try
            {
                return DatabaseService.EbicsContactRepo.GetAllEbicsContacts();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting EBICS contacts: {ex.Message}");
                // Return empty list if database access fails
                return new List<EbicsContactModel>();
            }
        }

        /// <summary>
        /// Gets an EBICS contact by its ID.
        /// </summary>
        /// <param name="contactId">The contact ID.</param>
        /// <returns>The EBICS contact model.</returns>
        public EbicsContactModel GetEbicsContact(int contactId)
        {
            return DatabaseService.EbicsContactRepo.FindEbicsContactById(contactId);
        }

        /// <summary>
        /// Gets an EBICS contact by user ID and partner ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="partnerId">The partner ID.</param>
        /// <returns>The EBICS contact model.</returns>
        public EbicsContactModel GetEbicsContact(string userId, string partnerId)
        {
            return DatabaseService.EbicsContactRepo.FindEbicsContactByUserPartnerId(userId, partnerId);
        }

        /// <summary>
        /// Gets a bank by its ID.
        /// </summary>
        /// <param name="id">The bank ID.</param>
        /// <returns>The bank table.</returns>
        public BankTable GetBank(int id)
        {
            return DatabaseService.EbicsContactRepo.FindBankById(id);
        }

        /// <summary>
        /// Gets a bank by its name.
        /// </summary>
        /// <param name="bankName">The bank name.</param>
        /// <returns>The bank table, or null if not found.</returns>
        public BankTable GetBank(string bankName)
        {
            return DatabaseService.EbicsContactRepo.FindBankByName(bankName);
        }

        /// <summary>
        /// Gets a bank by its host ID.
        /// </summary>
        /// <param name="hostId">The host ID.</param>
        /// <returns>The bank table, or null if not found.</returns>
        public BankTable GetBankByHostId(string hostId)
        {
            return DatabaseService.EbicsContactRepo.FindBankByHostId(hostId);
        }

        /// <summary>
        /// Gets the count of banks with a specific name.
        /// </summary>
        /// <param name="bankName">The bank name to search for.</param>
        /// <returns>The count of matching banks.</returns>
        public int GetBankCount(string bankName)
        {
            return DatabaseService.EbicsContactRepo.CountBanks(bankName);
        }

        /// <summary>
        /// Creates a new EBICS contact with bank and contact information.
        /// </summary>
        /// <param name="bank">The bank information.</param>
        /// <param name="contact">The contact information.</param>
        public void CreateEbicsContact(BankTable bank, ContactTable contact)
        {
            DatabaseService.EbicsContactRepo.CreateEbicsContact(bank, contact);
        }

        /// <summary>
        /// Deletes an EBICS contact by bank.
        /// </summary>
        /// <param name="bank">The bank to delete.</param>
        public void DeleteEbicsContact(BankTable bank)
        {
            DatabaseService.EbicsContactRepo.DeleteEbicsContact(bank);
        }

        /// <summary>
        /// Updates the initialization phase for an EBICS contact.
        /// </summary>
        /// <param name="contact">The EBICS contact.</param>
        /// <param name="phase">The new initialization phase.</param>
        public void UpdateIniPhase(EbicsContactModel contact, string phase)
        {
            DatabaseService.EbicsContactRepo.UpdateIniPhase(contact, phase);
        }

        /// <summary>
        /// Updates the keys for a contact.
        /// </summary>
        /// <param name="contact">The contact to update.</param>
        /// <param name="signKey">The signing key.</param>
        /// <param name="authKey">The authentication key.</param>
        /// <param name="encKey">The encryption key.</param>
        public void UpdateKeys(ContactTable contact, string signKey, string authKey, string encKey)
        {
            DatabaseService.EbicsContactRepo.UpdateKeys(contact, signKey, authKey, encKey);
        }

        /// <summary>
        /// Wipes inconsistent EBICS contacts by enforcing one-to-one relationships.
        /// </summary>
        public void WhipeInconsistentEbicsContacts()
        {
            DatabaseService.EbicsContactRepo.EnforceOneToOneRelationContactToBank();
        }

        /// <summary>
        /// Updates a bank in the database.
        /// </summary>
        /// <param name="bank">The bank to update.</param>
        public void UpdateBank(BankTable bank)
        {
            DatabaseService.EbicsContactRepo.UpdateBank(bank);
        }

        /// <summary>
        /// Updates the public bank keys for a specific bank.
        /// </summary>
        /// <param name="bankId">The bank ID.</param>
        /// <param name="pubBankKeys">The new public bank keys.</param>
        /// <returns>"OK" if successful.</returns>
        public string UpdatePublicBankKeys(int bankId, string pubBankKeys)
        {
            var bank = DatabaseService.EbicsContactRepo.FindBankById(bankId);
            bank.PublicBankKeys = pubBankKeys;
            DatabaseService.EbicsContactRepo.UpdateBank(bank);
            return "OK";
        }

        /// <summary>
        /// Updates a contact in the database.
        /// </summary>
        /// <param name="contact">The contact to update.</param>
        public void UpdateContact(ContactTable contact)
        {
            DatabaseService.EbicsContactRepo.UpdateContact(contact);
        }

        /// <summary>
        /// Updates the signing certificate for a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="signCert">The signing certificate.</param>
        public void UpdateSignCert(string userId, string signCert)
        {
            DatabaseService.EbicsContactRepo.UpdateSignCert(userId, signCert);
        }

        /// <summary>
        /// Updates the authentication certificate for a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="authCert">The authentication certificate.</param>
        public void UpdateAuthCert(string userId, string authCert)
        {
            DatabaseService.EbicsContactRepo.UpdateAuthCert(userId, authCert);
        }

        /// <summary>
        /// Updates the encryption certificate for a user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="encCert">The encryption certificate.</param>
        public void UpdateEncCert(string userId, string encCert)
        {
            DatabaseService.EbicsContactRepo.UpdateEncCert(userId, encCert);
        }
    }
} 