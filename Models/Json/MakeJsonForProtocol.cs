namespace PermissionProMaui.Models.Json
{
    /// <summary>
    /// JSON model for creating error protocol entries.
    /// </summary>
    public class MakeJsonForProtocol
    {
        /// <summary>
        /// Email address for sending the protocol.
        /// </summary>
        public string Mailadress { get; set; }

        /// <summary>
        /// Error text description.
        /// </summary>
        public string ErrorText { get; set; }

        /// <summary>
        /// EBICS error protocol data.
        /// </summary>
        public string EbicsErrorProtocol { get; set; }
    }
} 