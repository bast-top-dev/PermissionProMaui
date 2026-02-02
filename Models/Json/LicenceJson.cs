namespace PermissionProMaui.Models.Json
{
    /// <summary>
    /// JSON model for license information
    /// </summary>
    public class LicenceJson
    {
        /// <summary>
        /// Institute name
        /// </summary>
        public string Institute { get; set; }

        /// <summary>
        /// Host identifier
        /// </summary>
        public string HostId { get; set; }

        /// <summary>
        /// Server URL
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// Header background color
        /// </summary>
        public string HeaderColor { get; set; }

        /// <summary>
        /// Header icon path or name
        /// </summary>
        public string HeaderIcon { get; set; }

        /// <summary>
        /// Footer background color
        /// </summary>
        public string FooterColor { get; set; }

        /// <summary>
        /// Header text color
        /// </summary>
        public string HeaderTextColor { get; set; }

        /// <summary>
        /// Header text content
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Splash screen icon path or name
        /// </summary>
        public string SplashScreenIcon { get; set; }

        /// <summary>
        /// Customer identifier
        /// </summary>
        public string KundenId { get; set; }

        /// <summary>
        /// User identifier
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// License code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Support email address
        /// </summary>
        public string SupportMailadress { get; set; }

        /// <summary>
        /// Support contact phone number
        /// </summary>
        public string SupportContactPhonenumber { get; set; }

        /// <summary>
        /// Flag indicating if the license is used
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// Flag indicating if this is a license key
        /// </summary>
        public bool IsLicencekey { get; set; }
    }
} 