namespace GodOfUwU.Core.Entities
{
    using System.Collections.Generic;

    public class User
    {
        public ulong Id { get; set; }

        public ICollection<Group> Groups { get; set; } = new List<Group>();

        public override string ToString()
        {
            return $"{Id}";
        }
    }
}