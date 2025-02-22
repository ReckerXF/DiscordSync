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
            public string guildId { get; set; }
            public Dictionary<string, string> rolesToSync { get; set; }
            public string defaultACE { get; set; }
            public string whitelistedRoleId { get; set; }
        }
    }
}
