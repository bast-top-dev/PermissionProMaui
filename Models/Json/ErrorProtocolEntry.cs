namespace PermissionProMaui.Models.Json;

/// <summary>
/// JSON model for error protocol entry
/// </summary>
public class ErrorProtocolEntry
{
    /// <summary>
    /// Action that caused the error
    /// </summary>
    public string Aktion { get; set; }

    /// <summary>
    /// Error message or details
    /// </summary>
    public string Error { get; set; }
} 