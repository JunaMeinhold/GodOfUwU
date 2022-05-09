namespace GodOfUwU.Core.Entities.Attributes
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : Attribute
    {
        public PermissionAttribute(string space, string command)
        {
            PermissionString = $"{space}.{command}";
            Space = space;
            Command = command;
        }

        public PermissionAttribute(Type type, string command)
        {
            PermissionNamespaceAttribute attr = type.GetCustomAttribute<PermissionNamespaceAttribute>() ?? throw new Exception();
            PermissionString = $"{attr.Name}.{command}";
            Space = attr.Name;
            Command = command;
        }

        public string PermissionString { get; }

        public string Space { get; }

        public string Command { get; }

        public static implicit operator Permission(PermissionAttribute attribute)
        {
            return new Permission() { Name = attribute.PermissionString };
        }
    }
}