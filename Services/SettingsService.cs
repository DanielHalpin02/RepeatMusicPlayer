namespace RepeatMusicPlayer.Services;

public class SettingsService
{
    public bool IsDarkMode
    {
        get => Preferences.Default.Get("IsDarkMode", false);
        set => Preferences.Default.Set("IsDarkMode", value);
    }

    public string DefaultSortOrder
    {
        get => Preferences.Default.Get("DefaultSortOrder", "Title");
        set => Preferences.Default.Set("DefaultSortOrder", value);
    }
}