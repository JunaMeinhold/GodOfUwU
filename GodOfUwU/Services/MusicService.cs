namespace GodOfUwU.Services;

using Discord;
using Discord.WebSocket;
using GodOfUwU.Core;
using Victoria;
using Victoria.Enums;
using Victoria.Responses.Search;

public class MusicService
{
    private readonly LavaNode _lavaClient;
    private readonly DiscordSocketClient _client;

    public MusicService(LavaNode lavaRestClient, DiscordSocketClient client)
    {
        _client = client;
        _lavaClient = lavaRestClient;
    }

    public bool HasPlayer(IGuild guild)
    {
        if (!_lavaClient.TryGetPlayer(guild, out var player)) return false;
        return player.IsConnected;
    }

    public Task InitializeAsync()
    {
        _client.Ready += ClientReadyAsync;
        _lavaClient.OnLog += PluginLoader.PostLogMessage;
        _lavaClient.OnTrackEnded += OnTrackEnded;
        return Task.CompletedTask;
    }

    public async Task ConnectAsync(SocketVoiceChannel voiceChannel, ITextChannel textChannel)
    {
        await _lavaClient.JoinAsync(voiceChannel, textChannel);
    }

    public async Task LeaveAsync(SocketVoiceChannel voiceChannel)
    {
        await _lavaClient.LeaveAsync(voiceChannel);
    }

    public async Task<string> PlayAsync(string query, ulong guildId)
    {
        var _player = _lavaClient.GetPlayer(_client.GetGuild(guildId));
        var results = await _lavaClient.SearchAsync(SearchType.Direct, query);

        if (results.Status == SearchStatus.NoMatches || results.Status == SearchStatus.LoadFailed)
        {
            return "No matches found.";
        }

        if (results.Status == SearchStatus.TrackLoaded)
        {
            var track = results.Tracks.FirstOrDefault();
            if (track == null) return "No matches found.";

            if (_player.PlayerState == PlayerState.Playing)
            {
                _player.Queue.Enqueue(track);
                return $"{track.Title} has been added to the queue.";
            }
            else
            {
                await _player.PlayAsync(track);
                return $"Now Playing: {track.Title}";
            }
        }
        else if (results.Status == SearchStatus.PlaylistLoaded)
        {
            var tracks = results.Tracks;
            if (tracks.Count == 0) return "No matches found.";

            if (_player.PlayerState == PlayerState.Playing)
            {
                _player.Queue.Enqueue(tracks);
                return $"{tracks.Count} Tracks has been added to the queue.";
            }
            else
            {
                var track = tracks.First();
                await _player.PlayAsync(track);
                return $"Now Playing: {track.Title}";
            }
        }

        return "";
    }

    public async Task<string> StopAsync(ulong guildId)
    {
        var _player = _lavaClient.GetPlayer(_client.GetGuild(guildId));
        if (_player is null)
            return "Error with Player";
        await _player.StopAsync();
        return "Music Playback Stopped.";
    }

    public async Task<string> SkipAsync(ulong guildId)
    {
        var _player = _lavaClient.GetPlayer(_client.GetGuild(guildId));
        if (_player is null || _player.Queue.Count is 0)
            return "Nothing in queue.";

        var oldTrack = _player.Track;
        await _player.SkipAsync();

        return $"Skiped: {oldTrack.Title} \nNow Playing: {_player.Track.Title}";
    }

    public async Task<string> SetVolumeAsync(int vol, ulong guildId)
    {
        var _player = _lavaClient.GetPlayer(_client.GetGuild(guildId));
        if (_player is null)
            return "Player isn't playing.";

        if (vol > 150 || vol <= 2)
        {
            return "Please use a number between 2 - 150";
        }

        await _player.UpdateVolumeAsync((ushort)vol);
        return $"Volume set to: {vol}";
    }

    public async Task<string> PauseOrResumeAsync(ulong guildId)
    {
        var _player = _lavaClient.GetPlayer(_client.GetGuild(guildId));
        if (_player is null)
            return "Player isn't playing.";

        await _player.PauseAsync();
        return "Player is Paused.";
    }

    public async Task<string> ResumeAsync(ulong guildId)
    {
        var _player = _lavaClient.GetPlayer(_client.GetGuild(guildId));
        if (_player is null)
            return "Player isn't playing.";

        await _player.ResumeAsync();
        return "Playback resumed.";
    }

    private async Task ClientReadyAsync()
    {
        await _lavaClient.ConnectAsync();
    }

    private async Task OnTrackEnded(Victoria.EventArgs.TrackEndedEventArgs arg)
    {
        var player = arg.Player;
        var reason = arg.Reason;
        if (reason != TrackEndReason.Finished)
            return;

        if (!player.Queue.TryDequeue(out var item) || item is not LavaTrack nextTrack)
        {
            await player.TextChannel.SendMessageAsync("There are no more tracks in the queue.");
            return;
        }

        await player.PlayAsync(nextTrack);
    }
}