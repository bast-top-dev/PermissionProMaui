using SQLite;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Database table for error protocol settings
    /// </summary>
    public class SettingsTable
    {
        /// <summary>
        /// Primary key and auto-increment identifier
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Error protocol data in JSON format
        /// </summary>
        public string ErrorProtocol { get; set; }

        /// <summary>
        /// Name of the protocol
        /// </summary>
        public string ProtocolName { get; set; }

        /// <summary>
        /// Date of the protocol entry
        /// </summary>
        public string ProtocolDate { get; set; }
    }
} 