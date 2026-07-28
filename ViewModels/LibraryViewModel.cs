using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RepeatMusicPlayer.Models;
using RepeatMusicPlayer.Services;

namespace RepeatMusicPlayer.ViewModels;

public class LibraryViewModel : INotifyPropertyChanged
{
    private readonly LibraryService _libraryService = new();

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
        var songs = _libraryService.GetSongs();
        foreach (var song in songs)
            Songs.Add(song);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}