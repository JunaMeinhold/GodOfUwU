namespace GodOfUwU.Entities
{
    using Newtonsoft.Json;

    internal class TokenConfig
    {
        static TokenConfig()
        {
            Default = Load();
        }

        public string? Token { get; set; }

        internal static TokenConfig Default { get; set; }

        private static TokenConfig Load()
        {
            TokenConfig config;
            if (File.Exists("token.json"))
            {
                config = JsonConvert.DeserializeObject<TokenConfig>(File.ReadAllText("token.json")) ?? new();
                config.Save();
            }
            else
            {
                config = new TokenConfig();
                config.Save();
            }

            return config;
        }

        private void Save()
        {
            File.WriteAllText("token.json", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}