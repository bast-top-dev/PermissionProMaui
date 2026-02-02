using SQLite;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Database table for EBICS contacts
    /// </summary>
    public class ContactTable
    {
        /// <summary>
        /// Primary key and auto-increment identifier
        /// </summary>
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// Partner identifier
        /// </summary>
        public string PartnerId { get; set; }

        /// <summary>
        /// User identifier
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Bank identifier
        /// </summary>
        public int BankId { get; set; }

        /// <summary>
        /// Signing certificate
        /// </summary>
        public string SignCert { get; set; }

        /// <summary>
        /// Authentication certificate
        /// </summary>
        public string AuthCert { get; set; }

        /// <summary>
        /// Encryption certificate
        /// </summary>
        public string EncCert { get; set; }

        /// <summary>
        /// Signing key
        /// </summary>
        public string SignKey { get; set; }

        /// <summary>
        /// Authentication key
        /// </summary>
        public string AuthKey { get; set; }

        /// <summary>
        /// EBICS version
        /// </summary>
        public string EbicsVersion { get; set; }

        /// <summary>
        /// Encryption key
        /// </summary>
        public string EncKey { get; set; }
    }
} 