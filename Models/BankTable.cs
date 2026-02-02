using SQLite;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Database table for bank information
    /// </summary>
    public class BankTable
    {
        /// <summary>
        /// Primary key and auto-increment identifier
        /// </summary>
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// Name of the bank
        /// </summary>
        public string Bankname { get; set; }

        /// <summary>
        /// Host identifier for the bank
        /// </summary>
        public string HostId { get; set; }

        /// <summary>
        /// URI for the bank's EBICS endpoint
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Initialization phase information
        /// </summary>
        public string InitPhase { get; set; }

        /// <summary>
        /// Public bank keys
        /// </summary>
        public string PublicBankKeys { get; set; }

        /// <summary>
        /// Border brush color for UI
        /// </summary>
        public string BorderBrush { get; set; }

        /// <summary>
        /// Image location for bank logo
        /// </summary>
        public string ImageLocation { get; set; }

        /// <summary>
        /// To-do text for the bank
        /// </summary>
        public string ToDoText { get; set; }
    }
} 