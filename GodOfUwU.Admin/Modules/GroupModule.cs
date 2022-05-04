namespace GodOfUwU.Admin.Modules
{
    using Discord;
    using Discord.Commands;
    using GodOfUwU.Core;
    using GodOfUwU.Core.Entities;
    using GodOfUwU.Core.Entities.Attributes;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [PermissionNamespace(typeof(GroupModule), "groups")]
    public class GroupModule : ModuleBase<SocketCommandContext>
    {
        [Command("group-user-add")]
        public async Task AddUserGroupAsync(string groupname, IUser duser)
        {
            if (UserContext.CheckPermission(Context.User, typeof(GroupModule)))
            {
                User? user = UserContext.Current.Users.Include(u => u.Groups).FirstOrDefault(x => x.Id == duser.Id);

                if (user == null)
                {
                    await ReplyAsync($"User {duser} not found.");
                    return;
                }

                Group? group = UserContext.Current.Groups.Include(g => g.Users).FirstOrDefault(x => x.Name == groupname);

                if (group == null)
                {
                    await ReplyAsync($"Group {groupname} not found.");
                    return;
                }

                if (group.Users.Any(x => x.Id == user.Id))
                {
                    await ReplyAsync($"User {duser} is already in the group {groupname}.");
                    return;
                }

                user.Groups.Add(group);
                group.Users.Add(user);

                UserContext.Current.Users.Update(user);
                UserContext.Current.Groups.Update(group);

                await UserContext.Current.SaveChangesAsync();

                await ReplyAsync($"User {duser} has been successfully added to group {group}");
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("group-user-delete")]
        public async Task DeleteUserGroupAsync(string groupname, IUser duser)
        {
            if (UserContext.CheckPermission(Context.User, typeof(GroupModule)))
            {
                User? user = UserContext.Current.Users.Include(u => u.Groups).FirstOrDefault(x => x.Id == duser.Id);

                if (user == null)
                {
                    await ReplyAsync($"User {duser} does not exist.");
                    return;
                }

                Group? group = UserContext.Current.Groups.Include(g => g.Users).FirstOrDefault(x => x.Name == groupname);

                if (group == null)
                {
                    await ReplyAsync($"Group {groupname} does not exist.");
                    return;
                }

                if (!group.Users.Any(x => x.Id == user.Id))
                {
                    await ReplyAsync($"User {duser} is not in group {groupname}.");
                    return;
                }

                user.Groups.Remove(group);
                group.Users.Remove(user);

                UserContext.Current.Users.Update(user);
                UserContext.Current.Groups.Update(group);

                await UserContext.Current.SaveChangesAsync();

                await ReplyAsync($"User {duser} has been successfully removed from the group {group}");
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("group-list")]
        public async Task ListGroupAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(GroupModule)))
            {
                StringBuilder sb = new();
                sb.AppendLine("Groups:");
                foreach (Group group in UserContext.Current.Groups.Include(g => g.Users).ThenInclude(u => u.Groups))
                {
                    sb.AppendLine($"{group}, with {group.Users.Count} users");
                }
                await ReplyAsync(sb.ToString());
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("group-info")]
        public async Task GroupInfoAsync(string name)
        {
            if (UserContext.CheckPermission(Context.User, typeof(GroupModule)))
            {
                Group? group = UserContext.Current.Groups.Include(g => g.Users).Include(g => g.Permissions).FirstOrDefault(x => x.Name == name);
                if (group == null)
                {
                    await ReplyAsync($"Group {name} does not exists");
                    return;
                }

                StringBuilder sb = new();
                sb.AppendLine("Group info:");

                sb.AppendLine($"{group}");
                sb.AppendLine($"Users: \n{string.Join("\n", group.Users.Select(x => Context.Client.GetUser(x.Id)))}");
                sb.AppendLine($"Permissions: \n {string.Join("\n", group.Permissions.Select(p => p.Name))}");

                await ReplyAsync(sb.ToString());
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("group-add")]
        public async Task AddGroupAsync(string name)
        {
            if (UserContext.CheckPermission(Context.User, typeof(GroupModule)))
            {
                if (UserContext.Current.Groups.Any(x => x.Name == name))
                {
                    await ReplyAsync($"Group {name} already exists");
                    return;
                }

                Group group = new() { Name = name };

                await UserContext.Current.Groups.AddAsync(group);

                await UserContext.Current.SaveChangesAsync();

                await ReplyAsync($"Group {group} has been successfully created");
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("group-delete")]
        public async Task DeleteGroupAsync(string name)
        {
            if (UserContext.CheckPermission(Context.User, typeof(GroupModule)))
            {
                Group? group = UserContext.Current.Groups.FirstOrDefault(x => x.Name == name);
                if (group == null)
                {
                    await ReplyAsync($"Group {name} does not exists");
                    return;
                }

                UserContext.Current.Groups.Remove(group);

                await UserContext.Current.SaveChangesAsync();

                await ReplyAsync($"Group {group} has been successfully deleted");
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("group-perm-add")]
        public async Task AddPermToGroup(string groupname, string permissionname)
        {
            if (UserContext.CheckPermission(Context.User, typeof(GroupModule)))
            {
                Group? group = UserContext.Current.Groups.Include(g => g.Permissions).FirstOrDefault(x => x.Name == groupname);
                if (group == null)
                {
                    await ReplyAsync($"Group {groupname} does not exists");
                    return;
                }

                Permission? permission = UserContext.Current.Permissions.FirstOrDefault(x => x.Name == permissionname);

                if (permission == null)
                {
                    await ReplyAsync($"Permission {permissionname} does not exists");
                    return;
                }

                group.Permissions.Add(permission);

                UserContext.Current.Groups.Update(group);

                await UserContext.Current.SaveChangesAsync();

                await ReplyAsync($"Permission {permissionname} has been successfully added to group {groupname}");
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("group-perm-delete")]
        public async Task DeletePermToGroup(string groupname, string permissionname)
        {
            if (UserContext.CheckPermission(Context.User, typeof(GroupModule)))
            {
                Group? group = UserContext.Current.Groups.Include(g => g.Permissions).FirstOrDefault(x => x.Name == groupname);
                if (group == null)
                {
                    await ReplyAsync($"Group {groupname} does not exists");
                    return;
                }

                Permission? permission = UserContext.Current.Permissions.FirstOrDefault(x => x.Name == permissionname);

                if (permission == null)
                {
                    await ReplyAsync($"Permission {permissionname} does not exists");
                    return;
                }

                if (!group.Permissions.Any(x => x.Name == permissionname))
                {
                    await ReplyAsync($"Permission {permissionname} is not present in group");
                    return;
                }

                group.Permissions.Remove(permission);

                UserContext.Current.Groups.Update(group);

                await UserContext.Current.SaveChangesAsync();

                await ReplyAsync($"Permission {permissionname} has been successfully removed from group {groupname}");
            }
            else
            {
                await ReplyAsync("No");
            }
        }
    }
}