namespace PermissionProMaui.Models.Json
{
    /// <summary>
    /// JSON model for database information
    /// </summary>
    public class DbJson
    {
        /// <summary>
        /// Database content in Base64 format
        /// </summary>
        public string DbBase64 { get; set; }

        /// <summary>
        /// Email address for sending database
        /// </summary>
        public string MailAdress { get; set; }
    }
} 