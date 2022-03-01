# KillLogs
![GitHub release (latest by SemVer)](https://img.shields.io/github/downloads/0b10000/KillLogs/latest/total?sort=semver)
![GitHub all releases](https://img.shields.io/github/downloads/0b10000/KillLogs/total)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2F0b10000%2FKillLogs.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2F0b10000%2FKillLogs?ref=badge_shield)


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
  # Whether or not to ping when a teamkill happens.
  ping_teamkills: false
  # Whether or not to notify online players with the kill_logs.notify permission when a human kills another cuffed human.
  notify_cuffed_human_kills: true
  # Whether or not to notify online players with the kill_logs.notify permission when a teamkill happens.
  notify_team_kills: true
  # Duration (in seconds) of how long the kill notification should last.
  notify_hint_duration: 10
  debug: false
 ```
 
 # Installation
 * Upload the KillLogs.dll file from the [Releases](https://github.com/0b10000/KillLogs/releases) page into your EXILED plugins directory.
 * Upload the dependencies.zip file to the plugins/dependencies folder and unzip the contents.
 * Restart your server or reload plugins and configure the plugin. 


## License
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2F0b10000%2FKillLogs.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2F0b10000%2FKillLogs?ref=badge_large)