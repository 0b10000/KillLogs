using System;
using System.Collections.Generic;
using DSharp4Webhook.Core;
using DSharp4Webhook.Core.Constructor;
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
        public override Version Version { get; } = new(9, 6, 0);
        public override Version RequiredExiledVersion { get; } = new(9,6,0);

        private EventHandlers EventHandlers { get; set; }
        internal Methods Methods { get; set; }

        public LogManager LogManager { get; private set; }

        private WebhookProvider WebhookProvider { get; set; }
        internal IWebhook KillWebhook { get; set; }
        
        internal MessageBuilder MessageBuilder { get; set; }
        
        internal List<Player> PlayersToNotify { get; set; }

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Methods = new Methods(this);
            LogManager = new LogManager(this);

            PlayersToNotify = new List<Player>();

            WebhookProvider = new WebhookProvider("0b10000.kill_logs");
            WebhookProvider.AllowedMention = AllowedMention.ROLES;

            MessageBuilder = ConstructorProvider.GetMessageBuilder();
            
            KillWebhook = WebhookProvider.CreateWebhook(Config.DiscordWebhookUrl);

            PlayerEvents.Dying += EventHandlers.OnDying;
            PlayerEvents.Verified += EventHandlers.OnVerified;
            PlayerEvents.Left += EventHandlers.OnLeft;

            ServerEvents.RoundEnded += EventHandlers.OnRoundEnded;
            ServerEvents.RoundStarted += EventHandlers.OnRoundStarted;
            ServerEvents.RespawningTeam += EventHandlers.OnRespawningTeam;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerEvents.Dying -= EventHandlers.OnDying;
            PlayerEvents.Verified -= EventHandlers.OnVerified;
            PlayerEvents.Left -= EventHandlers.OnLeft;

            ServerEvents.RoundEnded -= EventHandlers.OnRoundEnded;
            ServerEvents.RoundStarted -= EventHandlers.OnRoundStarted;
            ServerEvents.RespawningTeam -= EventHandlers.OnRespawningTeam;

            KillWebhook = null;
            WebhookProvider = null;
            EventHandlers = null;
            Methods = null;
            LogManager = null;
            PlayersToNotify = null;

            base.OnDisabled();
        }
    }
}
