namespace GodOfUwU.Core.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Role
    {
#nullable disable

        public Role()
        {
        }

#nullable enable

        public Role(ulong id)
        {
            Id = id;
            Users = new();
            Permissions = new();
        }

        public virtual ulong Id { get; set; }

        public virtual List<GuildUser> Users { get; set; }

        public virtual List<Permission> Permissions { get; set; }
    }
}