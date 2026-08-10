using Id3;
using RepeatMusicPlayer.Models;

namespace RepeatMusicPlayer.Services;

public class LibraryService
{
    private readonly PersistenceService _persistenceService;

    public List<Song> Songs { get; set; }

    private static readonly FilePickerFileType AudioFileType = new(
        new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".ogg", ".flac", ".m4a" } },
            { DevicePlatform.Android, new[] { "audio/*" } }
        });

    public LibraryService()
    {
        _persistenceService = App.PersistenceService;
        Songs = _persistenceService.LoadLibrary();
    }

    public async Task<Song> PickAndAddSongAsync()
    {
        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select an audio file",
            FileTypes = AudioFileType
        });

        if (result == null)
            return null;

        var song = BuildSongFromFile(result.FullPath);
        Songs.Add(song);
        SaveLibrary();
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
            var song = BuildSongFromFile(file);
            Songs.Add(song);
            addedSongs.Add(song);
        }

        SaveLibrary();
        return addedSongs;
    }

    private Song BuildSongFromFile(string filePath)
    {
        var song = new Song
        {
            Title = Path.GetFileNameWithoutExtension(filePath),
            Artist = "Unknown Artist",
            Album = "Unknown Album",
            FilePath = filePath,
            Duration = TimeSpan.Zero
        };

        // Only .mp3 files carry ID3 tags; other formats keep the filename-based fallback above.
        if (Path.GetExtension(filePath).ToLower() == ".mp3")
        {
            try
            {
                using var mp3 = new Mp3(filePath);
                var tag = mp3.GetTag(Id3TagFamily.Version2X) ?? mp3.GetTag(Id3TagFamily.Version1X);

                if (tag != null)
                {
                    if (!string.IsNullOrWhiteSpace(tag.Title))
                        song.Title = tag.Title;

                    if (tag.Artists != null)
                    {
                        var artistString = tag.Artists.ToString();
                        if (!string.IsNullOrWhiteSpace(artistString))
                            song.Artist = artistString;
                    }

                    if (!string.IsNullOrWhiteSpace(tag.Album))
                        song.Album = tag.Album;
                }
            }
            catch
            {
                // If the tag can't be read, we already have filename-based fallbacks above.
            }
        }

        return song;
    }

    public void SaveLibrary()
    {
        _persistenceService.SaveLibrary(Songs);
    }
}