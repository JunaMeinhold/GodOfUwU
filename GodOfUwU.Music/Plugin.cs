namespace GodOfUwU.Music
{
    using Discord;
    using Discord.WebSocket;
    using GodOfUwU.Core;
    using GodOfUwU.Services;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using Victoria;

    public class MusicPlugin : Plugin
    {
        public override Task SetupServices(ServiceCollection services)
        {
            services.AddSingleton<LavaNode>().AddSingleton<LavaConfig>().AddSingleton<MusicService>();
            return Task.CompletedTask;
        }

        public override async Task Load(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<LavaConfig>().LogSeverity = LogSeverity.Error;
            await serviceProvider.GetRequiredService<MusicService>().InitializeAsync();
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