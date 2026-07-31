using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

public partial class App : Application
{
    public static PlaylistViewModel PlaylistViewModel { get; } = new();

    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}