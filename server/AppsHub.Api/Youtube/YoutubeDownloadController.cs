using Microsoft.AspNetCore.Mvc;

namespace AppsHub.Api.Youtube;

[ApiController]
[Route("api/youtube")]
public class YoutubeDownloadController : ControllerBase
{
    private readonly IDownloadService _downloadService;

    public YoutubeDownloadController(IDownloadService downloadService)
    {
        _downloadService = downloadService;
    }

    [HttpPost("download")]
    public async Task<ActionResult<DownloadResult>> Download(DownloadRequest request, CancellationToken cancellationToken)
    {
        var result = await _downloadService.DownloadFileAsync(request.Url, request.Format, cancellationToken);

        return result.Outcome switch
        {
            DownloadOutcome.Success => Ok(value: result),
            DownloadOutcome.Refused => Problem(detail: result.Message, statusCode: 400),
            DownloadOutcome.NotFound => Problem(detail: result.Message, statusCode: 422),
            DownloadOutcome.Failed => Problem(detail: result.Message, statusCode: 500),
            _ => Problem(detail: result.Message, statusCode: 500)
        };
    }
}
