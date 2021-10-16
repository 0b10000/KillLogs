using Exiled.API.Features;
using Exiled.Events.EventArgs;
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
                Log.Debug("Suicide", plugin.Config.Debug);
                plugin.LogManager.ReportKill(ev, LogReason.RegularLogging);
                return;
            }

            if (ev.Killer.Team == ev.Target.Team)
            {
                Log.Debug("**TEAMKILL**", plugin.Config.Debug);
                plugin.LogManager.ReportKill(ev, LogReason.TeamKill, true);
                return;
            }

            if (!ev.Killer.IsScp && ev.Target.IsCuffed)
            {
                Log.Debug("**CUFFED**", plugin.Config.Debug);
                plugin.LogManager.ReportKill(ev, LogReason.CuffedKill, true);
                return;
            }

            Log.Debug("Regular kill", plugin.Config.Debug);
            plugin.LogManager.ReportKill(ev, LogReason.RegularLogging);
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            plugin.LogManager.SendQueue();
            plugin.KillWebhook.SendMessage("**=== ROUND ENDED ===**")
                .Queue(() => Log.Debug("Sent round ended message", plugin.Config.Debug));
        }

        public void OnRoundStarted()
        {
            plugin.KillWebhook.SendMessage("**=== ROUND STARTED ===**")
                .Queue(() => Log.Debug("Sent round started message", plugin.Config.Debug));
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
            }
        }
    }
}