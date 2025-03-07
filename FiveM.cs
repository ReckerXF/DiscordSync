﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            // Variables
            string steamId = ply.Identifiers["steam"];
            string discordId = ply.Identifiers["discord"];

            // Handle Deferral.
            deferrals.defer();
            deferrals.update("Retrieving join data...");

            // Check if the player has discord connected to FiveM.
            if (steamId == null || discordId == null)
            {
                deferrals.done("You must have Discord and Steam connected to your FiveM to join the server!");
                return;
            }

            await GetInfo(ply);

            // Handle Whitelisting.
            if (_whitelisted == false)
            {
                deferrals.presentCard(deferralCardJson);
                return;
            }

            // Handle Rank Assignment.
            API.ExecuteCommand($"add_principal identifier.steam:{steamId} {_group}");
            deferrals.done();
            
        }

        [EventHandler("playerDropped")]
        private static void OnPlayerDropped([FromSource] Player ply, string reason)
        {
            string steamId = ply.Identifiers["steam"];

            API.ExecuteCommand($"add_principal identifier.steam:{steamId} {Config.defaultACE}");
        }
        #endregion

        #region Core Logic
        /// <summary>
        /// Obtains information through Discord and assigns obtained values in place of default variables (i.e. _group and _whitelisted).
        /// </summary>
        /// <param name="player"></param>
        /// <returns>Task</returns>
        private static async Task GetInfo(Player player)
        {
            string plyDiscordId = player.Identifiers["discord"];

            List<string> discordRoles = await Discord.GetDiscordRoles(plyDiscordId);

            foreach (string roleId in discordRoles)
            {
                if (roleId == Config.whitelistedRoleId)
                {
                    _whitelisted = true;

                    if (Config.debugMode)
                        Debug.WriteLine($"{player.Name} has been marked as whitelisted!");
                }
            }

            foreach (string roleId in discordRoles)
            {
                Config.rolesToSync.TryGetValue(roleId, out _group);
            }

            await Delay(300);
        }
        #endregion

    }
}