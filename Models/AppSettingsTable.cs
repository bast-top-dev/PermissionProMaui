using SQLite;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Database table for application settings
    /// </summary>
    public class AppSettingsTable
    {
        /// <summary>
        /// Primary key and auto-increment identifier
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Flag indicating if more info protocol is enabled
        /// </summary>
        public string MoreInfoProtocol { get; set; }

        /// <summary>
        /// Flag indicating if EBICS protocol is enabled
        /// </summary>
        public string EbicsProtocol { get; set; }

        /// <summary>
        /// Flag indicating if phone time should be used
        /// </summary>
        public string UsePhoneTime { get; set; }
    }
} 