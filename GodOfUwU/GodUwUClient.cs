namespace GodOfUwU
{
    using Discord;
    using Discord.Commands;
    using Discord.Interactions;
    using Discord.WebSocket;
    using GodOfUwU.Core;
    using GodOfUwU.Core.Handlers;
    using GodOfUwU.Core.Services;
    using GodOfUwU.Entities;
    using GodOfUwU.Services;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;
    using Victoria;

    public class GodUwUClient
    {
        private readonly LogService logService;
        private DiscordSocketClient client;
        private InteractionService interactionService;
        private CommandService cmdService;
        private IServiceProvider? services;
        private CoreService? coreService;

        public GodUwUClient()
        {
            logService = new();
            PluginLoader.Reloaded += PluginLoader_Reloaded;
            PluginLoader.BeginUnload += PluginLoader_BeginUnload;
            PluginLoader.Log += LogAsync;
            PluginLoader.Load().Wait();

            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 50,
                LogLevel = Config.Default.LogLevel,
            });

            cmdService = new CommandService(new CommandServiceConfig
            {
                LogLevel = Config.Default.LogLevel,
                CaseSensitiveCommands = false
            });

            interactionService = new(client, new InteractionServiceConfig()
            {
                LogLevel = Config.Default.LogLevel,
                UseCompiledLambda = true,
            });
        }

        private void Initialize()
        {
            Config.Reload();
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 50,
                LogLevel = Config.Default.LogLevel,
            });

            cmdService = new CommandService(new CommandServiceConfig
            {
                LogLevel = Config.Default.LogLevel,
                CaseSensitiveCommands = false
            });

            interactionService = new(client, new InteractionServiceConfig()
            {
                LogLevel = Config.Default.LogLevel,
                ThrowOnError = true,
            });
        }

        private async Task PluginLoader_BeginUnload()
        {
            await client.StopAsync();
        }

        private async Task PluginLoader_Reloaded()
        {
            Initialize();
            await InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            ServiceCollection serviceDescriptors = new();
            foreach (Plugin plugin in PluginLoader.Plugins)
            {
                await plugin.SetupServices(serviceDescriptors);
            }

            services = SetupServices(serviceDescriptors);
            client.Log += LogAsync;
            client.Ready += Ready;

            coreService = services.GetService<CoreService>();

            foreach (Plugin plugin in PluginLoader.Plugins)
            {
                await plugin.Load(services);
            }

            await client.LoginAsync(TokenType.Bot, TokenConfig.Default.Token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task Ready()
        {
            if (services == null) return;
            foreach (Plugin plugin in PluginLoader.Plugins)
            {
                await LogAsync(new(LogSeverity.Info, "Client", $"{plugin.Name}, {plugin.Version} initialized!"));
                await plugin.Initialize(client, services);
            }

            await client.SetGameAsync("");

            var cmdHandler = new CommandHandler(client, cmdService, services);
            await cmdHandler.InitializeAsync();

            var intHandler = new InteractionHandler(client, interactionService, services);
            await intHandler.InitializeAsync();

#if DEBUG
            await client.SetGameAsync("debug mode");
#endif
        }

        private async Task LogAsync(LogMessage logMessage)
        {
            if ((int)logMessage.Severity <= 3)
                await logService.LogAsync(logMessage);
        }

        private IServiceProvider SetupServices(ServiceCollection services)
            => services
                    .AddSingleton(client)
                    .AddSingleton(cmdService)
                    .AddSingleton(interactionService)
                    .AddSingleton(logService)
                    .AddDbContext<UserContext>()
                    .AddSingleton<CoreService>()
                    .AddSingleton<LavaNode>()
                    .AddSingleton<LavaConfig>()
                    .AddSingleton<MusicService>()
                    .AddSingleton<ReactionService>()
                    .BuildServiceProvider();
    }
}