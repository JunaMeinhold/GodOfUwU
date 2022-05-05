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
        [Command("reload-plugins")]
        public async Task ReloadAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(AdminModule)))
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
            if (UserContext.CheckPermission(Context.User, typeof(AdminModule)))
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

        [Command("set-log-level")]
        public async Task SetLogLevelAsync(int level)
        {
            if (UserContext.CheckPermission(Context.User, typeof(AdminModule)))
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

        [Command("flush-permissions")]
        public async Task FlushPermissionsAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(AdminModule)))
            {
                UserContext.Current.Permissions.RemoveRange(UserContext.Current.Permissions);
                UserContext.Current.SaveChanges();
                await UserContext.Current.UpdatePermissions();
            }
        }

        [Command("list-permissions")]
        public async Task ListPermissionsAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(AdminModule)))
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

        [Command("clear")]
        public async Task ClearBotAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(AdminModule)))
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
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("clearU")]
        public async Task ClearBotAsync(IUser user)
        {
            if (UserContext.CheckPermission(Context.User, typeof(AdminModule)))
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
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("clearUL")]
        public async Task ClearBotAsync(IUser user, int limit)
        {
            if (UserContext.CheckPermission(Context.User, typeof(AdminModule)))
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
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("clearL")]
        public async Task ClearBotAsync(int limit)
        {
            if (UserContext.CheckPermission(Context.User, typeof(AdminModule)))
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
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("set-game")]
        public async Task SetGameAsync(params string[] game)
        {
            string text = string.Join(" ", game);

            if (UserContext.CheckPermission(Context.User, typeof(AdminModule)))
            {
                await Context.Client.SetGameAsync(text);
                await ReplyAsync($"Setted status to: {text}");
            }
            else
            {
                await ReplyAsync("No");
            }
        }
    }
}