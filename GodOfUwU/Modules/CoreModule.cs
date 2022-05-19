namespace GodOfUwU.Modules
{
    using Discord;
    using Discord.Commands;
    using Discord.Interactions;
    using GodOfUwU.Core;
    using GodOfUwU.Core.Entities.Attributes;
    using System.ComponentModel;
    using System.Reflection;
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
            await ReplyAsync("Updating commands");
            PluginLoader.RegisterCommands = true;
            await PluginLoader.Reload();
        }

        [Command("clear-global-commands")]
        public async Task ClearGlobalAsync()
        {
            await Context.Client.Rest.DeleteAllGlobalCommandsAsync();
            await ReplyAsync("Clearing global commands");
        }

        [Command("register-guild-commands")]
        public async Task RegisterGuildAsync()
        {
            await service.AddCommandsToGuildAsync(Context.Guild);
            await ReplyAsync("Finished!");
        }

        [Command("clear-guild-commands")]
        public async Task ClearGuildAsync()
        {
            await Context.Guild.DeleteApplicationCommandsAsync();
            await ReplyAsync("Clearing guild commands");
        }

        [Command("help")]
        [Description("Prints out the help menu")]
        public async Task Help()
        {
            EmbedBuilder builder = new();
            StringBuilder sb = new();
            foreach (var command in commandService.Commands)
            {
                DescriptionAttribute? desc = (DescriptionAttribute?)command.Attributes.FirstOrDefault(x => x is DescriptionAttribute);
                string args = string.Join(", ", command.Parameters.Select(x => $"{x.Name}: {x.Type.Name}"));
                if (desc != null)
                    sb.AppendLine($"{command.Name}:\t ({args})\t {desc.Description}");
                else
                    sb.AppendLine($"{command.Name}:\t ({args})\t");
            }
            builder.AddField(new EmbedFieldBuilder()
            {
                Name = "Help page",
                Value = sb.ToString(),
            });
            var cbuilder = new ComponentBuilder().WithButton("About", "aboutbutton");
            await ReplyAsync(embed: builder.Build(), components: cbuilder.Build());
        }
    }
}