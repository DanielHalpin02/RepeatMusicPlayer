using Microsoft.Maui.Controls;
using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

public partial class PlaylistPage : ContentPage
{
    private readonly PlaylistViewModel _viewModel;

    public PlaylistPage()
    {
        InitializeComponent();
        _viewModel = new PlaylistViewModel();
        BindingContext = _viewModel;
    }

    private void OnAddPlaylistClicked(object sender, EventArgs e)
    {
        _viewModel.AddPlaylist();
    }
}