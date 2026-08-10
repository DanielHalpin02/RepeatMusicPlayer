namespace RepeatMusicPlayer;

public partial class NowPlayingPage : ContentPage
{
    public NowPlayingPage()
    {
        InitializeComponent();
        BindingContext = App.NowPlayingViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        PlayPauseButton.Text = App.NowPlayingViewModel.IsPlaying ? "Pause" : "Play";
    }

    private void OnPlayPauseClicked(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.PlayPause();
        PlayPauseButton.Text = App.NowPlayingViewModel.IsPlaying ? "Pause" : "Play";
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.SkipNext();
        PlayPauseButton.Text = "Play";
    }

    private void OnPreviousClicked(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.SkipPrevious();
        PlayPauseButton.Text = "Play";
    }

    private void OnSeekStarted(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.PauseTimerForSeeking();
    }

    private void OnSeekCompleted(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.SeekTo(SeekSlider.Value);
    }
}