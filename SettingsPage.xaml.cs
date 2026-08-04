namespace RepeatMusicPlayer;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        DarkModeSwitch.IsToggled = App.SettingsService.IsDarkMode;
        SortOrderPicker.SelectedItem = App.SettingsService.DefaultSortOrder;

        ApplyTheme(App.SettingsService.IsDarkMode);
    }

    private void OnDarkModeToggled(object sender, ToggledEventArgs e)
    {
        App.SettingsService.IsDarkMode = e.Value;
        ApplyTheme(e.Value);
    }

    private void OnSortOrderChanged(object sender, EventArgs e)
    {
        if (SortOrderPicker.SelectedItem is string sortOrder)
        {
            App.SettingsService.DefaultSortOrder = sortOrder;
        }
    }

    private void ApplyTheme(bool isDark)
    {
        Application.Current.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;
    }
}