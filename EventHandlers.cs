using System;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Permissions.Extensions;
using KillLogs.Enums;
using Respawning;

namespace KillLogs
{
    public class EventHandlers
    {
        private readonly Plugin plugin;

        public EventHandlers(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnDying(DyingEventArgs ev)
        {
            if (!Round.IsStarted) return;
            Log.Debug("Player died");
            if (ev.Attacker == null) return;
            if (ev.Player == null) return;
            Log.Debug("Not null target/victim");

            if (ev.Attacker.IsScp && !plugin.Config.LogScpKills) return;
            Log.Debug("Not SCP");

            if (ev.Attacker == ev.Player)
            {
                if (!plugin.Config.LogSuicides) return;
                Log.Debug("Suicide");
                plugin.LogManager.ReportKill(ev, LogReason.Regular);
                return;
            }

            if (ev.Attacker.Role.Team == ev.Player.Role.Team)
            {
                Log.Debug("**TEAMKILL**");
                plugin.LogManager.ReportKill(ev, LogReason.TeamKill, plugin.Config.PingTeamkills);
                return;
            }

            if (!ev.Attacker.IsScp && ev.Player.IsCuffed)
            {
                Log.Debug("**CUFFED**");
                plugin.LogManager.ReportKill(ev, LogReason.CuffedKill, plugin.Config.PingCuffedHumanKills);
                return;
            }

            Log.Debug("Regular kill");
            plugin.LogManager.ReportKill(ev, LogReason.Regular);
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            plugin.LogManager.EnqueueText("**=== ROUND ENDED ===**", true);
            Log.Debug("Sent round ended message");
        }

        public void OnRoundStarted()
        {
            plugin.LogManager.EnqueueText("**=== ROUND STARTED ===**", true);
            Log.Debug("Sent round started message");
            
            plugin.PlayersToNotify.Clear();
            plugin.PlayersToNotify = Player.List.Where(player => player.CheckPermission("kill_logs.notify")).ToList();
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            if (ev.Player.CheckPermission("kill_logs.notify") && !plugin.PlayersToNotify.Contains(ev.Player))
                plugin.PlayersToNotify.Add(ev.Player);
        }
        
        public void OnLeft(LeftEventArgs ev)
        {
            plugin.PlayersToNotify.Remove(ev.Player);
        }
        
        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            switch (ev.NextKnownTeam)
            {
                case SpawnableTeamType.ChaosInsurgency:
                    plugin.LogManager.EnqueueText("**=== CI SPAWNED ===**", true);
                    break;
                case SpawnableTeamType.NineTailedFox:
                    plugin.LogManager.EnqueueText("**=== NTF SPAWNED ===**",true);
                    break;
                case SpawnableTeamType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}