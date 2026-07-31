using Microsoft.Maui.Controls;
using RepeatMusicPlayer.Models;
using RepeatMusicPlayer.Services;
using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

public partial class SongPickerPage : ContentPage
{
    private readonly LibraryService _libraryService = new();

    public List<Song> Songs { get; set; }

    public SongPickerPage()
    {
        InitializeComponent();
        Songs = _libraryService.GetSongs();
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