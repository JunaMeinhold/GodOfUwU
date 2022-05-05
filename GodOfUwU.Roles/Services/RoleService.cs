namespace GodOfUwU.Roles.Services
{
    using Discord.WebSocket;
    using System.Threading.Tasks;

    public class RoleService
    {
        private readonly DiscordSocketClient _client;

        public RoleService(DiscordSocketClient client)
        {
            _client = client;
            _client.GuildAvailable += GuildAvailable;
            _client.GuildScheduledEventUserAdd += _client_GuildScheduledEventUserAdd;
            _client.GuildScheduledEventUserRemove += _client_GuildScheduledEventUserRemove;
            _client.GuildMembersDownloaded += _client_GuildMembersDownloaded;
            _client.Ready += _client_Ready;
        }

        private async Task _client_Ready()
        {
            await _client.DownloadUsersAsync(_client.Guilds);
        }

        private Task _client_GuildMembersDownloaded(SocketGuild arg)
        {
            throw new NotImplementedException();
        }

        private Task _client_GuildScheduledEventUserRemove(Discord.Cacheable<SocketUser, Discord.Rest.RestUser, Discord.IUser, ulong> arg1, SocketGuildEvent arg2)
        {
            throw new NotImplementedException();
        }

        private Task _client_GuildScheduledEventUserAdd(Discord.Cacheable<SocketUser, Discord.Rest.RestUser, Discord.IUser, ulong> arg1, SocketGuildEvent arg2)
        {
            throw new NotImplementedException();
        }

        private Task GuildAvailable(SocketGuild arg)
        {
            return Task.CompletedTask;
        }
    }
}