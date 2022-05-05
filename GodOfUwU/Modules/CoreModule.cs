namespace GodOfUwU.Modules
{
    using Discord.Commands;
    using Discord.Interactions;
    using GodOfUwU.Core;
    using GodOfUwU.Core.Entities.Attributes;
    using System.ComponentModel;
    using System.Text;
    using System.Threading.Tasks;

    [PermissionNamespace(typeof(CoreModule), "core")]
    public class CoreModule : ModuleBase<SocketCommandContext>
    {
        private readonly InteractionService service;
        private readonly CommandService commandService;

        public CoreModule(InteractionService service, CommandService commandService)
        {
            this.service = service;
            this.commandService = commandService;
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

        [Command("help")]
        [Description("Prints out the help menu")]
        public async Task Help()
        {
            StringBuilder sb = new();
            sb.AppendLine("Help Menu");
            foreach (var command in commandService.Commands)
            {
                DescriptionAttribute? desc = (DescriptionAttribute?)command.Attributes.FirstOrDefault(x => x is DescriptionAttribute);
                string args = string.Join(", ", command.Parameters.Select(x => $"{x.Name}: {x.Type.Name}"));
                if (desc != null)
                    sb.AppendLine($"{command.Name}:\t ({args})\t {desc.Description}");
                else
                    sb.AppendLine($"{command.Name}:\t ({args})\t");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}