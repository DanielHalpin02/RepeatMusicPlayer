using RepeatMusicPlayer.Services;
using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

public partial class App : Application
{
    public static PersistenceService PersistenceService { get; } = new();
    public static SettingsService SettingsService { get; } = new();
    public static PlaylistViewModel PlaylistViewModel { get; } = new();
    public static LibraryService LibraryService { get; } = new();

    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}