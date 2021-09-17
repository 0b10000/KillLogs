using Exiled.Events.EventArgs;

namespace KillLogs
{
    public class EventHandlers
    {
        private readonly Plugin plugin;
        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        public void OnDying(DyingEventArgs ev)
        {
            if (ev.Killer == null) return;
            if (ev.Target == null) return;
            
            if (ev.Killer.IsScp && !plugin.Config.LogScpKills) return;
            
            if (ev.Killer.Team == ev.Target.Team)
            {
                plugin.LogManager.ReportKill(ev, LogReason.TeamKill);
                return;
            }

            if (!ev.Killer.IsScp && ev.Target.IsCuffed)
            {
                plugin.LogManager.ReportKill(ev, LogReason.CuffedKill);
                return;
            }
            
            plugin.LogManager.ReportKill(ev, LogReason.RegularLogging);
        }

        public void OnEndingRound(EndingRoundEventArgs ev)
        {
            plugin.LogManager.SendQueue();
        }
    }
}