using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using PermissionProMaui.Models;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for handling EBICS protocol operations and version detection.
    /// </summary>
    public class EbicsService
    {
        private readonly ApiConnectionService _apiConnectionService = new ApiConnectionService();

        // EBICS Version constants
        public static class EbicsVersion
        {
            public const string EbicsV25 = "H004";
            public const string EbicsV30 = "H005";
        }

        /// <summary>
        /// Gets the supported EBICS version for a specific contact.
        /// </summary>
        /// <param name="ebicsContactModel">The EBICS contact model.</param>
        /// <returns>The supported EBICS version.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no supported version is found.</exception>
        public async Task<string> GetSupportedEbicsVersion(EbicsContactModel ebicsContactModel)
        {
            try
            {
                // Simplified version detection - try H005 first, then fallback to H004
                var supportedVersions = await GetAllSupportedEbicsVersion(ebicsContactModel.Bank.HostId, ebicsContactModel.Bank.Uri);
                
                if (supportedVersions.Contains(EbicsVersion.EbicsV30))
                    return EbicsVersion.EbicsV30;
                
                if (supportedVersions.Contains(EbicsVersion.EbicsV25))
                    return EbicsVersion.EbicsV25;

                throw new InvalidOperationException("This server does not support a matching EBICS-Version.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to determine EBICS version: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all supported EBICS versions for a specific host and server.
        /// </summary>
        /// <param name="hostId">The host identifier.</param>
        /// <param name="server">The server URL.</param>
        /// <returns>List of supported EBICS versions.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no supported versions are found.</exception>
        public async Task<List<string>> GetAllSupportedEbicsVersion(string hostId, string server)
        {
            try
            {
                var supportedVersions = new List<string>();

                // Simplified version detection - assume both versions are supported
                // In a real implementation, this would make actual HEV requests to the server
                supportedVersions.Add(EbicsVersion.EbicsV25);
                supportedVersions.Add(EbicsVersion.EbicsV30);

                if (supportedVersions.Count == 0)
                    throw new InvalidOperationException("This server does not support a matching EBICS-Version.");

                return supportedVersions;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get supported EBICS versions: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Simplified HEV document for version detection.
        /// </summary>
        private class HevDocument
        {
            public HevDocument(string content)
            {
                Content = content;
            }

            public string Content { get; }

            public bool VersionIsSupported(string version)
            {
                // Simplified version check
                return Content.Contains(version);
            }

            public bool VersionIsSupported(float version)
            {
                // Simplified version check for float values
                string versionStr = version.ToString("F1", CultureInfo.InvariantCulture);
                return Content.Contains(versionStr);
            }
        }

        /// <summary>
        /// Gets the parsed HEV (Host Electronic Signature) result for version detection.
        /// </summary>
        /// <param name="hostId">The host identifier.</param>
        /// <param name="server">The server URL.</param>
        /// <returns>The parsed HEV document.</returns>
        private async Task<HevDocument> GetParsedHevResult(string hostId, string server)
        {
            try
            {
                string timeStamp = await _apiConnectionService.GetBerlinTimeStamp();

                // Simplified HEV request simulation
                // In a real implementation, this would make an actual HEV request to the server
                var hevResponse = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><hev xmlns=\"urn:org:ebics:H004\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:org:ebics:H004 hev.xsd\"><version>H004</version><version>H005</version></hev>";

                return new HevDocument(hevResponse);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get HEV result: {ex.Message}", ex);
            }
        }
    }
} 