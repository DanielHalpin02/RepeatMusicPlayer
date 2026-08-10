using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RepeatMusicPlayer.Models;
using RepeatMusicPlayer.Services;

namespace RepeatMusicPlayer.ViewModels;

public class LibraryViewModel : INotifyPropertyChanged
{
    private readonly LibraryService _libraryService;

    public ObservableCollection<Song> Songs { get; set; } = new();

    private Song _selectedSong;
    public Song SelectedSong
    {
        get => _selectedSong;
        set
        {
            _selectedSong = value;
            OnPropertyChanged();
        }
    }

    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            RefreshSortedSongs();
        }
    }

    public LibraryViewModel()
    {
        _libraryService = App.LibraryService;
        RefreshSortedSongs();
    }

    public void RefreshSortedSongs()
    {
        Songs.Clear();

        IEnumerable<Song> filtered = _libraryService.Songs;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var search = SearchText.Trim();
            filtered = filtered.Where(s =>
                (s.Title != null && s.Title.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                (s.Artist != null && s.Artist.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        var sortOrder = App.SettingsService.DefaultSortOrder;
        IEnumerable<Song> sorted = sortOrder switch
        {
            "Artist" => filtered.OrderBy(s => s.Artist),
            "Album" => filtered.OrderBy(s => s.Album),
            _ => filtered.OrderBy(s => s.Title),
        };

        foreach (var song in sorted)
            Songs.Add(song);
    }

    public async Task PickSongAsync()
    {
        var song = await _libraryService.PickAndAddSongAsync();
        if (song != null)
            RefreshSortedSongs();
    }

    public async Task ScanFolderAsync(string folderPath)
    {
        var newSongs = await _libraryService.ScanFolderAsync(folderPath);
        if (newSongs.Count > 0)
            RefreshSortedSongs();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}