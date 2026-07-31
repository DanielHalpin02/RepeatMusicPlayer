using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

public partial class PlaylistPage : ContentPage
{
    private readonly PlaylistViewModel _viewModel;

    public PlaylistPage()
    {
        InitializeComponent();
        _viewModel = App.PlaylistViewModel;
        BindingContext = _viewModel;
    }

    private void OnAddPlaylistClicked(object sender, EventArgs e)
    {
        _viewModel.AddPlaylist();
    }

    private async void OnPlaylistSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Models.Playlist selectedPlaylist)
        {
            _viewModel.SelectedPlaylist = selectedPlaylist;
            await Shell.Current.GoToAsync(nameof(PlaylistDetailPage), new Dictionary<string, object>
            {
                { "ViewModel", _viewModel }
            });
        }
    }
}