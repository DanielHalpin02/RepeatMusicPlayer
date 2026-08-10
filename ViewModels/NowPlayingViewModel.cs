using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugin.Maui.Audio;
using RepeatMusicPlayer.Models;

namespace RepeatMusicPlayer.ViewModels;

public class NowPlayingViewModel : INotifyPropertyChanged
{
    private readonly IAudioManager _audioManager;
    private IAudioPlayer _audioPlayer;
    private IDispatcherTimer _positionTimer;

    public List<Song> PlaybackQueue { get; set; } = new();

    private Song _currentSong;
    public Song CurrentSong
    {
        get => _currentSong;
        set
        {
            _currentSong = value;
            OnPropertyChanged();
            LoadSong();
        }
    }

    private bool _isPlaying;
    public bool IsPlaying
    {
        get => _isPlaying;
        set
        {
            _isPlaying = value;
            OnPropertyChanged();
        }
    }

    private double _position;
    public double Position
    {
        get => _position;
        set
        {
            _position = value;
            OnPropertyChanged();
        }
    }

    private double _duration;
    public double Duration
    {
        get => _duration;
        set
        {
            _duration = value;
            OnPropertyChanged();
        }
    }

    public NowPlayingViewModel(IAudioManager audioManager)
    {
        _audioManager = audioManager;

        _positionTimer = Application.Current.Dispatcher.CreateTimer();
        _positionTimer.Interval = TimeSpan.FromMilliseconds(500);
        _positionTimer.Tick += (s, e) => UpdatePosition();
        _positionTimer.Start();
    }

    private void LoadSong()
    {
        if (CurrentSong == null || string.IsNullOrEmpty(CurrentSong.FilePath))
            return;

        StopAndDispose();

        if (File.Exists(CurrentSong.FilePath))
        {
            _audioPlayer = _audioManager.CreatePlayer(File.OpenRead(CurrentSong.FilePath));
            IsPlaying = false;
            Position = 0;
            Duration = _audioPlayer.Duration;
        }
    }

    public void PlayPause()
    {
        if (_audioPlayer == null)
            return;

        if (IsPlaying)
        {
            _audioPlayer.Pause();
        }
        else
        {
            _audioPlayer.Play();
        }

        IsPlaying = !IsPlaying;
    }

    private bool _isSeeking;

    public void PauseTimerForSeeking()
    {
        _isSeeking = true;
    }

    public void SeekTo(double seconds)
    {
        _audioPlayer?.Seek(seconds);
        Position = seconds;
        _isSeeking = false;
    }

    public void SkipNext()
    {
        SkipBy(1);
    }

    public void SkipPrevious()
    {
        SkipBy(-1);
    }

    private void SkipBy(int direction)
    {
        if (PlaybackQueue == null || PlaybackQueue.Count == 0 || CurrentSong == null)
            return;

        var currentIndex = PlaybackQueue.IndexOf(CurrentSong);
        if (currentIndex == -1)
            return;

        var newIndex = currentIndex + direction;
        if (newIndex < 0 || newIndex >= PlaybackQueue.Count)
            return;

        CurrentSong = PlaybackQueue[newIndex];
    }

    private void UpdatePosition()
    {
        if (_audioPlayer == null)
            return;

        // Keep catching up Duration in case it wasn't ready when the player was first created.
        if (Duration <= 0 && _audioPlayer.Duration > 0)
            Duration = _audioPlayer.Duration;

        if (IsPlaying && !_isSeeking)
        {
            Position = _audioPlayer.CurrentPosition;
        }
    }

    public void StopAndDispose()
    {
        if (_audioPlayer != null)
        {
            if (_audioPlayer.IsPlaying)
                _audioPlayer.Stop();

            _audioPlayer.Dispose();
            _audioPlayer = null;
        }

        IsPlaying = false;
        Position = 0;
        Duration = 0;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}