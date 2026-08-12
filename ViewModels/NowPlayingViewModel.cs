using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugin.Maui.Audio;
using RepeatMusicPlayer.Models;

namespace RepeatMusicPlayer.ViewModels;

public enum RepeatMode
{
    Off,
    RepeatOne,
    RepeatAll
}

public class NowPlayingViewModel : INotifyPropertyChanged
{
    private readonly IAudioManager _audioManager;
    private IAudioPlayer _audioPlayer;
    private IDispatcherTimer _positionTimer;
    private readonly Random _random = new();
    private int _albumArtRequestId;

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
            _ = LoadAlbumArtAsync();
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

    private double _volume = 1.0;
    public double Volume
    {
        get => _volume;
        set
        {
            _volume = value;
            OnPropertyChanged();
            if (_audioPlayer != null)
                _audioPlayer.Volume = value;
        }
    }

    private bool _isShuffleOn;
    public bool IsShuffleOn
    {
        get => _isShuffleOn;
        set
        {
            _isShuffleOn = value;
            OnPropertyChanged();
        }
    }

    private RepeatMode _repeatMode = RepeatMode.Off;
    public RepeatMode RepeatMode
    {
        get => _repeatMode;
        set
        {
            _repeatMode = value;
            OnPropertyChanged();
        }
    }

    private string _albumArtUrl;
    public string AlbumArtUrl
    {
        get => _albumArtUrl;
        set
        {
            _albumArtUrl = value;
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

    private async Task LoadAlbumArtAsync()
    {
        AlbumArtUrl = null;

        if (CurrentSong == null)
            return;

        var requestId = ++_albumArtRequestId;
        var url = await App.AlbumArtService.GetAlbumArtUrlAsync(CurrentSong.Artist, CurrentSong.Album);

        // Ignore the result if the user has already moved on to a different song by the time this returns.
        if (requestId == _albumArtRequestId)
        {
            AlbumArtUrl = url;
        }
    }

    private void LoadSong()
    {
        if (CurrentSong == null || string.IsNullOrEmpty(CurrentSong.FilePath))
            return;

        StopAndDispose();

        if (File.Exists(CurrentSong.FilePath))
        {
            _audioPlayer = _audioManager.CreatePlayer(File.OpenRead(CurrentSong.FilePath));
            _audioPlayer.Volume = Volume;
            Position = 0;
            Duration = _audioPlayer.Duration;

            _audioPlayer.Play();
            IsPlaying = true;
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

    public void ToggleShuffle()
    {
        IsShuffleOn = !IsShuffleOn;
    }

    public void CycleRepeatMode()
    {
        RepeatMode = RepeatMode switch
        {
            RepeatMode.Off => RepeatMode.RepeatAll,
            RepeatMode.RepeatAll => RepeatMode.RepeatOne,
            RepeatMode.RepeatOne => RepeatMode.Off,
            _ => RepeatMode.Off
        };
    }

    public void SkipNext()
    {
        SkipBy(1, isAutoAdvance: false);
    }

    public void SkipPrevious()
    {
        SkipBy(-1, isAutoAdvance: false);
    }

    private void SkipBy(int direction, bool isAutoAdvance)
    {
        if (PlaybackQueue == null || PlaybackQueue.Count == 0 || CurrentSong == null)
            return;

        if (isAutoAdvance && RepeatMode == RepeatMode.RepeatOne)
        {
            SeekTo(0);
            _audioPlayer?.Play();
            IsPlaying = true;
            return;
        }

        if (IsShuffleOn)
        {
            var nextIndex = _random.Next(PlaybackQueue.Count);
            CurrentSong = PlaybackQueue[nextIndex];
            return;
        }

        var currentIndex = PlaybackQueue.IndexOf(CurrentSong);
        if (currentIndex == -1)
            return;

        var newIndex = currentIndex + direction;

        if (newIndex < 0 || newIndex >= PlaybackQueue.Count)
        {
            if (RepeatMode == RepeatMode.RepeatAll)
            {
                newIndex = direction > 0 ? 0 : PlaybackQueue.Count - 1;
            }
            else
            {
                return;
            }
        }

        CurrentSong = PlaybackQueue[newIndex];
    }

    private void UpdatePosition()
    {
        if (_audioPlayer == null)
            return;

        if (Duration <= 0 && _audioPlayer.Duration > 0)
            Duration = _audioPlayer.Duration;

        if (IsPlaying && !_isSeeking)
        {
            Position = _audioPlayer.CurrentPosition;

            if (Duration > 0 && Position >= Duration - 0.5)
            {
                SkipBy(1, isAutoAdvance: true);
            }
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