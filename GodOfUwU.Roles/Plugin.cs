namespace GodOfUwU.Roles
{
    using Discord.WebSocket;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class RolePlugin : Plugin
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