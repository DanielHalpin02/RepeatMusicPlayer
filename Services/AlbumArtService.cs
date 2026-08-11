using System.Net.Http;
using System.Text.Json;

namespace RepeatMusicPlayer.Services;

public class AlbumArtService
{
    private readonly HttpClient _httpClient = new();

    public AlbumArtService()
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "RepeatMusicPlayer/1.0");
    }

    public async Task<string> GetAlbumArtUrlAsync(string artist, string album)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(artist) || string.IsNullOrWhiteSpace(album)
                || artist == "Unknown Artist" || album == "Unknown Album")
            {
                return null;
            }

            var releaseId = await GetMusicBrainzReleaseIdAsync(artist, album);
            if (string.IsNullOrEmpty(releaseId))
                return null;

            var coverUrl = await GetCoverArtUrlAsync(releaseId);
            return coverUrl;
        }
        catch
        {
            // Any failure (network, parsing, no match) just means no album art - never crash the app for this.
            return null;
        }
    }

    private async Task<string> GetMusicBrainzReleaseIdAsync(string artist, string album)
    {
        var query = Uri.EscapeDataString($"artist:\"{artist}\" AND release:\"{album}\"");
        var url = $"https://musicbrainz.org/ws/2/release/?query={query}&fmt=json";

        var response = await _httpClient.GetStringAsync(url);
        using var doc = JsonDocument.Parse(response);

        if (!doc.RootElement.TryGetProperty("releases", out var releases) || releases.GetArrayLength() == 0)
            return null;

        var firstRelease = releases[0];
        if (firstRelease.TryGetProperty("id", out var idElement))
            return idElement.GetString();

        return null;
    }

    private async Task<string> GetCoverArtUrlAsync(string releaseId)
    {
        var url = $"https://coverartarchive.org/release/{releaseId}";

        var response = await _httpClient.GetStringAsync(url);
        using var doc = JsonDocument.Parse(response);

        if (!doc.RootElement.TryGetProperty("images", out var images) || images.GetArrayLength() == 0)
            return null;

        var firstImage = images[0];
        if (firstImage.TryGetProperty("image", out var imageUrlElement))
            return imageUrlElement.GetString();

        return null;
    }
}