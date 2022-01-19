namespace KillLogs
{
    public class Methods
    {
        private readonly Plugin plugin;
        public Methods(Plugin plugin) => this.plugin = plugin;

        internal void SendHintToNotifiablePlayers(string message)
        {
            foreach (var player in plugin.PlayersToNotify)
            {
                player.ShowHint(message, 5f);
            }
        }
    }
}