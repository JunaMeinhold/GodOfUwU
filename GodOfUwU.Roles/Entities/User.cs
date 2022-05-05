namespace GodOfUwU.Roles.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class User
    {
        public ulong Id { get; set; }

        public List<Role> Roles { get; set; } = new();
    }
}