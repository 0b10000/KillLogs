using System;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using KillLogs.Enums;

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

            _killString.Append(
                $"<t:{DateTime.Now.Second}:F> **[{ev.Killer.Role}] {ev.Killer.Nickname} (`{ev.Killer.UserId}`)** killed **[{ev.Target.Role}] {ev.Target.Nickname} (`{ev.Target.UserId}`)** {GetSpecialDecoration(reason)} [ZONE: {ev.Target.Zone}] {GetMention(reason)}");

            Log.Debug(_killString, plugin.Config.Debug);

            EnqueueText(_killString.ToString(), sendImmediately);

            switch (reason)
            {
                case LogReason.CuffedKill when plugin.Config.NotifyCuffedHumanKills:
                    plugin.Methods.SendHintToNotifiablePlayers(
                        $"<color=red>{ev.Killer.Nickname} has killed {ev.Target.Nickname} while cuffed!</color>");
                    break;
                case LogReason.TeamKill when plugin.Config.NotifyTeamKills:
                    plugin.Methods.SendHintToNotifiablePlayers(
                        $"<color=red>{ev.Killer.Nickname} has teamkilled {ev.Target.Nickname}!</color>");
                    break;
                case LogReason.Regular:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reason), reason, null);
            }
        }

        private void SendQueue()
        {
            plugin.KillWebhook.SendMessage(_queue.ToString())
                .Queue(() => Log.Debug($"Sent queue of length {_queue.Length}", plugin.Config.Debug));

            _queue.Clear();
        }

        internal void EnqueueText(string killString, bool sendImmediately = false)
        {
            if (killString.Length + _queue.Length >= plugin.Config.QueueLength || _queue.Length >= 1900) SendQueue();
            _queue.AppendLine(killString);
            if (sendImmediately) SendQueue();
            Log.Debug("Enqueued kill", plugin.Config.Debug);
        }

        private string GetMention(LogReason reason)
        {
            return reason switch
            {
                LogReason.CuffedKill when plugin.Config.PingCuffedHumanKills => $"<@&{plugin.Config.RoleIdToPing}>",
                LogReason.TeamKill when plugin.Config.PingTeamkills => $"<@&{plugin.Config.RoleIdToPing}>",
                _ => null
            };
        }

        private string GetSpecialDecoration(LogReason reason)
        {
            return reason switch
            {
                LogReason.CuffedKill => "**(CUFFED)**",
                LogReason.TeamKill => "**(TEAMKILL)**",
                _ => null
            };
        }
    }
}