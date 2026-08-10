using Plugin.Maui.Audio;
using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

public partial class NowPlayingPage : ContentPage
{
    private readonly NowPlayingViewModel _viewModel;

    public NowPlayingPage(IAudioManager audioManager)
    {
        InitializeComponent();
        _viewModel = new NowPlayingViewModel(audioManager);
        BindingContext = _viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.StopAndDispose();
    }

    private void OnPlayPauseClicked(object sender, EventArgs e)
    {
        _viewModel.PlayPause();
        PlayPauseButton.Text = _viewModel.IsPlaying ? "Pause" : "Play";
    }
}