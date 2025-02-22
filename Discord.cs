using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CitizenFX.Core;
using Newtonsoft.Json.Linq;

namespace DiscordSync.Server
{
    internal class Discord : BaseScript
    {
        #region Variables
        private static HttpClient _httpClient = new HttpClient();
        private static string _botToken = Config.botToken;
        private static string _guildId = Config.guildId;
        #endregion

        #region Constructor
        public Discord()
        {
            // Setup HttpClientHandler to disable SSL/TLS verification.
            var handler = new HttpClientHandler()
            {
                // Bypass SSL certificate validation.
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            // Create the HttpClient using the handler.
            _httpClient = new HttpClient(handler);
        }
        #endregion

        #region Discord Bot Handling
        /// <summary>
        /// Returns a list of Discord Roles that belongs to the player.
        /// </summary>
        /// <param name="discordId"></param>
        /// <returns>List (String)</returns>
        public static async Task<List<string>> GetDiscordRoles(string discordId)
        {
            // Declare new list for the player's Discord roles.
            List<string> userRoles = new List<string>();

            // URL to fetch the guild member's roles.
            string url = $"https://discord.com/api/v10/guilds/{_guildId}/members/{discordId}";
            
            // Set the authorization headers.
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {_botToken}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            // Make the request.
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // Log the status code and response content for debugging.
                string responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[DiscordHttpClient] Failed to fetch roles for user {discordId}. HTTP Status: {response.StatusCode}, Response: {responseContent}");
                return null;
            }

            // Log raw JSON content for debugging.
            string content = await response.Content.ReadAsStringAsync();
            
            if (Config.debugMode)
                Debug.WriteLine($"[DiscordHttpClient] Raw JSON response: {content}");

            JObject json = JObject.Parse(content);
            var roles = json["roles"];

            if (roles == null)
                return null;

            foreach (var role in roles)
            {
                userRoles.Add((string)role);
            }
            
            if (Config.debugMode)
                Debug.WriteLine($"[DiscordHttpClient] Retrieved {userRoles.Count} roles for user {discordId}.");

            // Return the list of roles.
            return userRoles;
        }
        #endregion
    }
}
