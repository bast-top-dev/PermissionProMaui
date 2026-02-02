namespace PermissionProMaui.Models.Json
{
    /// <summary>
    /// JSON model for code and device information
    /// </summary>
    public class CodeJson
    {
        /// <summary>
        /// Code value
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Device name
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Serial number of the device
        /// </summary>
        public string SerialNumber { get; set; }
    }
}