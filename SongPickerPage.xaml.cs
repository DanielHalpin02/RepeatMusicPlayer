using RepeatMusicPlayer.Models;

namespace RepeatMusicPlayer;

public partial class SongPickerPage : ContentPage
{
    public List<Song> Songs { get; set; }

    public SongPickerPage()
    {
        InitializeComponent();
        Songs = App.LibraryService.Songs;
        BindingContext = this;
    }

    private async void OnAddSelectedClicked(object sender, EventArgs e)
    {
        var selectedSongs = SongsPickerCollectionView.SelectedItems;

        foreach (var item in selectedSongs)
        {
            if (item is Song song)
            {
                App.PlaylistViewModel.AddSongToSelectedPlaylist(song);
            }
        }

        await Shell.Current.GoToAsync("..");
    }
}