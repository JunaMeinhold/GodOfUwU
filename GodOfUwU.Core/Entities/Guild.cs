namespace GodOfUwU.Core.Entities
{
    using Discord;
    using System.Collections.Generic;
    using System.Linq;

    public class Guild
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Guild()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public Guild(ulong id)
        {
            Id = id;
            Users = new();
            Roles = new();
            ForbiddenTexts = new();
        }

        public virtual ulong Id { get; set; }

        public virtual List<GuildUser> Users { get; set; }

        public virtual List<Role> Roles { get; set; }

        public virtual List<ForbiddenText> ForbiddenTexts { get; set; }

        public GuildUser? GetGuildUser(IUser user)
        {
            return GetGuildUser(user.Id);
        }

        public GuildUser? GetGuildUser(User user)
        {
            return GetGuildUser(user.Id);
        }

        public GuildUser? GetGuildUser(ulong user)
        {
            return Users.FirstOrDefault(u => u.UserId == user);
        }

        public Role? GetRole(IRole role)
        {
            return GetRole(role.Id);
        }

        public Role? GetRole(ulong role)
        {
            return Roles.FirstOrDefault(u => u.Id == role);
        }
    }
}