using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new LibraryViewModel();
    }

    private async void OnSongSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Models.Song selectedSong)
        {
            await Shell.Current.GoToAsync(nameof(NowPlayingPage), new Dictionary<string, object>
            {
                { "Song", selectedSong }
            });
        }
    }
}