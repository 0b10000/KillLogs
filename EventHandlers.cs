using System;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
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
            Log.Debug("Player died", plugin.Config.Debug);
            if (ev.Killer == null) return;
            if (ev.Target == null) return;
            Log.Debug("Not null target/victim", plugin.Config.Debug);

            if (ev.Killer.IsScp && !plugin.Config.LogScpKills) return;
            Log.Debug("Not SCP", plugin.Config.Debug);

            if (ev.Killer == ev.Target)
            {
                if (!plugin.Config.LogSuicides) return;
                Log.Debug("Suicide", plugin.Config.Debug);
                plugin.LogManager.ReportKill(ev, LogReason.Regular);
                return;
            }

            if (ev.Killer.Role.Team == ev.Target.Role.Team)
            {
                Log.Debug("**TEAMKILL**", plugin.Config.Debug);
                plugin.LogManager.ReportKill(ev, LogReason.TeamKill, plugin.Config.PingTeamkills);
                return;
            }

            if (!ev.Killer.IsScp && ev.Target.IsCuffed)
            {
                Log.Debug("**CUFFED**", plugin.Config.Debug);
                plugin.LogManager.ReportKill(ev, LogReason.CuffedKill, plugin.Config.PingCuffedHumanKills);
                return;
            }

            Log.Debug("Regular kill", plugin.Config.Debug);
            plugin.LogManager.ReportKill(ev, LogReason.Regular);
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            plugin.LogManager.EnqueueText("**=== ROUND ENDED ===**", true);
            Log.Debug("Sent round ended message", plugin.Config.Debug);
        }

        public void OnRoundStarted()
        {
            plugin.LogManager.EnqueueText("**=== ROUND STARTED ===**", true);
            Log.Debug("Sent round started message", plugin.Config.Debug);
            
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