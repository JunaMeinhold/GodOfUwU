namespace GodOfUwU.Services;

using Discord.WebSocket;
using GodOfUwU.Entities;
using System.Text;
using System.Threading.Tasks;

public class ReactionService
{
    private readonly DiscordSocketClient _client;
    private readonly Random random = new();
    private readonly ReactionContext context;

    public ReactionService(DiscordSocketClient client)
    {
        _client = client;
        _client.MessageReceived += MessageReceived;
        context = new();
    }

    public void AddReaction(Reaction reaction)
    {
        context.Reactions.Add(reaction);
        context.SaveChanges();
    }

    public int ReactionCount()
    {
        return context.Reactions.Count();
    }

    public void RemoveReaction(int index)
    {
        context.Reactions.Remove(context.Reactions.ElementAt(index));
        context.SaveChanges();
    }

    public string ListReactions()
    {
        if (!context.Reactions.Any())
            return "No reactions in list";
        StringBuilder sb = new();
        int i = 0;
        foreach (var reaction in context.Reactions)
        {
            sb.AppendLine($"{i}:\t {reaction}");
            i++;
        }
        return sb.ToString();
    }

    private async Task MessageReceived(SocketMessage arg)
    {
        if (arg.Author.IsBot)
            return;
        if (arg.CleanContent.StartsWith("!") || arg.CleanContent.StartsWith("/"))
            return;

        Reaction? reaction = context.Reactions.FirstOrDefault(x => x.Regex.IsMatch(arg.CleanContent));

        if (reaction != null)
        {
            await arg.Channel.SendMessageAsync(reaction.GetRandomReaction(random), messageReference: arg.Reference);
        }
    }
}