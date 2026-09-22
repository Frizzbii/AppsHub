namespace AppsHub.Api;

public enum DownloadFormat { Mp3, Mp4 }

public record YoutubeDownload(string Url, DownloadFormat Format);