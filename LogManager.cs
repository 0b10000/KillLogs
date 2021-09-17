using System;
using System.Collections.Generic;
using System.Text;
using Exiled.Events.EventArgs;

namespace KillLogs
{
    public class LogManager
    {
        private readonly Plugin plugin;
        public LogManager(Plugin plugin) => this.plugin = plugin;

        private StringBuilder _queue = new(2000);
        private StringBuilder _killString = new();

        public void ReportKill(DyingEventArgs ev, LogReason reason)
        {
            _killString.Append($"{DateTime.Now} ");
            _killString.Append($"{ev.Killer.Nickname} (`{ev.Killer.UserId}`) killed ");
            _killString.Append($"{ev.Target.Nickname} (`{ev.Target.UserId}`) ");
            _killString.Append(GetSpecialDecoration(reason));
            _killString.Append(GetMention(reason));

        }
        
        public void CleanupQueue()
        {
            
        }

        private void EnqueueKill(string killString)
        {
            if (killString.Length + _queue.Length >= 2000) CleanupQueue();
            _queue.AppendLine(killString);
        }

        private string GetMention(LogReason reason)
        {
            if (reason == LogReason.CuffedKill && plugin.Config.PingCuffedHumanKills)
                return $"<@{plugin.Config.RoleIdToPing}>";
            if (reason == LogReason.TeamKill && plugin.Config.PingTeamkills)
                return $"<@{plugin.Config.RoleIdToPing}>";
            return null;
        }

        private string GetSpecialDecoration(LogReason reason)
        {
            switch (reason)
            {
                case LogReason.CuffedKill:
                    return "**(CUFFED)**";
                case LogReason.TeamKill:
                    return "**(TEAMKILL)**";
                default:
                    return null;
            }
        }
        
    }
}