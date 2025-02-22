-- CFX Details
lua54 'on'
fx_version 'cerulean'
games {'gta5'}

-- Resource stuff
name 'DiscordSync'
description 'Synchronizes Discord permissions with FiveM ACE permissions and also adds whitelist checking.'
version 'v1'
author 'ReckerXF'

files {
    "Newtonsoft.Json.dll",
    "Discord.Net.Commands.dll",
    "Discord.Net.Core.dll",
    "Discord.Net.Interactions.dll",
    "Discord.Net.Rest.dll",
    "Discord.Net.WebSocket.dll",
}

server_script 'DiscordSync.Server.net.dll'