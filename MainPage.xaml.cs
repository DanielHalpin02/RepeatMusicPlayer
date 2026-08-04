using CommunityToolkit.Maui.Storage;
using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

public partial class MainPage : ContentPage
{
    private readonly LibraryViewModel _viewModel;

    public MainPage()
    {
        InitializeComponent();
        _viewModel = new LibraryViewModel();
        BindingContext = _viewModel;
    }

    private async void OnPlaylistsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PlaylistPage));
    }

    private async void OnAddSongClicked(object sender, EventArgs e)
    {
        await _viewModel.PickSongAsync();
    }

    private async void OnScanFolderClicked(object sender, EventArgs e)
    {
        var folder = await FolderPicker.Default.PickAsync();
        if (folder.IsSuccessful)
        {
            await _viewModel.ScanFolderAsync(folder.Folder.Path);
        }
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
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