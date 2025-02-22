using System.Collections.Generic;

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
            public bool debugMode { get; set; }
        }
    }
}
