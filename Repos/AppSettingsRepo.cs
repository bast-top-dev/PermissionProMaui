using PermissionProMaui.Models;
using SQLite;

namespace PermissionProMaui.Repos
{
    /// <summary>
    /// Repository for accessing and updating application settings in the database.
    /// </summary>
    public class AppSettingsRepo
    {
        private readonly string _databasePath;

        public AppSettingsRepo(string databasePath)
        {
            _databasePath = databasePath;
        }

        /// <summary>
        /// Gets the application settings from the database.
        /// </summary>
        /// <returns>The AppSettingsTable entry with Id = 1, or null if not found.</returns>
        public AppSettingsTable GetAppSettings()
        {
            using var connection = new SQLiteConnection(_databasePath);
            return connection.Table<AppSettingsTable>().FirstOrDefault(asm => asm.Id == 1);
        }

        /// <summary>
        /// Updates the application settings in the database.
        /// </summary>
        /// <param name="appSettings">The new application settings.</param>
        public void UpdateAppSettings(AppSettingsTable appSettings)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            databaseConnection.Update(appSettings);
        }
    }
}