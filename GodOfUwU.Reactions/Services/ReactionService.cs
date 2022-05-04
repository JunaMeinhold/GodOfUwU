namespace GodOfUwU.Services;

using Discord.WebSocket;
using GodOfUwU.Entities;
using System.Text;
using System.Threading.Tasks;

public class ReactionService
{
    private const string Path = "reactions.json";
    private readonly DiscordSocketClient _client;
    private readonly Random random = new();
    private ReactionFile file;

    public ReactionService(DiscordSocketClient client)
    {
        _client = client;
        _client.MessageReceived += MessageReceived;
        file = ReactionFile.Load(Path);
    }

    public void ReloadReactions()
    {
        file = ReactionFile.Load(Path);
    }

    public void AddReaction(Reaction reaction)
    {
        file.Reactions.Add(reaction);
        file.Save(Path);
    }

    public int ReactionCount()
    {
        return file.Reactions.Count;
    }

    public void RemoveReaction(int index)
    {
        file.Reactions.RemoveAt(index);
        file.Save(Path);
    }

    public string ListReactions()
    {
        if (file.Reactions.Count == 0)
            return "No reactions in list";
        StringBuilder sb = new();
        int i = 0;
        foreach (var reaction in file.Reactions)
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

        Reaction? reaction = file.Reactions.FirstOrDefault(x => x.Regex.IsMatch(arg.CleanContent));

        if (reaction != null)
        {
            await arg.Channel.SendMessageAsync(reaction.GetRandomReaction(random), messageReference: arg.Reference);
        }
    }
}