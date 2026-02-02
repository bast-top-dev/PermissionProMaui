using Newtonsoft.Json;
using PermissionProMaui.Models.Json;
using PermissionProMaui.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Base class for error protocol services
    /// </summary>
    public abstract class ErrorProtocolServiceBase
    {
        private readonly DatabaseService _databaseService;
        private readonly ApiConnectionService _apiConnectionService;

        protected ErrorProtocolServiceBase(DatabaseService databaseService = null, ApiConnectionService apiConnectionService = null)
        {
            _databaseService = databaseService ?? new DatabaseService();
            _apiConnectionService = apiConnectionService ?? new ApiConnectionService();
        }

        /// <summary>
        /// Writes an error protocol entry
        /// </summary>
        /// <param name="action">The action that caused the error</param>
        /// <param name="e">The exception that occurred</param>
        public void WriteErrorProtocol(string action, Exception e)
        {
            var error = new ErrorProtocolEntry
            {
                Aktion = action,
                Error = e.ToString() ?? string.Empty
            };

            var dbEntry = new SettingsTable
            {
                ProtocolDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                ProtocolName = action,
                ErrorProtocol = JsonConvert.SerializeObject(error)
            };

            _databaseService.ErrorProtocolRepo.CreateErrorProtocol(dbEntry);
        }

        /// <summary>
        /// Gets all error protocols
        /// </summary>
        /// <returns>List of error protocols</returns>
        public List<SettingsTable> GetErrorProtocols()
        {
            return _databaseService.ErrorProtocolRepo.ReadAllErrorProtocols();
        }

        /// <summary>
        /// Wipes all error protocols
        /// </summary>
        public void WhipeAllProtocols()
        {
            _databaseService.ErrorProtocolRepo.DeleteErrorProtocol(GetErrorProtocols());
        }

        /// <summary>
        /// Wipes old error protocols (older than today)
        /// </summary>
        public void WhipeOldProtocols()
        {
            var allProtocols = GetErrorProtocols();

            var protocolsToRemove = allProtocols.Where(p =>
            {
                var datestring = p.ProtocolDate;

                if (string.IsNullOrWhiteSpace(datestring)) return true;

                if (DateTime.TryParseExact(p.ProtocolDate, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    var today = DateTime.Now.Date;

                    if (date.Date < today)
                    {
                        return true;
                    }
                    return false;
                }

                return true;

            }).ToList();

            _databaseService.ErrorProtocolRepo.DeleteErrorProtocol(protocolsToRemove);
        }

        /// <summary>
        /// Sends debug database to development team
        /// </summary>
        public async Task SendDebugDb()
        {
            var dbJson = new DbJson
            {
                DbBase64 = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync(_databaseService.DatabasePath)),
                MailAdress = "entwicklung@windata.de"
            };

            await _apiConnectionService.SendDebugDb(dbJson);
        }

        /// <summary>
        /// Creates a protocol head for error reporting
        /// </summary>
        /// <returns>The protocol head</returns>
        public abstract ErrorProtocolHead CreateProtocolHead();

        /// <summary>
        /// Sends error protocols to the specified email
        /// </summary>
        /// <param name="mail">The email address to send to</param>
        /// <param name="additionalInfo">Additional information to include</param>
        public async Task SendErrorProtocols(string mail, string additionalInfo)
        {
            var headEntry = CreateProtocolHead();

            var protocols = GetErrorProtocols();

            var protocolSerialisation = new ErrorProtocolSerialisation()
            {
                Head = headEntry,
                Protocols = protocols.Select(p => p.ErrorProtocol).ToList()
            };

            await _apiConnectionService.SendErrorProtocols(protocolSerialisation, mail, additionalInfo);
        }

        private class ErrorProtocolSerialisation
        {
            public ErrorProtocolHead Head { get; set; }
            public List<string> Protocols { get; set; }
        }
    }
} 