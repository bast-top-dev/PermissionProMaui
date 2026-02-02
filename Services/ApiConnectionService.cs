using Newtonsoft.Json;
using PermissionProMaui.Models.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Uri = System.Uri;

namespace PermissionProMaui.Services
{
    /// <summary>
    /// Service for handling API communication with the EBICS server.
    /// </summary>
    public class ApiConnectionService
    {
        private const string ServerUri = "https://easyebics.windata.de";

        /// <summary>
        /// Posts error protocols to the server.
        /// </summary>
        /// <param name="content">The error protocol content to send.</param>
        /// <returns>The server response.</returns>
        /// <exception cref="NullReferenceException">Thrown when content is null or empty.</exception>
        public async Task<string> PostErrorProtocols(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new NullReferenceException("ErrorProtocolContent cannot be null");

            HttpResponseMessage httpResponse = await new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = false
            }).PostAsync(ServerUri + "/api/SendProtocol", new StringContent(content));
            httpResponse.EnsureSuccessStatusCode();
            string response = await httpResponse.Content.ReadAsStringAsync();
            httpResponse.Dispose();

            return response;
        }

        /// <summary>
        /// Gets the Berlin timestamp from the server.
        /// </summary>
        /// <returns>The timestamp string, or null if the request fails.</returns>
        public async Task<string> GetBerlinTimeStamp()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "text/plain");
            var response = await client.GetAsync(new Uri(ServerUri + "/api/Timestamp"));

            var statusCode = response.StatusCode;

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;
                case HttpStatusCode.InternalServerError:
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    break;
                case HttpStatusCode.OK:
                    return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        /// <summary>
        /// Gets server information for a specific BLZ (Bankleitzahl).
        /// </summary>
        /// <param name="blz">The bank code.</param>
        /// <returns>The server information, or null if not found.</returns>
        public Server GetServer(string blz)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var myUri = new Uri(ServerUri + $"/api/Server/{blz}");

                var response = client.GetAsync(myUri.AbsoluteUri).Result;
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.InternalServerError:
                    case HttpStatusCode.ServiceUnavailable:
                        return null;
                    case HttpStatusCode.OK:
                        var result = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<Server>(result);
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }

        /// <summary>
        /// Sends initialization letters to the server.
        /// </summary>
        /// <param name="letters">The initialization letters to send.</param>
        /// <returns>The server response.</returns>
        public async Task<string> SendIniLetters(InitLetterJson letters)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ServerUri);

            string json = JsonConvert.SerializeObject(letters);
            var content = new StringContent(json, Encoding.UTF8, "Application/json");

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.PostAsync("/api/InitLetters", content);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;
                case HttpStatusCode.InternalServerError:
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    break;
                case HttpStatusCode.OK:
                    return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        /// <summary>
        /// Sends error protocols to the server.
        /// </summary>
        /// <param name="protocolSerialisation">The error protocol serialization.</param>
        /// <param name="mail">The email address.</param>
        /// <param name="additionalInfo">Additional information.</param>
        /// <returns>The server response.</returns>
        public async Task<string> SendErrorProtocols(object protocolSerialisation, string mail, string additionalInfo)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ServerUri);

            string json = JsonConvert.SerializeObject(protocolSerialisation);
            var content = new StringContent(json, Encoding.UTF8, "Application/json");

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.PostAsync($"/api/SendProtocol?mail={mail}&additionalInfo={additionalInfo}", content);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;
                case HttpStatusCode.InternalServerError:
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    break;
                case HttpStatusCode.OK:
                    return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        /// <summary>
        /// Sends debug database to the server.
        /// </summary>
        /// <param name="db">The database information to send.</param>
        /// <returns>The server response.</returns>
        public async Task<string> SendDebugDb(DbJson db)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ServerUri);

            string json = JsonConvert.SerializeObject(db);
            var content = new StringContent(json, Encoding.UTF8, "Application/json");

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.PostAsync("/api/SendDebugDb", content);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    break;
                case HttpStatusCode.InternalServerError:
                    break;
                case HttpStatusCode.ServiceUnavailable:
                    break;
                case HttpStatusCode.OK:
                    return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

        /// <summary>
        /// Gets the license status from the server.
        /// </summary>
        /// <param name="code">The license code to check.</param>
        /// <returns>True if the license is valid, false otherwise.</returns>
        public async Task<bool> GetLicenseStatus(string code)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ServerUri);

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.GetAsync($"/api/LicenseStatus/{code}");

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return false;
                case HttpStatusCode.InternalServerError:
                    return false;
                case HttpStatusCode.ServiceUnavailable:
                    return false;
                case HttpStatusCode.OK:
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<bool>(result);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets license information with a specific code.
        /// </summary>
        /// <param name="code">The license code.</param>
        /// <returns>The license information, or null if not found.</returns>
        public async Task<LicenceJson> GetLicenceJsonWithCode(string code)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ServerUri);

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.GetAsync($"/api/License/{code}");

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return null;
                case HttpStatusCode.InternalServerError:
                    return null;
                case HttpStatusCode.ServiceUnavailable:
                    return null;
                case HttpStatusCode.OK:
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LicenceJson>(result);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Sets a license code as used.
        /// </summary>
        /// <param name="code">The license code to mark as used.</param>
        /// <returns>The updated license information.</returns>
        public async Task<LicenceJson> SetCodeAsUsed(string code)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ServerUri);

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.PostAsync($"/api/License/{code}/SetAsUsed", null);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return null;
                case HttpStatusCode.InternalServerError:
                    return null;
                case HttpStatusCode.ServiceUnavailable:
                    return null;
                case HttpStatusCode.OK:
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LicenceJson>(result);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Creates an export for a contact.
        /// </summary>
        /// <param name="contact">The contact information to export.</param>
        /// <returns>The export result.</returns>
        public async Task<string> CreateExport(ExportContactJson contact)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ServerUri);

            string json = JsonConvert.SerializeObject(contact);
            var content = new StringContent(json, Encoding.UTF8, "Application/json");

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.PostAsync("/api/Export", content);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return null;
                case HttpStatusCode.InternalServerError:
                    return null;
                case HttpStatusCode.ServiceUnavailable:
                    return null;
                case HttpStatusCode.OK:
                    return await response.Content.ReadAsStringAsync();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Consumes an export for a contact.
        /// </summary>
        /// <param name="importContact">The contact information to import.</param>
        /// <returns>The imported contact information.</returns>
        public async Task<ExportContactJson> ConsumeExport(ExportContactJson importContact)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ServerUri);

            string json = JsonConvert.SerializeObject(importContact);
            var content = new StringContent(json, Encoding.UTF8, "Application/json");

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.PostAsync("/api/Import", content);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return null;
                case HttpStatusCode.InternalServerError:
                    return null;
                case HttpStatusCode.ServiceUnavailable:
                    return null;
                case HttpStatusCode.OK:
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ExportContactJson>(result);
                default:
                    return null;
            }
        }
    }
} 