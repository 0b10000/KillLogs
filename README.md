# KillLogs
![GitHub release (latest by SemVer)](https://img.shields.io/github/downloads/0b10000/KillLogs/latest/total?sort=semver)


A plugin for EXILED that logs kills into a Discord channel using webhooks.

Default configuration: 
```yml
KillLogs:
# Whether or not this plugin is enabled.
  is_enabled: true
  # The Discord webhook URL.
  discord_webhook_url: https://canary.discord.com/api/webhooks/id/secret
  # The Discord role ID to ping when a cuffed kill/teamkill happens.
  role_id_to_ping: 012345678910
  # Length of the queue before it should be sent. Lower numbers result in faster sends to Discord but can lead to ratelimiting by Discord.
  queue_length: 1000
  # Whether to log SCP kills or not.
  log_scp_kills: false
  # Whether to log suicides or not.
  log_suicides: true
  # Whether or not to ping when a human kills another cuffed human.
  ping_cuffed_human_kills: true
  # Whether or not to ping when a teamkill happens.
  ping_teamkills: true
  debug: false
 ```
 
 # Installation
 * Upload the KillLogs.dll file from the [Releases](https://github.com/0b10000/KillLogs/releases) page into your EXILED plugins directory.
 * Upload the dependencies.zip file to the plugins/dependencies folder and unzip the contents.
 * Restart your server or reload plugins and configure the plugin. 
