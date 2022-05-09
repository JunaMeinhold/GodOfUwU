namespace GodOfUwU.Core
{
    using Discord.WebSocket;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public abstract class Plugin
    {
        public int Id { get => (Name ?? string.Empty).GetHashCode(); }

        public string? Name { get; internal set; }

        public string? Version { get; internal set; }

        public string FullName { get => $"{Name}, {Version}"; }

        public List<Plugin> Dependencies { get; internal set; } = new();

        public Assembly? Assembly { get; internal set; }

        public abstract Task SetupServices(ServiceCollection services);

        public abstract Task Load(IServiceProvider serviceProvider);

        public abstract Task Initialize(DiscordSocketClient client, IServiceProvider services);

        public abstract Task Uninitialize();
    }
}