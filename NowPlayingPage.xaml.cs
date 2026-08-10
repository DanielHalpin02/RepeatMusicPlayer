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
}