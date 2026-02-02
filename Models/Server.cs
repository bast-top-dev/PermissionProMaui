namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for server information
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Server host ID
        /// </summary>
        public string HostId { get; set; }

        /// <summary>
        /// Server URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Bank name
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Bank code (BLZ)
        /// </summary>
        public string Blz { get; set; }
    }
}

