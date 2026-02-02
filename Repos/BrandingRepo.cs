using PermissionProMaui.Models;
using PermissionProMaui.Models.Json;
using SQLite;

namespace PermissionProMaui.Repos
{
    /// <summary>
    /// Repository for accessing and updating branding information in the database.
    /// </summary>
    public class BrandingRepo
    {
        private readonly string _databasePath;

        public BrandingRepo(string databasePath)
        {
            _databasePath = databasePath;
        }

        /// <summary>
        /// Gets the branding information from the database.
        /// </summary>
        /// <returns>The BrandingTable entry, or null if not found.</returns>
        public BrandingTable GetBranding()
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            return databaseConnection.Table<BrandingTable>().FirstOrDefault();
        }

        /// <summary>
        /// Updates the branding information in the database using a LicenceJson object.
        /// </summary>
        /// <param name="branding">The new branding information.</param>
        public void UpdateBranding(LicenceJson branding)
        {
            using var databaseConnection = new SQLiteConnection(_databasePath);
            BrandingTable updateQuery = databaseConnection.Table<BrandingTable>().First(c => c.Id == 1);

            updateQuery.HeaderIcon = branding.HeaderIcon;
            updateQuery.HeaderColor = branding.HeaderColor;
            updateQuery.HeaderText = branding.HeaderText;
            updateQuery.FooterColor = branding.FooterColor;
            updateQuery.SplashScreen = branding.SplashScreenIcon;
            updateQuery.HeaderTextColor = branding.HeaderTextColor;

            databaseConnection.Update(updateQuery);
        }
    }
}