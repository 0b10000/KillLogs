using System.ComponentModel;
using Exiled.API.Interfaces;

namespace KillLogs
{
    public class Config : IConfig
    {
        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("The Discord webhook URL.")]
        public string DiscordWebhookUrl { get; set; } = "";

        [Description("Whether to log SCP kills or not.")]
        public bool LogScpKills { get; set; } = false;
        
        [Description("The Discord role ID to ping when a cuffed kill/teamkill happens.")]
        public string RoleIdToPing { get; set; } = "01234567890";
        
        [Description("Whether or not to ping when a human kills another cuffed human.")]
        public bool PingCuffedHumanKills { get; set; } = true;

        [Description("Whether or not to ping when a teamkill happens.")]
        public bool PingTeamkills { get; set; } = true;
        
    }
}