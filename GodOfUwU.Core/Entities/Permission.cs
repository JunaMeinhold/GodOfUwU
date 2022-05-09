namespace GodOfUwU.Core.Entities
{
    using GodOfUwU.Core.Entities.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public class Permission
    {
        [Key]
        public virtual string Name { get; set; } = string.Empty;

        public virtual List<Role> Roles { get; set; } = new();

        public static IEnumerable<Permission> GetPermissions(Assembly assembly)
        {
            return assembly.GetTypes().Select(x => x.GetCustomAttribute<PermissionNamespaceAttribute>()).SelectMany(x => x?.GetPermissions() ?? Array.Empty<Permission>());
        }

        public override string ToString()
        {
            return Name;
        }
    }
}