namespace GodOfUwU.Core.Entities
{
    using Discord;
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
        }

        public User(ulong id)
        {
            Id = id;
        }

        public virtual ulong Id { get; set; }

        public virtual List<Role> Roles { get; set; } = new List<Role>();

        public virtual List<GuildUser> Guilds { get; set; } = new();

        public GuildUser? GetGuildUser(IGuild guild)
        {
            return GetGuildUser(guild.Id);
        }

        public GuildUser? GetGuildUser(Guild guild)
        {
            return GetGuildUser(guild.Id);
        }

        public GuildUser? GetGuildUser(ulong guild)
        {
            return Guilds.FirstOrDefault(x => x.GuildId == guild);
        }

        public Role? GetRole(IRole role)
        {
            return GetRole(role.Id);
        }

        public Role? GetRole(ulong role)
        {
            return Roles.FirstOrDefault(x => x.Id == role);
        }

        public override string ToString()
        {
            return $"{Id}";
        }
    }
}