using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RepeatMusicPlayer.Models;

namespace RepeatMusicPlayer.ViewModels;

public class PlaylistViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Playlist> Playlists { get; set; } = new();

    private string _newPlaylistName;
    public string NewPlaylistName
    {
        get => _newPlaylistName;
        set
        {
            _newPlaylistName = value;
            OnPropertyChanged();
        }
    }

    private Playlist _selectedPlaylist;
    public Playlist SelectedPlaylist
    {
        get => _selectedPlaylist;
        set
        {
            _selectedPlaylist = value;
            OnPropertyChanged();
        }
    }

    public void AddPlaylist()
    {
        if (string.IsNullOrWhiteSpace(NewPlaylistName))
            return;

        Playlists.Add(new Playlist { Name = NewPlaylistName });
        NewPlaylistName = string.Empty;
    }

    public void AddSongToSelectedPlaylist(Song song)
    {
        if (SelectedPlaylist == null || song == null)
            return;

        if (!SelectedPlaylist.Songs.Contains(song))
            SelectedPlaylist.Songs.Add(song);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}