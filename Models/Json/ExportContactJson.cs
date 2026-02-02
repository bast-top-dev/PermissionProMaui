namespace PermissionProMaui.Models.Json;

/// <summary>
/// JSON model for exporting EBICS contacts.
/// </summary>
public class ExportContactJson
{
    /// <summary>
    /// Bank information.
    /// </summary>
    public ExportBankJson Bank { get; set; }

    /// <summary>
    /// Contact information.
    /// </summary>
    public ExportContactDataJson Contact { get; set; }

    /// <summary>
    /// EBICS password.
    /// </summary>
    public string EbicsPassword { get; set; }

    /// <summary>
    /// Transport password.
    /// </summary>
    public string TransportPassword { get; set; }

    /// <summary>
    /// Contact file data.
    /// </summary>
    public string KontaktFile { get; set; }

    /// <summary>
    /// Contact name.
    /// </summary>
    public string Name { get; set; }
}

/// <summary>
/// JSON model for bank information in exports.
/// </summary>
public class ExportBankJson
{
    public string Bankname { get; set; }
    public string HostId { get; set; }
    public string Uri { get; set; }
}

/// <summary>
/// JSON model for contact data in exports.
/// </summary>
public class ExportContactDataJson
{
    public string UserId { get; set; }
    public string PartnerId { get; set; }
    public string EbicsVersion { get; set; }
    public string SignKey { get; set; }
    public string SignCert { get; set; }
    public string AuthKey { get; set; }
    public string AuthCert { get; set; }
    public string EncKey { get; set; }
    public string EncCert { get; set; }
} 