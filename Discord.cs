using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Utils;
using System.Diagnostics;
using System.Threading;

namespace DiscordSync.Server
{
    internal class Discord
    {
        private static DiscordSocketClient _client;

        private static async Task Main(string[] args)
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent | GatewayIntents.GuildMembers | GatewayIntents.Guilds
            };

            _client = new DiscordSocketClient(config);

            _client.Ready += ReadAsync;

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));

            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private static Task ReadAsync()
        {
            Debug.WriteLine("DiscordSync has connected successfully to the Discord API!");

            return Task.CompletedTask;
        }

        public static IReadOnlyCollection<SocketRole> GetDiscordRoles(string discordId)
        {
            SocketGuild guild = _client.Guilds.Where(x => x.Id == Config.guildId) as SocketGuild;
            SocketGuildUser guildUser = guild.Users.Where(user => user.Id.ToString() == discordId) as SocketGuildUser;

            if (guild == null || guildUser == null) return null;

            return guildUser.Roles;
        }
    }
}
