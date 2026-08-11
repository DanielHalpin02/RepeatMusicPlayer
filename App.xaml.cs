using Plugin.Maui.Audio;
using RepeatMusicPlayer.Services;
using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

public partial class App : Application
{
    public static PersistenceService PersistenceService { get; } = new();
    public static SettingsService SettingsService { get; } = new();
    public static AlbumArtService AlbumArtService { get; } = new();
    public static PlaylistViewModel PlaylistViewModel { get; } = new();
    public static LibraryService LibraryService { get; } = new();
    public static NowPlayingViewModel NowPlayingViewModel { get; private set; }

    public App(IAudioManager audioManager)
    {
        InitializeComponent();
        NowPlayingViewModel = new NowPlayingViewModel(audioManager);
        MainPage = new AppShell();
    }
}