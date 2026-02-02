namespace PermissionProMaui.Models.Json
{
    /// <summary>
    /// JSON model for initialization letters.
    /// </summary>
    public class InitLetterJson
    {
        /// <summary>
        /// Email address for sending letters.
        /// </summary>
        public string MailAdress { get; set; }

        /// <summary>
        /// Initialization letters content.
        /// </summary>
        public string Letters { get; set; }

        /// <summary>
        /// User identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Partner identifier.
        /// </summary>
        public string PartnerId { get; set; }
    }
} 