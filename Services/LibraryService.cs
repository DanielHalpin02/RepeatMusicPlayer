using RepeatMusicPlayer.Models;

namespace RepeatMusicPlayer.Services;

public class LibraryService
{
    public List<Song> GetSongs()
    {
        return new List<Song>
        {
            new Song { Title = "Test Track One", Artist = "Artist A", Album = "Demo Album", Duration = TimeSpan.FromMinutes(3.2) },
            new Song { Title = "Test Track Two", Artist = "Artist B", Album = "Demo Album", Duration = TimeSpan.FromMinutes(4.1) },
            new Song { Title = "Test Track Three", Artist = "Artist C", Album = "Another Album", Duration = TimeSpan.FromMinutes(2.5) },
            new Song { Title = "Midnight Drive", Artist = "Artist D", Album = "Night Sessions", Duration = TimeSpan.FromMinutes(3.8) },
            new Song { Title = "Coastal Echoes", Artist = "Artist E", Album = "Night Sessions", Duration = TimeSpan.FromMinutes(4.4) },
            new Song { Title = "Static Bloom", Artist = "Artist F", Album = "Another Album", Duration = TimeSpan.FromMinutes(2.9) },
            new Song { Title = "Glass Horizon", Artist = "Artist G", Album = "Demo Album", Duration = TimeSpan.FromMinutes(3.5) },
        };
    }
}