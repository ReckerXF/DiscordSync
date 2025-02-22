# DiscordSync
A Discord-version of [InvisionSync](https://github.com/reckerxf/InvisionSync)! DiscordSync sets your in-game FiveM permissions based on what Discord roles are held, and also checks for whitelisting. Screw paying money for synchronizing your permissions and whitelist scripts!

**Features**
- 
* Synchronize Discord roles to in-game permissions.
* Use a "Whitelisted" role in Discord to whitelist your players!
* Debug mode ensures you're getting all of the appropriate data!
* Does not require a server restart for permissions to be applied/revoked.

**Setup**
-
Setup:

1. Create a Bot Token at https://discord.com/developers.

2. Put your bot token in the ``config.json``.

3. Enable Developer Mode in Discord and right click on the roles you want to sync and then Copy ID.

4. Format the ``rolesToSync`` list as ``"DiscordID": "Ace.Perm"``. An example of this is ``"1342797989147840584": "group.admin"``. Don't forget a comma after each line (except the last)!

5. In your server.cfg or other .cfg file, add the following.
``add_ace resource.discordsync command.add_principal allow``

You're ready to start having permissions and whitelisting be sync'd!

**Support**
-
Support will be provided as requested.
