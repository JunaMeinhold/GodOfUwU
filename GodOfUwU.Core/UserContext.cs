namespace GodOfUwU.Core
{
    using Discord;
    using Discord.Commands;
    using GodOfUwU.Core.Entities;
    using GodOfUwU.Core.Entities.Attributes;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class UserContext : DbContext
    {
#nullable disable
        public DbSet<Guild> Guilds { get; set; }

        public DbSet<GuildUser> GuildUsers { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

#nullable enable

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

        public bool CheckPermission(IUser duser, CommandInfo command)
        {
            if (duser.Id == 308203742736678914)
                return true;

            User? user = Users.FirstOrDefault(x => x.Id == duser.Id);
            if (user == null)
                return false;

            PermissionAttribute? attr = (PermissionAttribute?)command.Attributes.FirstOrDefault(x => x is PermissionAttribute);
            if (attr == null)
                return true;

            string perm = attr.PermissionString;
            return user.Roles.Any(x => x.Permissions.Any(y => y.Name == "*" || y.Name == perm || y.Name == attr.Space + ".*"));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite($"Data Source=database.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<GuildUser>()
                .HasKey(gu => new { gu.UserId, gu.GuildId });

            modelBuilder
                .Entity<GuildUser>()
                .HasOne(u => u.Guild)
                .WithMany(u => u.Users)
                .HasForeignKey(gu => gu.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<GuildUser>()
                .HasOne(u => u.User)
                .WithMany(u => u.Guilds)
                .HasForeignKey(gu => gu.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}