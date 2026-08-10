using RepeatMusicPlayer.ViewModels;

namespace RepeatMusicPlayer;

public partial class NowPlayingPage : ContentPage
{
    public NowPlayingPage()
    {
        InitializeComponent();
        BindingContext = App.NowPlayingViewModel;
        App.NowPlayingViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshButtonLabels();
    }

    private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NowPlayingViewModel.IsPlaying)
            || e.PropertyName == nameof(NowPlayingViewModel.IsShuffleOn)
            || e.PropertyName == nameof(NowPlayingViewModel.RepeatMode))
        {
            MainThread.BeginInvokeOnMainThread(RefreshButtonLabels);
        }
    }

    private void RefreshButtonLabels()
    {
        var vm = App.NowPlayingViewModel;
        PlayPauseButton.Text = vm.IsPlaying ? "Pause" : "Play";
        ShuffleButton.Text = vm.IsShuffleOn ? "Shuffle: On" : "Shuffle: Off";
        RepeatButton.Text = vm.RepeatMode switch
        {
            RepeatMode.RepeatAll => "Repeat: All",
            RepeatMode.RepeatOne => "Repeat: One",
            _ => "Repeat: Off"
        };
    }

    private void OnPlayPauseClicked(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.PlayPause();
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.SkipNext();
    }

    private void OnPreviousClicked(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.SkipPrevious();
    }

    private void OnShuffleClicked(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.ToggleShuffle();
    }

    private void OnRepeatClicked(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.CycleRepeatMode();
    }

    private void OnSeekStarted(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.PauseTimerForSeeking();
    }

    private void OnSeekCompleted(object sender, EventArgs e)
    {
        App.NowPlayingViewModel.SeekTo(SeekSlider.Value);
    }

    private void OnVolumeChanged(object sender, ValueChangedEventArgs e)
    {
        App.NowPlayingViewModel.Volume = e.NewValue;
    }
}