using System.Collections.ObjectModel;

namespace RepeatMusicPlayer.Models;

public class Playlist
{
    public string Name { get; set; }
    public ObservableCollection<Song> Songs { get; set; } = new();
}