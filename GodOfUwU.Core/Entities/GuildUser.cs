namespace GodOfUwU.Core.Entities
{
    using System;

    public class GuildUser
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public GuildUser()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public GuildUser(User user, Guild guild)
        {
            UserId = user.Id;
            User = user;
            GuildId = guild.Id;
            Guild = guild;
            LastActivity = DateTime.UtcNow;
            Roles = new();
            Warnings = new();
        }

        public virtual ulong UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ulong GuildId { get; set; }

        public virtual Guild Guild { get; set; }

        public virtual DateTime LastActivity { get; set; }

        public virtual List<Role> Roles { get; set; }

        public virtual List<Warning> Warnings { get; set; }
    }
}