namespace GodOfUwU.Admin.Modules
{
    using Discord;
    using Discord.Commands;
    using GodOfUwU.Core;
    using GodOfUwU.Core.Entities;
    using GodOfUwU.Core.Entities.Attributes;
    using System.Text;

    [PermissionNamespace(typeof(UserModule), "users")]
    public class UserModule : ModuleBase<SocketCommandContext>
    {
        [Command("user-list")]
        public async Task ListUserAsync()
        {
            if (UserContext.CheckPermission(Context.User, typeof(UserModule)))
            {
                StringBuilder sb = new();
                sb.AppendLine("Users:");
                foreach (User user in UserContext.Current.Users)
                {
                    sb.AppendLine($"{Context.Client.GetUser(user.Id)}, {string.Join("; ", user.Groups)}");
                }
                await ReplyAsync(sb.ToString());
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("user-info")]
        public async Task UserInfoAsync(IUser duser)
        {
            if (UserContext.CheckPermission(Context.User, typeof(UserModule)))
            {
                User? user = UserContext.Current.Users.FirstOrDefault(u => u.Id == duser.Id);
                if (user == null)
                {
                    await ReplyAsync($"User {duser} not found");
                    return;
                }

                StringBuilder sb = new();
                sb.AppendLine(Context.Client.GetUser(user.Id).ToString());
                sb.AppendLine($"Groups: \n{string.Join("\n", user.Groups.Select(x => x.Name))}");

                await ReplyAsync(sb.ToString());
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("user-add")]
        public async Task AddUserAsync(IUser duser)
        {
            if (UserContext.CheckPermission(Context.User, typeof(UserModule)))
            {
                if (UserContext.Current.Users.Any(u => u.Id == duser.Id))
                {
                    await ReplyAsync($"User {duser} is already in the database");
                    return;
                }

                User user = new() { Id = duser.Id };

                UserContext.Current.Users.Add(user);

                await UserContext.Current.SaveChangesAsync();

                Group group = UserContext.Current.Roles.First(g => g.Name == Group.DefaultGroup);

                user.Groups.Add(group);
                group.Users.Add(user);

                UserContext.Current.Users.Update(user);
                UserContext.Current.Roles.Update(group);

                await UserContext.Current.SaveChangesAsync();

                await ReplyAsync($"User {duser} has been added successfully");
            }
            else
            {
                await ReplyAsync("No");
            }
        }

        [Command("user-remove")]
        public async Task DeleteUserAsync(IUser duser)
        {
            if (UserContext.CheckPermission(Context.User, typeof(UserModule)))
            {
                User? user = UserContext.Current.Users.FirstOrDefault(u => u.Id == duser.Id);
                if (user == null)
                {
                    await ReplyAsync($"User {duser} not found");
                    return;
                }

                UserContext.Current.Users.Remove(user);
                await UserContext.Current.SaveChangesAsync();
                await ReplyAsync($"User {duser} has been deleted successfully");
            }
            else
            {
                await ReplyAsync("No");
            }
        }
    }
}