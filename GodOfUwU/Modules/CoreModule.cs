namespace GodOfUwU.Modules
{
    using Discord;
    using Discord.Commands;
    using Discord.Interactions;
    using GodOfUwU.Core;
    using GodOfUwU.Core.Entities;
    using GodOfUwU.Core.Entities.Attributes;
    using System.Text;
    using System.Threading.Tasks;

    [PermissionNamespace(typeof(CoreModule), "core")]
    public class CoreModule : ModuleBase<SocketCommandContext>
    {
        private readonly InteractionService service;

        public CoreModule(InteractionService service)
        {
            this.service = service;
        }

        [Command("reload-plugins")]
        public async Task ReloadAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(CoreModule)))
            {
                await ReplyAsync("Reloading plugins!");
                await PluginLoader.Reload();
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("list-plugins")]
        public async Task ListPluginsAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(CoreModule)))
            {
                StringBuilder sb = new();
                sb.AppendLine("Currently loaded plugins:");
                foreach (Plugin plugin in PluginLoader.Plugins)
                {
                    sb.AppendLine(plugin.FullName);
                }
                await ReplyAsync(sb.ToString());
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("list-permissions")]
        public async Task ListPermissionsAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(CoreModule)))
            {
                StringBuilder sb = new();
                sb.AppendLine("Permissions:");
                foreach (Permission permission in UserContext.Current.Permissions)
                {
                    sb.AppendLine(permission.Name);
                }
                await ReplyAsync(sb.ToString());
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("set-log-level")]
        public async Task SetLogLevelAsync(int level)
        {
            if (UserContext.CheckPermission(Context.User, typeof(CoreModule)))
            {
                LogSeverity severity = (LogSeverity)level;
                Config.Default.LogLevel = severity;
                await ReplyAsync($"Log level set to {severity}");
                await PluginLoader.Reload();
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("register-commands")]
        public async Task RegisterGlobalAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(CoreModule)))
            {
                await ReplyAsync("Updating commands");
                GodUwUClient.RegisterCommands = true;
                await PluginLoader.Reload();
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("clear-global-commands")]
        public async Task ClearGlobalAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(CoreModule)))
            {
                await Context.Client.Rest.DeleteAllGlobalCommandsAsync();
                await ReplyAsync("Clearing global commands");
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("register-guild-commands")]
        public async Task RegisterGuildAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(CoreModule)))
            {
                await service.AddCommandsToGuildAsync(Context.Guild);
                await ReplyAsync("Finished!");
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("clear-guild-commands")]
        public async Task ClearGuildAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(CoreModule)))
            {
                await Context.Guild.DeleteApplicationCommandsAsync();
                await ReplyAsync("Clearing guild commands");
            }
            else
            {
                await ReplyAsync("No");
            }
        }
    }
}