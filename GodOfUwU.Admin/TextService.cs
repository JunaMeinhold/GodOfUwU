namespace GodOfUwU.Services;

using Discord;
using Discord.WebSocket;
using System.Text.RegularExpressions;

public class TextService
{
    private static readonly Regex regex = new("n(?=[aeiou])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private readonly DiscordSocketClient _client;

    public TextService(DiscordSocketClient client)
    {
        _client = client;
        _client.MessageReceived += MessageReceived;
    }

    private async Task MessageReceived(SocketMessage arg)
    {
        if (arg.Author.IsBot)
            return;

        if (arg.Author is not IGuildUser user) return;

        if (user.GuildId != 882227476166758410u) return;

        if (!IsValidMessage(arg.Content))
        {
            await arg.DeleteAsync();
            await arg.Channel.SendMessageAsync("You viowated ffe waw");
        }
    }

    private static bool IsValidMessage(string msg)
    {
        if (msg == null) return true;
        if (msg.StartsWith('/') | msg.StartsWith('!'))
            return true;
        if (msg.Contains('r', StringComparison.CurrentCultureIgnoreCase))
            return false;
        if (msg.Contains('l', StringComparison.CurrentCultureIgnoreCase))
            return false;
        if (msg.Contains("th", StringComparison.CurrentCultureIgnoreCase))
            return false;
        if (regex.IsMatch(msg))
            return false;

        return true;
    }
}