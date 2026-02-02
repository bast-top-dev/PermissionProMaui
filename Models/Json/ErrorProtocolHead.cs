namespace PermissionProMaui.Models.Json;

/// <summary>
/// JSON model for error protocol header information
/// </summary>
public class ErrorProtocolHead
{
    /// <summary>
    /// Name of the phone/device
    /// </summary>
    public string PhoneName { get; set; }

    /// <summary>
    /// Operating system of the phone
    /// </summary>
    public string PhoneOS { get; set; }

    /// <summary>
    /// Version of the phone operating system
    /// </summary>
    public string PhoneOSVersion { get; set; }

    /// <summary>
    /// Date and time of the phone
    /// </summary>
    public string PhoneDateTime { get; set; }

    /// <summary>
    /// Application version
    /// </summary>
    public string AppVersion { get; set; }

    /// <summary>
    /// Device information
    /// </summary>
    public string DeviceInfo { get; set; }

    /// <summary>
    /// Timestamp of the error
    /// </summary>
    public string Timestamp { get; set; }
} 