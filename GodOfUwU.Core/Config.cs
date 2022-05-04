namespace GodOfUwU.Core;

using Discord;
using Newtonsoft.Json;

public class Config
{
    private static readonly JsonSerializerSettings settings = new()
    {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Include,
        TypeNameHandling = TypeNameHandling.Auto,
    };

    private LogSeverity logLevel = LogSeverity.Debug;
    private string permissionsFile = "permissions.sqlite";

    static Config()
    {
        Default = Load();
        Default.Save();
    }

    public static Config Default { get; set; }

    public string PermissionsFile { get => permissionsFile; set => SetAndSave(ref permissionsFile, value); }

    public LogSeverity LogLevel { get => logLevel; set => SetAndSave(ref logLevel, value); }

    public Dictionary<string, object> Properties { get; set; } = new();

    public void AddOrUpdate<T>(string key, T value) where T : class
    {
        if (Properties.ContainsKey(key))
        {
            Properties[key] = value;
        }
        else
        {
            Properties.Add(key, value);
        }
        Save();
    }

    public T? Get<T>(string key) where T : class
    {
        return Properties[key] as T;
    }

    public bool TryGet<T>(string key, out T? value) where T : class
    {
        if (Properties.TryGetValue(key, out object? _value))
        {
            if (_value is T t)
            {
                value = t;
                return true;
            }
        }
        value = default;
        return false;
    }

    public bool Remove(string key)
    {
        return Properties.Remove(key);
    }

    public bool Contains(string key)
    {
        return Properties.ContainsKey(key);
    }

    public string? GetKeyByValue<T>(T value) where T : class
    {
        foreach (var pair in Properties)
        {
            if (pair.Value is T t && t == value)
            {
                return pair.Key;
            }
        }
        return null;
    }

    public IEnumerable<string> GetKeysByValue<T>(T value) where T : class
    {
        foreach (var pair in Properties)
        {
            if (pair.Value is T t && t == value)
            {
                yield return pair.Key;
            }
        }
    }

    private void SetAndSave<T>(ref T field, T value)
    {
        field = value;
        Save();
    }

    public static Config Load()
    {
        Config config;
        if (File.Exists("config.json"))
        {
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"), settings) ?? new();
            config.Save();
        }
        else
        {
            config = new Config();
            config.Save();
        }

        return config;
    }

    public static void Reload()
    {
        Default = Load();
        Default.Save();
    }

    public void Save()
    {
        File.WriteAllText("config.json", JsonConvert.SerializeObject(this, settings));
    }
}