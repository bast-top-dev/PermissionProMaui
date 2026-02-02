namespace PermissionProMaui.Enums
{
    /// <summary>
    /// Enumeration for different types of cryptographic keys used in the application.
    /// </summary>
    public enum KeyType
    {
        /// <summary>
        /// Key used for login authentication.
        /// </summary>
        Login,

        /// <summary>
        /// Key used for EBICS protocol operations.
        /// </summary>
        Ebics,

        /// <summary>
        /// Key that should not be migrated during updates.
        /// </summary>
        NoMigration
    }
} 