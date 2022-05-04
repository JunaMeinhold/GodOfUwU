namespace GodOfUwU.Entities;

using Newtonsoft.Json;
using System.Text.RegularExpressions;

public class Reaction
{
    public Reaction()
    {
    }

    public Reaction(string target, string[] replies)
    {
        Target = target;
        Replies.AddRange(replies);
    }

    private Regex? regex;

    public string Target { get; set; } = string.Empty;

    public List<string> Replies { get; set; } = new();

    [JsonIgnore]
    public Regex Regex
    {
        get
        {
            if (regex == null)
            {
                regex = new Regex(Target, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            return regex;
        }
    }

    public string GetRandomReaction(Random random)
    {
        return Replies[random.Next(0, Replies.Count - 1)];
    }

    public override string ToString()
    {
        return $"{Target}:\t{string.Join(",\t", Replies)}";
    }
}