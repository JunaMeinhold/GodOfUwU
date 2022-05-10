namespace GodOfUwU.Modules
{
    using Discord;
    using Discord.Interactions;
    using Discord.WebSocket;
    using GodOfUwU.Core.Entities.Attributes;
    using GodOfUwU.Entities;
    using GodOfUwU.Services;

    [PermissionNamespace(typeof(InteractionReactionsModule), "reactions")]
    public class InteractionReactionsModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly ReactionService _service;
        private readonly DiscordSocketClient _client;

        public InteractionReactionsModule(ReactionService service, DiscordSocketClient client)
        {
            _service = service;
            _client = client;
            _client.ModalSubmitted += ModalSubmitted;
        }

        [SlashCommand("reactions-add", "Adds a new reaction")]
        public async Task AddReaction()
        {
            var mb = new ModalBuilder()
                .WithTitle("New reaction")
                .WithCustomId("new_reaction_menu")
                .AddTextInput("Target", "reaction_target", placeholder: "Target (Regex)")
                .AddTextInput("Replies", "reaction_replies", TextInputStyle.Paragraph, "Replies");

            await Context.Interaction.RespondWithModalAsync(mb.Build());
        }

        [SlashCommand("reactions-remove", "Removes a reaction with the given index")]
        public async Task RemoveReaction(int index)
        {
            if (index >= 0 && index < _service.ReactionCount())
            {
                _service.RemoveReaction(index);
                await Context.Interaction.RespondAsync("Reaction removed.");
            }
            else
            {
                await Context.Interaction.RespondAsync("Upsi something went wrong");
            }
        }

        [SlashCommand("reactions-list", "Lists all reactions with the index")]
        public async Task ListReactions()
        {
            await Context.Interaction.RespondAsync(_service.ListReactions());
        }

        private async Task ModalSubmitted(SocketModal modal)
        {
            if (modal.HasResponded) return;
            if (modal.Data.CustomId == "new_reaction_menu")
            {
                // Get the values of components.
                List<SocketMessageComponentData> components = modal.Data.Components.ToList();

                string target = components.First(x => x.CustomId == "reaction_target").Value;
                string replies = components.First(x => x.CustomId == "reaction_replies").Value;

                string[] lines = replies.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                Reaction reaction = new(target, lines);
                _service.AddReaction(reaction);

                // Build the message to send.
                string message = $"Added new reaction with target: {target} and replies: {replies}";

                // Respond to the modal.
                await modal.RespondAsync(message);
            }
        }
    }
}