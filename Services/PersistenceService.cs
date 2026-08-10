using System.Text.Json;
using RepeatMusicPlayer.Models;

namespace RepeatMusicPlayer.Services;

public class PersistenceService
{
    private readonly string _libraryFilePath;
    private readonly string _playlistsFilePath;

    public PersistenceService()
    {
        var folder = FileSystem.AppDataDirectory;
        _libraryFilePath = Path.Combine(folder, "library.json");
        _playlistsFilePath = Path.Combine(folder, "playlists.json");
    }

    public void SaveLibrary(List<Song> songs)
    {
        var json = JsonSerializer.Serialize(songs);
        File.WriteAllText(_libraryFilePath, json);
    }

    public List<Song> LoadLibrary()
    {
        if (!File.Exists(_libraryFilePath))
            return new List<Song>();

        var json = File.ReadAllText(_libraryFilePath);
        return JsonSerializer.Deserialize<List<Song>>(json) ?? new List<Song>();
    }

    public void SavePlaylists(List<Playlist> playlists)
    {
        var json = JsonSerializer.Serialize(playlists);
        File.WriteAllText(_playlistsFilePath, json);
    }

    public List<Playlist> LoadPlaylists()
    {
        if (!File.Exists(_playlistsFilePath))
            return new List<Playlist>();

        var json = File.ReadAllText(_playlistsFilePath);
        return JsonSerializer.Deserialize<List<Playlist>>(json) ?? new List<Playlist>();
    }
}