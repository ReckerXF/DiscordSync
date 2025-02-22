using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static DiscordSync.Server.Models.ConfigModel;

namespace DiscordSync.Server
{
    internal class Config : BaseScript
    {
        public static string botToken = GetConfig().botToken;

        public static string guildId = GetConfig().guildId;

        public static Dictionary<string, string> rolesToSync = GetConfig().rolesToSync;

        public static string defaultACE = GetConfig().defaultACE;

        public static string whitelistedRoleId = GetConfig().whitelistedRoleId;


        public static ConfigStruct GetConfig()
        {
            ConfigStruct data = new ConfigStruct();
            string jsonFile = API.LoadResourceFile(API.GetCurrentResourceName(), "config.json");

            try
            {
                if (string.IsNullOrEmpty(jsonFile))
                {
                    Debug.WriteLine("The config.json file is empty!");
                }
                else
                {
                    data = JsonConvert.DeserializeObject<ConfigStruct>(jsonFile);
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine($"Json Error Reported: {e.Message}\nStackTrace:\n{e.StackTrace}");
            }

            return data;
        }
    }
}
