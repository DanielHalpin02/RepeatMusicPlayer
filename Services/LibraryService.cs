using RepeatMusicPlayer.Models;

namespace RepeatMusicPlayer.Services;

public class LibraryService
{
    public List<Song> Songs { get; set; } = new();

    private static readonly FilePickerFileType AudioFileType = new(
        new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".ogg", ".flac", ".m4a" } },
            { DevicePlatform.Android, new[] { "audio/*" } }
        });

    public async Task<Song> PickAndAddSongAsync()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select an audio file",
            FileTypes = AudioFileType
        });

        if (result == null)
            return null;

        var song = new Song
        {
            Title = Path.GetFileNameWithoutExtension(result.FileName),
            Artist = "Unknown Artist",
            Album = "Unknown Album",
            FilePath = result.FullPath,
            Duration = TimeSpan.Zero
        };

        Songs.Add(song);
        return song;
    }

    public async Task<List<Song>> ScanFolderAsync(string folderPath)
    {
        var addedSongs = new List<Song>();

        if (!Directory.Exists(folderPath))
            return addedSongs;

        var audioExtensions = new[] { ".mp3", ".wav", ".ogg", ".flac", ".m4a" };
        var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                              .Where(f => audioExtensions.Contains(Path.GetExtension(f).ToLower()));

        foreach (var file in files)
        {
            var song = new Song
            {
                Title = Path.GetFileNameWithoutExtension(file),
                Artist = "Unknown Artist",
                Album = "Unknown Album",
                FilePath = file,
                Duration = TimeSpan.Zero
            };

            Songs.Add(song);
            addedSongs.Add(song);
        }

        return addedSongs;
    }
}