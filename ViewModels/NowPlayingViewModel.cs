using System.ComponentModel;
using System.Runtime.CompilerServices;
using RepeatMusicPlayer.Models;

namespace RepeatMusicPlayer.ViewModels;

[QueryProperty(nameof(CurrentSong), "Song")]
public class NowPlayingViewModel : INotifyPropertyChanged
{
    private Song _currentSong;
    public Song CurrentSong
    {
        get => _currentSong;
        set
        {
            _currentSong = value;
            OnPropertyChanged();
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

    public void PlayPause()
    {
        IsPlaying = !IsPlaying;
        // actual audio playback logic goes here later
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}