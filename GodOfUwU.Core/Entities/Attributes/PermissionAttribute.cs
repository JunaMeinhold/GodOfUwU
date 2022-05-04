namespace GodOfUwU.Core.Entities.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : Attribute
    {
        public PermissionAttribute(string space, string command)
        {
            PermissionString = $"{space}.{command}";
        }

        public string PermissionString { get; }

        public static implicit operator Permission(PermissionAttribute attribute)
        {
            return new Permission() { Name = attribute.PermissionString };
        }
    }
}