using RepeatMusicPlayer;

namespace RepeatMusicPlayer;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(NowPlayingPage), typeof(NowPlayingPage));
    }
}