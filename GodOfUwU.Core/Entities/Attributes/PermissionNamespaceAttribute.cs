namespace GodOfUwU.Core.Entities.Attributes
{
    using Discord.Commands;
    using Discord.Interactions;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Class)]
    public class PermissionNamespaceAttribute : Attribute
    {
        public PermissionNamespaceAttribute(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; }
        public string Name { get; }

        public string FindPermission(string method)
        {
            foreach (MethodInfo info in Type.GetMethods())
            {
                if (info.Name == method)
                {
                    {
                        CommandAttribute? attr = info.GetCustomAttribute<CommandAttribute>();
                        if (attr != null)
                        {
                            return $"{Name}.{attr.Text}";
                        }
                    }

                    {
                        SlashCommandAttribute? attr = info.GetCustomAttribute<SlashCommandAttribute>();
                        if (attr != null)
                        {
                            return $"{Name}.{attr.Name}";
                        }
                    }
                }
            }

            return string.Empty;
        }

        public IEnumerable<Permission> GetPermissions()
        {
            yield return new Permission() { Name = $"*" };
            yield return new Permission() { Name = $"{Name}.*" };
            foreach (MethodInfo info in Type.GetMethods())
            {
                {
                    CommandAttribute? attr = info.GetCustomAttribute<CommandAttribute>();
                    if (attr != null)
                    {
                        yield return new Permission() { Name = $"{Name}.{attr.Text}" };
                    }
                }

                {
                    SlashCommandAttribute? attr = info.GetCustomAttribute<SlashCommandAttribute>();
                    if (attr != null)
                    {
                        yield return new Permission() { Name = $"{Name}.{attr.Name}" };
                    }
                }
            }
        }
    }
}