using System;
using PermissionProMaui.Models;
using PermissionProMaui.Services;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace PermissionProMaui.Repos
{
    /// <summary>
    /// Repository for managing EBICS contacts and banks in the database.
    /// </summary>
    public class EbicsContactRepo
    {
        private readonly string _databasePath;

        public EbicsContactRepo(string databasePath)
        {
            _databasePath = databasePath;
        }

        public BankTable FindBankById(int id)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            return databaseConnection.Table<BankTable>().FirstOrDefault(b => b.Id == id);
        }

        public BankTable FindBankByName(string bankName)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            return databaseConnection.Table<BankTable>().FirstOrDefault(b => b.Bankname == bankName);
        }

        public BankTable FindBankByHostId(string hostId)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            return databaseConnection.Table<BankTable>().FirstOrDefault(b => b.HostId.ToUpper() == hostId.ToUpper());
        }

        public List<BankTable> FindAllBanks()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            return databaseConnection.Table<BankTable>().ToList();
        }

        public List<ContactTable> FindAllContactTableEntries()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            return databaseConnection.Table<ContactTable>().ToList();
        }

        public void DeleteEbicsContact(ContactTable contact)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            var bank = databaseConnection.Table<BankTable>().FirstOrDefault(b => b.Id == contact.BankId);
            databaseConnection.Delete(contact);
            if (bank != null)
            {
                databaseConnection.Delete(bank);
            }
        }

        public void DeleteEbicsContact(BankTable bank)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            var contact = databaseConnection.Table<ContactTable>().FirstOrDefault(c => c.BankId == bank.Id);
            databaseConnection.Delete(bank);
            if (contact != null)
            {
                databaseConnection.Delete(contact);
            }
        }

        public int CountBanks(string bankName)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            return databaseConnection.Table<BankTable>().Count(b => b.Bankname.Contains(bankName));
        }

        public void CreateEbicsContact(BankTable bank, ContactTable contact)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            databaseConnection.Insert(bank);
            contact.BankId = bank.Id;
            databaseConnection.Insert(contact);
        }

        public List<EbicsContactModel> GetAllEbicsContacts()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            List<EbicsContactModel> ebicsContacts = new List<EbicsContactModel>();
            foreach (ContactTable contactTable in databaseConnection.Table<ContactTable>())
            {
                ebicsContacts.Add(new EbicsContactModel
                {
                    Contact = contactTable,
                    Bank = databaseConnection.Table<BankTable>().FirstOrDefault(b => b.Id == contactTable.BankId)
                });
            }
            return ebicsContacts;
        }

        public void UpdateIniPhase(EbicsContactModel contact, string phase)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            var bank = databaseConnection.Table<BankTable>().FirstOrDefault(b => b.Id == contact.Bank.Id);
            if (bank != null)
            {
                bank.InitPhase = phase;
                databaseConnection.Update(bank);
            }
        }

        public EbicsContactModel FindEbicsContactById(int contactId)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            var contactTable = databaseConnection.Table<ContactTable>().FirstOrDefault(c => c.Id == contactId);
            EbicsContactModel contact = new EbicsContactModel
            {
                Contact = contactTable
            };
            contact.Bank = databaseConnection.Table<BankTable>().FirstOrDefault(b => b.Id == contactTable.BankId);
            return contact;
        }

        public EbicsContactModel FindEbicsContactByUserPartnerId(string userId, string partnerId)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            EbicsContactModel contact = new EbicsContactModel
            {
                Contact = databaseConnection.Table<ContactTable>().FirstOrDefault(c => c.UserId == userId && c.PartnerId == partnerId)
            };
            contact.Bank = databaseConnection.Table<BankTable>().FirstOrDefault(b => b.Id == contact.Contact.BankId);
            return contact;
        }

        public void UpdateBank(BankTable bank)
        {
            if (bank.Id == 0) return;
            using var databaseConnection = new SQLiteConnection(_databasePath);
            databaseConnection.Update(bank);
        }

        public void UpdateContact(ContactTable contact)
        {
            if (contact.Id == 0) return;
            using var databaseConnection = new SQLiteConnection(_databasePath);
            databaseConnection.Update(contact);
        }

        public void UpdateSignCert(string userId, string signCert)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            var updateQuery = (databaseConnection.Table<ContactTable>().Where(c => c.UserId == userId)).First();
            updateQuery.SignCert = signCert;
            databaseConnection.Update(updateQuery);
        }

        public void UpdateAuthCert(string userId, string authCert)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            var updateQuery = (databaseConnection.Table<ContactTable>().Where(c => c.UserId == userId)).First();
            updateQuery.AuthCert = authCert;
            databaseConnection.Update(updateQuery);
        }

        public void UpdateEncCert(string userId, string encCert)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            var updateQuery = (databaseConnection.Table<ContactTable>().Where(c => c.UserId == userId)).First();
            updateQuery.EncCert = encCert;
            databaseConnection.Update(updateQuery);
        }

        public void UpdateKeys(ContactTable contact, string signKey, string authKey, string encKey)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            if (contact.Id == 0)
            {
                throw new InvalidOperationException("Can not update Contact with ID 0.");
            }
            var updateQuery = databaseConnection.Table<ContactTable>().FirstOrDefault(c => c.Id == contact.Id);
            updateQuery.SignKey = signKey;
            updateQuery.AuthKey = authKey;
            updateQuery.EncKey = encKey;
            databaseConnection.Update(updateQuery);
        }

        internal void EnforceOneToOneRelationContactToBank()
        {
            var contacts = FindAllContactTableEntries();
            var banksToDelete = FindAllBanks();
            foreach (ContactTable contactTable in contacts)
            {
                var bank = banksToDelete.FirstOrDefault(b => b.Id == contactTable.BankId);
                if (bank != null)
                {
                    banksToDelete.Remove(bank);
                }
            }
            foreach (BankTable bankTable in banksToDelete)
            {
                DeleteEbicsContact(bankTable);
            }
            //reverse process for contacts
            var contactToDelete = FindAllContactTableEntries();
            var banks = FindAllBanks();
            foreach (BankTable bank in banks)
            {
                var contact = contactToDelete.FirstOrDefault(b => b.BankId == bank.Id);
                if (contact != null)
                {
                    contactToDelete.Remove(contact);
                }
            }
            foreach (ContactTable contact in contactToDelete)
            {
                DeleteEbicsContact(contact);
            }
        }
    }
}