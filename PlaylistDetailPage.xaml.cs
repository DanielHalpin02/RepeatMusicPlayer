using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

[QueryProperty(nameof(ViewModel), "ViewModel")]
public partial class PlaylistDetailPage : ContentPage
{
    public PlaylistViewModel ViewModel
    {
        set => BindingContext = value;
    }

    public PlaylistDetailPage()
    {
        InitializeComponent();
    }

    private async void OnAddSongsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SongPickerPage));
    }

    private void OnRemoveSongClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Models.Song song)
        {
            App.PlaylistViewModel.RemoveSongFromSelectedPlaylist(song);
        }
    }

    private async void OnSongSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Models.Song selectedSong)
        {
            App.NowPlayingViewModel.PlaybackQueue = App.PlaylistViewModel.SelectedPlaylist.Songs.ToList();
            App.NowPlayingViewModel.CurrentSong = selectedSong;
            await Shell.Current.GoToAsync(nameof(NowPlayingPage));
        }
    }
}