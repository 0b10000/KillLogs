using System;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
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
            Log.Debug("ReportKill");

            _killString.Clear();

            _killString.Append(
                $"<t:{DateTimeOffset.Now.ToUnixTimeSeconds()}> **[{ev.Attacker.Role.Type}] {ev.Attacker.Nickname} (`{ev.Attacker.UserId}`)** killed **[{ev.Player.Role.Type}] {ev.Player.Nickname} (`{ev.Player.UserId}`)** {GetSpecialDecoration(reason)} [ZONE: {ev.Player.Zone}] {GetMention(reason)}");

            Log.Debug(_killString);

            EnqueueText(_killString.ToString(), sendImmediately);

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (reason)
            {
                case LogReason.CuffedKill when plugin.Config.NotifyCuffedHumanKills:
                    plugin.Methods.SendHintToNotifiablePlayers(
                        $"<color=red>{ev.Attacker.Nickname} has killed {ev.Player.Nickname} while cuffed!</color>");
                    break;
                case LogReason.TeamKill when plugin.Config.NotifyTeamKills:
                    plugin.Methods.SendHintToNotifiablePlayers(
                        $"<color=red>{ev.Attacker.Nickname} has teamkilled {ev.Player.Nickname}!</color>");
                    break;
            }
        }

        private void SendQueue()
        {
            plugin.MessageBuilder.Reset();
            plugin.MessageBuilder.AvatarUrl = plugin.Config.WebhookAvatarUrl;
            plugin.MessageBuilder.Username = plugin.Config.WebhookName;
            plugin.MessageBuilder.Append(_queue.ToString());
            var message = plugin.MessageBuilder.Build();
            
            plugin.KillWebhook.SendMessage(message)
                .Queue(() => Log.Debug($"Sent queue of length {_queue.Length}"));

            _queue.Clear();
        }

        internal void EnqueueText(string killString, bool sendImmediately = false)
        {
            if (sendImmediately) SendQueue();
            if (killString.Length + _queue.Length >= plugin.Config.QueueLength || _queue.Length >= 1900) SendQueue();
            _queue.AppendLine(killString);
            if (sendImmediately) SendQueue(); // second time so the message is highlighted only
            Log.Debug("Enqueued kill");
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