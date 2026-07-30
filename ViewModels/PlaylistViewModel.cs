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

    public void AddPlaylist()
    {
        if (string.IsNullOrWhiteSpace(NewPlaylistName))
            return;

        Playlists.Add(new Playlist { Name = NewPlaylistName });
        NewPlaylistName = string.Empty;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}