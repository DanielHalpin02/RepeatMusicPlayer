using Microsoft.Maui.Controls;
using RepeatMusicPlayer;

namespace RepeatMusicPlayer;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(NowPlayingPage), typeof(NowPlayingPage));
        Routing.RegisterRoute(nameof(PlaylistPage), typeof(PlaylistPage));
        Routing.RegisterRoute(nameof(PlaylistDetailPage), typeof(PlaylistDetailPage));
        Routing.RegisterRoute(nameof(SongPickerPage), typeof(SongPickerPage));
    }
}