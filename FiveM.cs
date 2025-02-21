using CitizenFX.Core;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSync.Server
{
    public class FiveM : BaseScript
    {
        #region Events
        [EventHandler("playerConnecting")]
        private static void OnPlayerConnecting([FromSource] Player ply, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            
        }

        [EventHandler("playerDropped")]
        private static void OnPlayerDropped([FromSource] Player ply, string reason)
        {

        }
        #endregion

        #region Core Logic
        private bool IsWhitelisted(Player player)
        {
            if (player.Identifiers["discord"] == null) return false;

            IReadOnlyCollection<SocketRole> discordRoles = DiscordSync.Server.Discord.GetDiscordRoles(player.Identifiers["discord"]);

            foreach (SocketRole role in discordRoles)
            {
                if (role.Id == Config.whitelistedRoleId) return true;
            }

            return false;
        }

        private string Rank(Player player)
        {
            if (player.Identifiers["discord"] == null) return Config.defaultACE;

            IReadOnlyCollection<SocketRole> discordRoles = DiscordSync.Server.Discord.GetDiscordRoles(player.Identifiers["discord"]);

            foreach (SocketRole role in discordRoles)
            {
                if (Config.RolesToSync.TryGetValue(role.Id, out string rank)) return rank;
            }

            return Config.defaultACE;
        }
        #endregion

    }
}