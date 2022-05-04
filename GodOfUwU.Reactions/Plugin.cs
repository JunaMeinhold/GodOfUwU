namespace GodOfUwU.Reactions
{
    using Discord.WebSocket;
    using GodOfUwU.Services;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class ReactionsPlugin : Plugin
    {
        public override Task SetupServices(ServiceCollection services)
        {
            services.AddSingleton<ReactionService>();
            return Task.CompletedTask;
        }

        public override Task Load(IServiceProvider serviceProvider)
        {
            return Task.CompletedTask;
        }

        public override Task Initialize(DiscordSocketClient client, IServiceProvider services)
        {
            return Task.CompletedTask;
        }

        public override Task Uninitialize()
        {
            return Task.CompletedTask;
        }
    }
}