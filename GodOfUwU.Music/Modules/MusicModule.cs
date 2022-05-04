using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using GodOfUwU.Core.Entities.Attributes;
using GodOfUwU.Services;

namespace GodOfUwU.Modules
{
    [PermissionNamespace(typeof(MusicModule), "music")]
    public class MusicModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly MusicService _musicService;

        public MusicModule(MusicService musicService)
        {
            _musicService = musicService;
        }

        [SlashCommand("join", "Makes the bot join your current channel")]
        public async Task Join()
        {
            if (Context.User is not SocketGuildUser user) return;
            if (user.VoiceChannel is null)
            {
                await Context.Interaction.RespondAsync("You need to connect to a voice channel.");
                return;
            }
            else
            {
                if (Context.Channel is not ITextChannel channel) return;
                await _musicService.ConnectAsync(user.VoiceChannel, channel);
                await Context.Interaction.RespondAsync($"now connected to {user.VoiceChannel.Name}");
            }
        }

        [SlashCommand("leave", "Leaves the current channel")]
        public async Task Leave()
        {
            if (Context.User is not SocketGuildUser user) return;
            if (user.VoiceChannel is null)
            {
                await Context.Interaction.RespondAsync("Please join the channel the bot is in to make it leave.");
            }
            else
            {
                await _musicService.LeaveAsync(user.VoiceChannel);
                await Context.Interaction.RespondAsync($"Bot has now left {user.VoiceChannel.Name}");
            }
        }

        [SlashCommand("play", "Plays the given url")]
        public async Task Play([Remainder] string query)
        {
            if (Context.User is not SocketGuildUser user) return;
            if (user.VoiceChannel is null)
            {
                await Context.Interaction.RespondAsync("You need to connect to a voice channel.");
                return;
            }
            else
            {
                if (Context.Channel is not ITextChannel channel) return;

                if (!_musicService.HasPlayer(Context.Guild))
                    await _musicService.ConnectAsync(user.VoiceChannel, channel);
                await Context.Interaction.RespondAsync(await _musicService.PlayAsync(query, Context.Guild.Id));
            }
        }

        [SlashCommand("stop", "Stops the music bot")]
        public async Task Stop()
        {
            if (Context.User is not SocketGuildUser user) return;
            if (user.VoiceChannel is null)
            {
                await Context.Interaction.RespondAsync("You need to connect to a voice channel.");
                return;
            }
            else
            {
                if (_musicService.HasPlayer(Context.Guild))
                {
                    await _musicService.StopAsync(Context.Guild.Id);
                    await _musicService.LeaveAsync(user.VoiceChannel);
                    await Context.Interaction.RespondAsync($"Bot has now left {user.VoiceChannel.Name}");
                }
                else
                    await Context.Interaction.RespondAsync("The bot is not connected to any server.");
            }
        }

        [SlashCommand("skip", "Skips the current song")]
        public async Task Skip()
        {
            if (Context.User is not SocketGuildUser user) return;
            if (user.VoiceChannel is null)
            {
                await Context.Interaction.RespondAsync("You need to connect to a voice channel.");
                return;
            }
            else
            {
                if (_musicService.HasPlayer(Context.Guild))
                    await Context.Interaction.RespondAsync(await _musicService.SkipAsync(Context.Guild.Id));
                else
                    await Context.Interaction.RespondAsync("The bot is not connected to any server.");
            }
        }

        [SlashCommand("volume", "Sets the volume of the music bot")]
        public async Task Volume(int vol)
        {
            if (Context.User is not SocketGuildUser user) return;
            if (user.VoiceChannel is null)
            {
                await Context.Interaction.RespondAsync("You need to connect to a voice channel.");
                return;
            }
            else
            {
                if (_musicService.HasPlayer(Context.Guild))
                    await Context.Interaction.RespondAsync(await _musicService.SetVolumeAsync(vol, Context.Guild.Id));
                else
                    await Context.Interaction.RespondAsync("The bot is not connected to any server.");
            }
        }

        [SlashCommand("pause", "Pauses the queue")]
        public async Task Pause()
        {
            if (Context.User is not SocketGuildUser user) return;
            if (user.VoiceChannel is null)
            {
                await Context.Interaction.RespondAsync("You need to connect to a voice channel.");
                return;
            }
            else
            {
                if (_musicService.HasPlayer(Context.Guild))
                    await Context.Interaction.RespondAsync(await _musicService.PauseOrResumeAsync(Context.Guild.Id));
                else
                    await Context.Interaction.RespondAsync("The bot is not connected to any server.");
            }
        }

        [SlashCommand("resume", "Resumes the queue")]
        public async Task Resume()
        {
            if (Context.User is not SocketGuildUser user) return;
            if (user.VoiceChannel is null)
            {
                await Context.Interaction.RespondAsync("You need to connect to a voice channel.");
                return;
            }
            else
            {
                if (_musicService.HasPlayer(Context.Guild))
                    await Context.Interaction.RespondAsync(await _musicService.ResumeAsync(Context.Guild.Id));
                else
                    await Context.Interaction.RespondAsync("The bot is not connected to any server.");
            }
        }
    }
}