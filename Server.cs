using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSync.Server
{
    public class Server : BaseScript
    {
        #region Events
        [EventHandler("playerConnecting")]
        private async void OnPlayerConnecting([FromSource] Player ply, string playerName, dynamic setKickReason, dynamic deferrals)
        {

        }
        #endregion

    }
}
