namespace PermissionProMaui.Models.Json
{
    /// <summary>
    /// JSON model for server information.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Regional center identifier.
        /// </summary>
        public string RZ { get; set; }

        /// <summary>
        /// Host identifier.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Server URL.
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Server name.
        /// </summary>
        public string Servername { get; set; }

        /// <summary>
        /// Bank name.
        /// </summary>
        public string Bank { get; set; }
    }
} 