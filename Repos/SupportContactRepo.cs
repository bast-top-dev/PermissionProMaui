using PermissionProMaui.Models;
using SQLite;

namespace PermissionProMaui.Repos
{
    /// <summary>
    /// Repository for accessing support contact information in the database.
    /// </summary>
    public class SupportContactRepo
    {
        private readonly string _databasePath;

        public SupportContactRepo(string databasePath)
        {
            _databasePath = databasePath;
        }

        /// <summary>
        /// Gets the support contact information from the database.
        /// </summary>
        /// <returns>The SupportContactTable entry, or null if not found.</returns>
        public SupportContactTable GetSupportContact()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            return databaseConnection.Table<SupportContactTable>().FirstOrDefault();
        }
    }
} 