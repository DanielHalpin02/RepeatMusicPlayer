using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugin.Maui.Audio;
using RepeatMusicPlayer.Models;

namespace RepeatMusicPlayer.ViewModels;

[QueryProperty(nameof(CurrentSong), "Song")]
public class NowPlayingViewModel : INotifyPropertyChanged
{
    private readonly IAudioManager _audioManager;
    private IAudioPlayer _audioPlayer;

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

    public NowPlayingViewModel(IAudioManager audioManager)
    {
        _audioManager = audioManager;
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
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}