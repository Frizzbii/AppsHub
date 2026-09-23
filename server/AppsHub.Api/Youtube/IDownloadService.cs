namespace AppsHub.Api.Youtube;

public interface IDownloadService
{
    public Task<DownloadResult> DownloadFileAsync(string url, DownloadFormat format, CancellationToken cancellationToken);
}