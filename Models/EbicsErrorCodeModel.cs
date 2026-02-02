namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for EBICS error codes and messages
    /// </summary>
    public class EbicsErrorCodeModel
    {
        /// <summary>
        /// EBICS error code
        /// </summary>
        public string EbicsErrorCode { get; set; } = string.Empty;

        /// <summary>
        /// EBICS error message
        /// </summary>
        public string EbicsErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// MessageBox title for displaying the error
        /// </summary>
        public string MessageBoxTitle { get; set; } = string.Empty;
    }
}

