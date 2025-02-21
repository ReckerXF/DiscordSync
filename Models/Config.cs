using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSync.Server.Models
{
    internal class ConfigModel
    {
        public struct ConfigStruct
        {
            public string botToken { get; set; }
            public ulong guildId { get; set; }
            public Dictionary<ulong, string> rolesToSync { get; set; }
            public string defaultACE { get; set; }
            public ulong whitelistedRoleId { get; set; }
        }
    }
}
