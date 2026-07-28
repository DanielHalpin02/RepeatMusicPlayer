using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

public partial class NowPlayingPage : ContentPage
{
    private readonly NowPlayingViewModel _viewModel;

    public NowPlayingPage()
    {
        InitializeComponent();
        _viewModel = new NowPlayingViewModel();
        BindingContext = _viewModel;
    }

    private void OnPlayPauseClicked(object sender, EventArgs e)
    {
        _viewModel.PlayPause();
        PlayPauseButton.Text = _viewModel.IsPlaying ? "Pause" : "Play";
    }
}