namespace GodOfUwU.Modules
{
    using Discord;
    using Discord.Commands;
    using GodOfUwU.Core;
    using GodOfUwU.Core.Entities;
    using GodOfUwU.Core.Entities.Attributes;
    using System.Text;
    using System.Threading.Tasks;

    [PermissionNamespace(typeof(AdminModule), "admin")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly UpdateService updateService;

        public AdminModule(UpdateService updateService)
        {
            this.updateService = updateService;
        }

        [Permission(typeof(AdminModule), "checkupdates")]
        [Command("checkupdates")]
        public async Task CheckUpdates()
        {
            string latestVersion = await updateService.GetLatestVersion();
            string currentVersion = UpdateService.GetCurrentVersion();
            int versionComparison = await updateService.CheckVersionAsync();

            StringBuilder sb = new();
            sb.AppendLine($"Current version: {currentVersion}");
            sb.AppendLine($"Latest version: {latestVersion}");
            sb.AppendLine(versionComparison < 0 ? "A new version is available" : versionComparison > 0 ? ":O you are using a newer version than my mom published" : "The bot is up to date");
            sb.AppendLine();
            sb.AppendLine("Changelog:");
            sb.AppendLine(await updateService.GetChangelog());

            EmbedBuilder builder = new();
            builder.AddField(new EmbedFieldBuilder()
            {
                Name = "Update check",
                Value = sb.ToString()
            });

            if (versionComparison < 0)
            {
                await ReplyAsync(embed: builder.Build(), components: new ComponentBuilder().WithButton("Update", "updatebutton").Build());
            }
            else
                await ReplyAsync(embed: builder.Build());
        }

        [Permission(typeof(AdminModule), "testupdatescript")]
        [Command("testupdatescript")]
        public Task TestScript()
        {
            updateService.TestUpdateScript(Context.Channel.Id);
            return Task.CompletedTask;
        }

        [Permission(typeof(AdminModule), "restart")]
        [Command("restart")]
        public async Task RestartBot()
        {
            await ReplyAsync("Restarting...");
            updateService.Restart();
        }

        [Permission(typeof(AdminModule), "reload-plugins")]
        [Command("reload-plugins")]
        public async Task ReloadAsync()
        {
            await ReplyAsync("Reloading plugins!");
            await PluginLoader.Reload();
        }

        [Permission(typeof(AdminModule), "list-plugins")]
        [Command("list-plugins")]
        public async Task ListPluginsAsync()
        {
            StringBuilder sb = new();
            sb.AppendLine("Currently loaded plugins:");
            foreach (Plugin plugin in PluginLoader.Plugins)
            {
                sb.AppendLine(plugin.FullName);
            }
            await ReplyAsync(sb.ToString());
        }

        [Permission(typeof(AdminModule), "set-log-level")]
        [Command("set-log-level")]
        public async Task SetLogLevelAsync(int level)
        {
            LogSeverity severity = (LogSeverity)level;
            Config.Default.LogLevel = severity;
            await ReplyAsync($"Log level set to {severity}");
            await PluginLoader.Reload();
        }

        [Permission(typeof(AdminModule), "clear")]
        [Command("clear")]
        public async Task ClearBotAsync()
        {
            IAsyncEnumerable<IReadOnlyCollection<IMessage>> messages = Context.Channel.GetMessagesAsync();

            await foreach (IReadOnlyCollection<IMessage> message in messages)
            {
                foreach (IMessage msg in message)
                {
                    await msg.DeleteAsync();
                }
            }
        }

        [Permission(typeof(AdminModule), "clearU")]
        [Command("clearU")]
        public async Task ClearBotAsync(IUser user)
        {
            IAsyncEnumerable<IReadOnlyCollection<IMessage>> messages = Context.Channel.GetMessagesAsync();
            await foreach (IReadOnlyCollection<IMessage> message in messages)
            {
                foreach (IMessage msg in message)
                {
                    if (msg.Author.Id == user.Id)
                        await msg.DeleteAsync();
                }
            }
        }

        [Permission(typeof(AdminModule), "clearUL")]
        [Command("clearUL")]
        public async Task ClearBotAsync(IUser user, int limit)
        {
            IAsyncEnumerable<IReadOnlyCollection<IMessage>> messages = Context.Channel.GetMessagesAsync();
            int i = 0;
            await foreach (IReadOnlyCollection<IMessage> message in messages)
            {
                foreach (IMessage msg in message)
                {
                    if (msg.Author.Id == user.Id)
                        await msg.DeleteAsync();
                    i++;
                    if (i == limit)
                        break;
                }
                if (i == limit)
                    break;
            }
        }

        [Permission(typeof(AdminModule), "clearL")]
        [Command("clearL")]
        public async Task ClearBotAsync(int limit)
        {
            IAsyncEnumerable<IReadOnlyCollection<IMessage>> messages = Context.Channel.GetMessagesAsync();
            int i = 0;
            await foreach (IReadOnlyCollection<IMessage> message in messages)
            {
                foreach (IMessage msg in message)
                {
                    await msg.DeleteAsync();
                    i++;
                    if (i == limit)
                        break;
                }
                if (i == limit)
                    break;
            }
        }

        [Permission(typeof(AdminModule), "set-game")]
        [Command("set-game")]
        public async Task SetGameAsync(params string[] game)
        {
            string text = string.Join(" ", game);

            await Context.Client.SetGameAsync(text);
            await ReplyAsync($"Setted status to: {text}");
        }
    }
}