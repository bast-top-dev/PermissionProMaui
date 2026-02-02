using SQLite;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Database table for system settings and user information
    /// </summary>
    public class SystemTable
    {
        /// <summary>
        /// Primary key and auto-increment identifier
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Username for the application
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email address of the user
        /// </summary>
        public string Mailadress { get; set; }

        /// <summary>
        /// Login password for the application
        /// </summary>
        public string LoginPassword { get; set; }

        /// <summary>
        /// Biometric login information
        /// </summary>
        public string LoginBiometric { get; set; }

        /// <summary>
        /// Key chain information
        /// </summary>
        public string KeyChain { get; set; }

        /// <summary>
        /// Saved key chain information
        /// </summary>
        public string SavedKeyChain { get; set; }

        /// <summary>
        /// Password for authentication
        /// </summary>
        public string PwAuthentification { get; set; }

        /// <summary>
        /// Biometric password for authentication
        /// </summary>
        public string PwAuthentificatonBiometric { get; set; }

        /// <summary>
        /// Authentication key
        /// </summary>
        public string AuthKey { get; set; }

        /// <summary>
        /// Signing key
        /// </summary>
        public string SignKey { get; set; }

        /// <summary>
        /// Contact number
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// Actual version of the application
        /// </summary>
        public string ActualVersion { get; set; }
    }
} 