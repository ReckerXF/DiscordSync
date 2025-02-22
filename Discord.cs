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
        private static HttpClient _httpClient = new HttpClient();
        private static string _botToken = Config.botToken;
        private static string _guildId = Config.guildId;
        private static List<string> userRoles = new List<string>();

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

            EventHandlers["onResourceStart"] += new Action<string>(OnResourceStart);
        }

        private async void OnResourceStart(string resourceName)
        {
            try
            {
                Debug.WriteLine("Initializing DiscordSync...");
                await Task.CompletedTask;
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        public static async Task<List<string>> GetDiscordRoles(string discordId)
        {
            if (string.IsNullOrEmpty(_botToken) || string.IsNullOrEmpty(_guildId))
            {
                Debug.WriteLine("Discord Bot token or Guild ID is not set.");
                return null;
            }

            // URL to fetch the guild member's roles
            string url = $"https://discord.com/api/v10/guilds/{_guildId}/members/{discordId}";
            
            // Set the authorization headers
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {_botToken}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            // Make the request
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // Log the status code and response content for debugging
                string responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"[DiscordHttpClient] Failed to fetch roles for user {discordId}. HTTP Status: {response.StatusCode}, Response: {responseContent}");
                return null;
            }

            // Log raw JSON content for debugging
            string content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"[DiscordHttpClient] Raw JSON response: {content}");

            JObject json = JObject.Parse(content);
            var roles = json["roles"];

            if (roles == null)
                return null;

            foreach (var role in roles)
            {
                userRoles.Add((string)role);
            }
            
            // Return the list of roles.
            Debug.WriteLine($"[DiscordHttpClient] Retrieved {userRoles.Count} roles for user {discordId}.");
            return userRoles;
        }
    }
}
