namespace GodOfUwU.Entities;
using Newtonsoft.Json;

public class ReactionFile
{
    public List<Reaction> Reactions { get; set; } = new();

    public static ReactionFile Load(string path)
    {
        if (File.Exists(path))
        {
            return JsonConvert.DeserializeObject<ReactionFile>(File.ReadAllText(path)) ?? new();
        }
        else
        {
            ReactionFile file = new();
            file.Save(path);
            return file;
        }
    }

    public void Save(string path)
    {
        File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
    }
}