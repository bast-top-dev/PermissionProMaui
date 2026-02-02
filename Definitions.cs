namespace PermissionProMaui
{
    /// <summary>
    /// Contains application-wide constants and definitions.
    /// </summary>
    public class Definitions
    {
        /// <summary>
        /// Name of the main database file.
        /// </summary>
        public const string DatabaseName = "S3L4EB2CSA5PV1.sqlite";

        /// <summary>
        /// Name of the legacy database file.
        /// </summary>
        public const string DatabaseNameLegacy = "S3L4EB2CSA5P.sqlite";

        /// <summary>
        /// Security key name for biometric authentication.
        /// </summary>
        public const string Security_KeyName = "de.windata.biometric_authkey";

        /// <summary>
        /// Pattern for validating user ID and partner ID characters.
        /// </summary>
        public const string UserIdPartnerIdCharPattern = "[^a-zA-Z0-9-,=_]{1,35}";

        /// <summary>
        /// Demo email account for testing purposes.
        /// </summary>
        public const string DemoMailAccount = "app@test.de";
    }

    /// <summary>
    /// Contains constants for EBICS contact initialization status.
    /// </summary>
    public class EbicsContactInitStatus
    {
        /// <summary>
        /// Contact is not initialized.
        /// </summary>
        public const string UnInitialized = "Status: Uninitialisiert";

        /// <summary>
        /// Initialization failed (legacy status, not actively used).
        /// </summary>
        public const string IniFailed = "Status: Initialisierung fehlgeschlagen";

        /// <summary>
        /// Contact is partially initialized.
        /// </summary>
        public const string PartiallyInitialized = "Status: Teilweise Initialisiert";

        /// <summary>
        /// Initialization letters need to be sent.
        /// </summary>
        public const string Letters = "Status: INI-Briefe verschicken";

        /// <summary>
        /// Bank keys need to be retrieved.
        /// </summary>
        public const string NeedBankKeys = "Status: Bankschlüssel abrufen";

        /// <summary>
        /// Contact is fully initialized.
        /// </summary>
        public const string FullyInitialized = "Vollständig Initialisiert";
    }

    /// <summary>
    /// Contains constants for EBICS protocol versions.
    /// </summary>
    public class EbicsVersion
    {
        /// <summary>
        /// EBICS version 2.5.
        /// </summary>
        public const string EbicsV25 = "EBICS 2.5";

        /// <summary>
        /// EBICS version 3.0.
        /// </summary>
        public const string EbicsV30 = "EBICS 3.0";
    }

    /// <summary>
    /// Contains constants for application actions.
    /// </summary>
    public class Actions
    {
        /// <summary>
        /// Action to get open orders.
        /// </summary>
        public const string GetOpenOrders = "HVZ: Get Open Orders";
    }
} 