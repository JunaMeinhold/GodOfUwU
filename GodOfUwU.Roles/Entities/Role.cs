namespace GodOfUwU.Roles.Entities
{
    using GodOfUwU.Core.Entities;
    using System.Collections.Generic;

    public class Role
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}