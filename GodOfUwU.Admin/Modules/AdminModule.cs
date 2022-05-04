namespace GodOfUwU.Modules
{
    using Discord;
    using Discord.Commands;
    using GodOfUwU.Core;
    using GodOfUwU.Core.Entities.Attributes;
    using System.Threading.Tasks;

    [PermissionNamespace(typeof(AdminModule), "admin")]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
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