using PermissionProMaui.Models;
using SQLite;

namespace PermissionProMaui.Repos
{
    /// <summary>
    /// Repository for accessing and updating code license information in the database.
    /// </summary>
    public class CodeLicenceRepo
    {
        private readonly string _databasePath;

        public CodeLicenceRepo(string databasePath)
        {
            _databasePath = databasePath;
        }

        /// <summary>
        /// Gets the code license from the database.
        /// </summary>
        /// <returns>The CodeLicenceTable entry with Id = 1, or null if not found.</returns>
        public CodeLicenceTable GetCodeLicense()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            return databaseConnection.Table<CodeLicenceTable>().FirstOrDefault(c => c.Id == 1);
        }

        /// <summary>
        /// Updates the code license in the database.
        /// </summary>
        /// <param name="licenseCode">The new license code.</param>
        public void UpdateCodeLicense(string licenseCode)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            CodeLicenceTable updateQuery = databaseConnection.Table<CodeLicenceTable>().First(c => c.Id == 1);

            updateQuery.LicenceCode = licenseCode;
            updateQuery.IsAppLicenced = "True";

            databaseConnection.Update(updateQuery);
        }
    }
}