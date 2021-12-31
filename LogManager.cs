using System;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace KillLogs
{
    public class LogManager
    {
        private readonly Plugin plugin;
        private readonly StringBuilder _killString = new();

        private readonly StringBuilder _queue = new(2000);

        public LogManager(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void ReportKill(DyingEventArgs ev, LogReason reason, bool sendImmediately = false)
        {
            Log.Debug("ReportKill", plugin.Config.Debug);

            _killString.Clear();

            _killString.Append($"`{DateTime.Now}` ");
            _killString.Append($"**[{ev.Killer.Role}] {ev.Killer.Nickname} (`{ev.Killer.UserId}`)** killed ");
            _killString.Append($"**[{ev.Target.Role}] {ev.Target.Nickname} (`{ev.Target.UserId}`)** ");
            _killString.Append(GetSpecialDecoration(reason));
            _killString.Append($" [ZONE: {ev.Target.Zone}] ");
            _killString.Append(GetMention(reason));

            Log.Debug(_killString, plugin.Config.Debug);

            EnqueueText(_killString.ToString(), sendImmediately);
        }

        private void SendQueue()
        {
            plugin.KillWebhook.SendMessage(_queue.ToString())
                .Queue(() => Log.Debug($"Sent queue of length {_queue.Length}", plugin.Config.Debug));

            _queue.Clear();
        }

        internal void EnqueueText(string killString, bool sendImmediately = false)
        {
            if (killString.Length + _queue.Length >= plugin.Config.QueueLength) SendQueue();
            _queue.AppendLine(killString);
            if (sendImmediately) SendQueue();
            Log.Debug("Enqueued kill", plugin.Config.Debug);
        }

        private string GetMention(LogReason reason)
        {
            switch (reason)
            {
                case LogReason.CuffedKill when plugin.Config.PingCuffedHumanKills:
                    return $"<@&{plugin.Config.RoleIdToPing}>";
                case LogReason.TeamKill when plugin.Config.PingTeamkills:
                    return $"<@&{plugin.Config.RoleIdToPing}>";
                default:
                    return null;
            }
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