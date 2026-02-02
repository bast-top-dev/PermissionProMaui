namespace PermissionProMaui.Models
{
    /// <summary>
    /// Model for EBICS contact information combining bank and contact data
    /// </summary>
    public class EbicsContactModel
    {
        /// <summary>
        /// Bank information
        /// </summary>
        public BankTable Bank { get; set; }

        /// <summary>
        /// Contact information (nullable)
        /// </summary>
        public ContactTable Contact { get; set; }
    }
} 