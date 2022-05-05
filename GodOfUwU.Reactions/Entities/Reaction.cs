namespace GodOfUwU.Entities;

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

public class Reaction
{
    public Reaction()
    {
    }

    public Reaction(string target, string[] replies)
    {
        Target = target;
        Replies.AddRange(replies.Select(x => new Reply() { Text = x }));
    }

    private Regex? regex;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Target { get; set; } = string.Empty;

    public List<Reply> Replies { get; set; } = new();

    [NotMapped]
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
        return Replies[random.Next(0, Replies.Count - 1)].Text;
    }

    public override string ToString()
    {
        return $"{Target}:\t{string.Join(",\t", Replies.Select(x => $"{x.Id}: {x.Text}"))}";
    }
}

public class Reply
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Text { get; set; }
}