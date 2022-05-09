namespace GodOfUwU.Music
{
    using Discord.WebSocket;
    using GodOfUwU.Core;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class AdminPlugin : Plugin
    {
        public override Task SetupServices(ServiceCollection services)
        {
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