using SQLite;

namespace PermissionProMaui.Models
{
    /// <summary>
    /// Database table for application licensing information
    /// </summary>
    public class CodeLicenceTable
    {
        /// <summary>
        /// Primary key and auto-increment identifier
        /// </summary>
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        /// <summary>
        /// License code for the application
        /// </summary>
        public string LicenceCode { get; set; }

        /// <summary>
        /// Flag indicating if the app is licensed
        /// </summary>
        public string IsAppLicenced { get; set; }

        /// <summary>
        /// Test end date for trial version
        /// </summary>
        public string TestEndDate { get; set; }
    }
} 