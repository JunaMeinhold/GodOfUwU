namespace GodOfUwU.Roles.Modules
{
    using Discord;
    using Discord.Commands;
    using GodOfUwU.Core;
    using GodOfUwU.Core.Entities.Attributes;
    using GodOfUwU.Roles.Entities;
    using GodOfUwU.Roles.Services;
    using Microsoft.EntityFrameworkCore;
    using System.Text;
    using System.Threading.Tasks;

    [PermissionNamespace(typeof(RoleModule), "roles")]
    public class RoleModule : ModuleBase<SocketCommandContext>
    {
        private readonly RoleService service;

        public RoleModule(RoleService service)
        {
            this.service = service;
        }

        [Command("register")]
        public async Task RegisterCommand()
        {
            User? user = await service.Users.FirstOrDefaultAsync(u => u.Id == Context.User.Id);

            if (user == null)
            {
                user = new User() { Id = Context.User.Id };
                await service.Users.AddAsync(user);
                await service.SaveChangesAsync();
                await ReplyAsync("Succesfully registered!");
            }
            else
            {
                await ReplyAsync("You are already registered!");
            }
        }

        [Command("update")]
        public async Task UpdateCommand()
        {
            User? user = await service.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == Context.User.Id);

            if (user == null)
            {
                await ReplyAsync("Please register with !register");
                return;
            }
            else
            {
                foreach (Role role in user.Roles)
                {
                    IRole? drole = Context.Guild.Roles.FirstOrDefault(x => x.Name == role.Name);

                    if (drole == null)
                    {
                        drole = await Context.Guild.CreateRoleAsync(role.Name);
                    }

                    IGuildUser guildUser = Context.Guild.GetUser(Context.User.Id);

                    await guildUser.AddRoleAsync(drole);
                }

                await ReplyAsync("Succesfully updated roles for you");
            }
        }

        [Command("role-add")]
        public async Task RoleAdd(params string[] roleNamepart)
        {
            if (UserContext.CheckPermission(Context.User, typeof(RoleModule)))
            {
                string roleName = string.Join("", roleNamepart);
                Role? role = service.Roles.Find(roleName);
                if (role != null)
                {
                    await ReplyAsync("Role already exists");
                    return;
                }

                role = new() { Name = roleName };
                await service.Roles.AddAsync(role);
                await service.SaveChangesAsync();
                await ReplyAsync($"Succesfully added role {role.Name}!");
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("role-remove")]
        public async Task RoleRemove(params string[] roleNamepart)
        {
            if (UserContext.CheckPermission(Context.User, typeof(RoleModule)))
            {
                string roleName = string.Join("", roleNamepart);
                Role? role = service.Roles.Find(roleName);
                if (role == null)
                {
                    await ReplyAsync("Role does not exists");
                    return;
                }

                service.Roles.Remove(role);
                await service.SaveChangesAsync();
                await ReplyAsync($"Succesfully removed role {role.Name}!");
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("roles")]
        public async Task Roles()
        {
            StringBuilder sb = new();
            sb.AppendLine("Available roles are:");
            foreach (Role role in service.Roles)
            {
                sb.AppendLine(role.Name);
            }
            await ReplyAsync(sb.ToString());
        }

        [Command("role")]
        public async Task Role(string roleName)
        {
            User? user = service.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == Context.User.Id);

            if (user == null)
            {
                await ReplyAsync("Please register with !register");
                return;
            }

            Role? role = service.Roles.Include(r => r.Users).FirstOrDefault(r => r.Name == roleName);

            if (role == null)
            {
                await ReplyAsync($"Role {roleName} does not exists");
                return;
            }

            IRole? drole = Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName);

            if (drole == null)
            {
                drole = await Context.Guild.CreateRoleAsync(roleName);
            }

            IGuildUser guildUser = Context.Guild.GetUser(Context.User.Id);

            if (user.Roles.Any(x => x.Name == roleName))
            {
                await guildUser.RemoveRoleAsync(drole);

                user.Roles.Remove(role);
                role.Users.Remove(user);

                service.Users.Update(user);
                service.Roles.Update(role);

                await service.SaveChangesAsync();

                await ReplyAsync($"Removed role **{roleName}** from you");
            }
            else
            {
                await guildUser.AddRoleAsync(drole);

                user.Roles.Add(role);
                role.Users.Add(user);

                service.Users.Update(user);
                service.Roles.Update(role);

                await service.SaveChangesAsync();

                await ReplyAsync($"Added role **{roleName}** to you");
            }
        }
    }
}