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

    public LibraryViewModel()
    {
        _libraryService = App.LibraryService;

        foreach (var song in _libraryService.Songs)
            Songs.Add(song);
    }

    public async Task PickSongAsync()
    {
        var song = await _libraryService.PickAndAddSongAsync();
        if (song != null)
            Songs.Add(song);
    }

    public async Task ScanFolderAsync(string folderPath)
    {
        var newSongs = await _libraryService.ScanFolderAsync(folderPath);
        foreach (var song in newSongs)
            Songs.Add(song);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}