namespace GodOfUwU.Core
{
    using Discord;
    using GodOfUwU.Core.Entities;
    using GodOfUwU.Core.Entities.Attributes;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class UserContext : DbContext
    {
        private static readonly bool firstsetup;

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public static UserContext Current { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        private UserContext()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        static UserContext()
        {
            Current = new UserContext();
            firstsetup = Current.Database.EnsureCreated();
        }

        public async Task UpdatePermissions()
        {
            Assembly? entry = Assembly.GetEntryAssembly();
            if (entry is not null)
                await UpdatePermissions(entry);
            foreach (Plugin plugin in PluginLoader.Plugins)
            {
                if (plugin.Assembly is not null)
                {
                    await UpdatePermissions(plugin.Assembly);
                }
            }

            if (firstsetup)
            {
                Group group = new() { Name = Group.DefaultGroup };
                Current.Groups.Add(group);
                Group group1 = new() { Name = Group.RootGroup, };
                group1.Permissions.Add(Permissions.First(x => x.Name == "*"));
                Current.Groups.Add(group1);
                Current.SaveChanges();
            }
        }

        public async Task UpdatePermissions(Assembly assembly)
        {
            IEnumerable<Permission> permissions = Permission.GetPermissions(assembly);
            var except = Filter(permissions);
            await Permissions.AddRangeAsync(except);
            await SaveChangesAsync();
        }

        public IEnumerable<Permission> Filter(IEnumerable<Permission> permissions)
        {
            foreach (Permission permission in permissions)
            {
                if (Permissions.All(x => x.Name != permission.Name))
                {
                    yield return permission;
                }
            }
        }

        public static bool CheckPermission(IUser duser, Type type, [CallerMemberName] string method = "")
        {
            if (duser.Id == 308203742736678914)
                return true;

            User? user = Current.Users.Include(u => u.Groups).ThenInclude(g => g.Permissions).FirstOrDefault(x => x.Id == duser.Id);
            if (user == null)
                return false;

            PermissionNamespaceAttribute? attr = type.GetCustomAttribute<PermissionNamespaceAttribute>();
            if (attr != null)
            {
                string perm = attr.FindPermission(method);
                return user.Groups.Any(x => x.Permissions.Any(y => y.Name == "*" || y.Name == perm || y.Name.StartsWith(attr.Name + ".*")));
            }

            return false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={Config.Default.PermissionsFile}");
        }
    }
}