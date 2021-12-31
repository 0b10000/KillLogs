using System;
using DSharp4Webhook.Core;
using Exiled.API.Features;
using MapEvents = Exiled.Events.Handlers.Map;
using PlayerEvents = Exiled.Events.Handlers.Player;
using Scp049Events = Exiled.Events.Handlers.Scp049;
using Scp079Events = Exiled.Events.Handlers.Scp079;
using Scp096Events = Exiled.Events.Handlers.Scp096;
using Scp106Events = Exiled.Events.Handlers.Scp106;
using Scp914Events = Exiled.Events.Handlers.Scp914;
using ServerEvents = Exiled.Events.Handlers.Server;
using WarheadEvents = Exiled.Events.Handlers.Warhead;

namespace KillLogs
{
    public class Plugin : Plugin<Config>
    {
        public override string Author => "0b10000";
        public override string Name => "KillLogs";
        public override string Prefix => "KillLogs";
        public override Version Version { get; } = new(2, 0, 2);
        public override Version RequiredExiledVersion { get; } = new(4, 1, 7);

        private EventHandlers EventHandlers { get; set; }

        public LogManager LogManager { get; private set; }

        private WebhookProvider WebhookProvider { get; set; }
        internal IWebhook KillWebhook { get; set; }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            LogManager = new LogManager(this);

            WebhookProvider = new WebhookProvider("0b10000.kill_logs");
            WebhookProvider.AllowedMention = AllowedMention.ROLES;
            KillWebhook = WebhookProvider.CreateWebhook(Config.DiscordWebhookUrl);

            PlayerEvents.Dying += EventHandlers.OnDying;

            ServerEvents.RoundEnded += EventHandlers.OnRoundEnded;
            ServerEvents.RoundStarted += EventHandlers.OnRoundStarted;
            ServerEvents.RespawningTeam += EventHandlers.OnRespawningTeam;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerEvents.Dying -= EventHandlers.OnDying;

            ServerEvents.RoundEnded -= EventHandlers.OnRoundEnded;
            ServerEvents.RoundStarted -= EventHandlers.OnRoundStarted;
            ServerEvents.RespawningTeam -= EventHandlers.OnRespawningTeam;

            KillWebhook = null;
            WebhookProvider = null;
            EventHandlers = null;
            LogManager = null;

            base.OnDisabled();
        }
    }
}
