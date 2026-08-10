using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RepeatMusicPlayer.Models;
using RepeatMusicPlayer.Services;

namespace RepeatMusicPlayer.ViewModels;

public class PlaylistViewModel : INotifyPropertyChanged
{
    private readonly PersistenceService _persistenceService;

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

    public PlaylistViewModel()
    {
        _persistenceService = App.PersistenceService;

        foreach (var playlist in _persistenceService.LoadPlaylists())
            Playlists.Add(playlist);
    }

    public void AddPlaylist()
    {
        if (string.IsNullOrWhiteSpace(NewPlaylistName))
            return;

        Playlists.Add(new Playlist { Name = NewPlaylistName });
        NewPlaylistName = string.Empty;
        SavePlaylists();
    }

    public void DeletePlaylist(Playlist playlist)
    {
        if (playlist == null)
            return;

        Playlists.Remove(playlist);
        SavePlaylists();
    }

    public void AddSongToSelectedPlaylist(Song song)
    {
        if (SelectedPlaylist == null || song == null)
            return;

        if (!SelectedPlaylist.Songs.Contains(song))
        {
            SelectedPlaylist.Songs.Add(song);
            SavePlaylists();
        }
    }

    public void RemoveSongFromSelectedPlaylist(Song song)
    {
        if (SelectedPlaylist == null || song == null)
            return;

        SelectedPlaylist.Songs.Remove(song);
        SavePlaylists();
    }

    public void SavePlaylists()
    {
        _persistenceService.SavePlaylists(Playlists.ToList());
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}