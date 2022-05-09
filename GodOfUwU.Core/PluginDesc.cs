namespace GodOfUwU.Core
{
    using Newtonsoft.Json;

    public class PluginDesc : IEquatable<PluginDesc>
    {
        public PluginDesc(string name, string version, string path)
        {
            Name = name;
            Version = version;
            Path = path;
            Dependencies = new();
        }

        public string Name { get; set; }

        public string Version { get; set; }

        public string Path { get; set; }

        public List<string> Dependencies { get; set; }

        public static PluginDesc Load(string path)
        {
            return JsonConvert.DeserializeObject<PluginDesc>(File.ReadAllText(path)) ?? new(string.Empty, string.Empty, string.Empty);
        }

        public static IEnumerable<PluginDesc> LoadAll(string path)
        {
            foreach (var file in Directory.GetFiles(path, "*.json"))
            {
                if (file.Contains("module."))
                    yield return Load(file);
            }
        }

        public bool Equals(PluginDesc? other)
        {
            return Name == other?.Name;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PluginDesc);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Version, Path);
        }
    }
}