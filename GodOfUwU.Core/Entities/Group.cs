namespace GodOfUwU.Core.Entities
{
    using System.Collections.Generic;

    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        public ICollection<User> Users { get; set; } = new List<User>();

        public const string DefaultGroup = "Default";

        public const string RootGroup = "Root";

        public override string ToString()
        {
            return Name;
        }
    }
}