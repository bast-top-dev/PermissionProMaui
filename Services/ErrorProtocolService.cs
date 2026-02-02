using PermissionProMaui.Models;
using PermissionProMaui.Models.Json;
using System;
using System.Globalization;
using Microsoft.Maui.Devices;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for handling error protocols and logging
    /// </summary>
    public class ErrorProtocolService : ErrorProtocolServiceBase
    {
        /// <summary>
        /// Creates a protocol head for error reporting
        /// </summary>
        /// <returns>The protocol head</returns>
        public override ErrorProtocolHead CreateProtocolHead()
        {
            return new ErrorProtocolHead
            {
                PhoneDateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                PhoneName = $"{DeviceInfo.Manufacturer} {DeviceInfo.Model}",
                PhoneOS = $"{DeviceInfo.Platform}",
                PhoneOSVersion = $"{DeviceInfo.VersionString}"
            };
        }

        /// <summary>
        /// Writes an error protocol entry
        /// </summary>
        /// <param name="action">The action that caused the error</param>
        /// <param name="e">The exception that occurred</param>
        public new void WriteErrorProtocol(string action, Exception e)
        {
            base.WriteErrorProtocol(action, e);
        }
    }
} 