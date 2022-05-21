namespace GodOfUwU.Modules;

using Discord;
using Discord.Interactions;
using GodOfUwU.Core;
using System.Reflection;
using System.Text;

public class InfoModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly UpdateService updateService;

    public InfoModule(UpdateService updateService)
    {
        this.updateService = updateService;
    }

    [ComponentInteraction("aboutbutton")]
    public async Task About()
    {
        EmbedBuilder builder = new();

        StringBuilder sb = new();
        sb.AppendLine($"currently running version {Assembly.GetExecutingAssembly().GetName().Version}");
        sb.AppendLine("created by Juna");
        builder.AddField(new EmbedFieldBuilder()
        {
            Name = "About",
            Value = sb.ToString(),
        });
        await Context.Interaction.RespondAsync(embed: builder.Build());
    }

    [ComponentInteraction("updatebutton")]
    public async Task Update()
    {
        if (Context.User.Id != 308203742736678914) return;
        await Context.Interaction.RespondAsync("Update in progress...");
        await updateService.Update(Context.Channel.Id);
    }
}