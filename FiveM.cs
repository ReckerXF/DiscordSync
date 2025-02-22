using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace DiscordSync.Server
{
    public class FiveM : BaseScript
    {
        #region Variables
        private static string deferralCardJson = "";
        private static bool _whitelisted = false;
        private static string _group = Config.defaultACE;

        #endregion

        #region Constructor
        public FiveM()
        {
            Debug.WriteLine("DiscordSync by github.com/ReckerXF loaded!");

            try
            {
                deferralCardJson = API.LoadResourceFile(API.GetCurrentResourceName(), "deferralCard.json");

                if (string.IsNullOrEmpty(deferralCardJson))
                {
                    Debug.WriteLine("The deferralCard.json file is empty!");
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine($"Json Error Reported: {e.Message}\nStackTrace:\n{e.StackTrace}");
            }
        }
        #endregion

        #region Events
        [EventHandler("playerConnecting")]
        private static async void OnPlayerConnecting([FromSource] Player ply, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            // Handle Deferral
            deferrals.defer();
            deferrals.update("Retrieving join data...");

            // Check if the player has discord connected to FiveM
            if (ply.Identifiers["discord"] == null || ply.Identifiers["steam"] == null)
            {
                deferrals.done("You must have Discord and Steam connected to your FiveM to join the server!");
                return;
            }

            await GetInfo(ply);

            // Handle Whitelisting
            if (!_whitelisted)
            {
                deferrals.presentCard(deferralCardJson);
                return;
            }

            // Handle Rank Assignment
            API.ExecuteCommand($"add_principal {_group}");
            deferrals.done();
            
        }

        [EventHandler("playerDropped")]
        private static void OnPlayerDropped([FromSource] Player ply, string reason)
        {
            API.ExecuteCommand($"add_principal {Config.defaultACE}");
        }
        #endregion

        #region Core Logic
        private static async Task GetInfo(Player player)
        {
            string plyDiscordId = player.Identifiers["discord"];

            List<string> discordRoles = await Discord.GetDiscordRoles(plyDiscordId);

            foreach (string roleId in discordRoles)
            {
                if (roleId == Config.whitelistedRoleId)
                    _whitelisted = true;
            }

            foreach (string roleId in discordRoles)
            {
                Config.rolesToSync.TryGetValue(roleId, out _group);
            }

            await Delay(200);
        }
        #endregion

    }
}