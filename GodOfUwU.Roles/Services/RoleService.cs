namespace GodOfUwU.Roles.Services
{
    using GodOfUwU.Roles.Entities;
    using Microsoft.EntityFrameworkCore;

    public class RoleService : DbContext
    {
        public RoleService()
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=roles.sqlite");
        }
    }
}