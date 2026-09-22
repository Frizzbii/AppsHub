using Microsoft.AspNetCore.Mvc;

namespace AppsHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class YoutubeController : ControllerBase
{
    [HttpPost("download")]
    public YoutubeDownload Download(YoutubeDownload download)
    {
        return download;
    }
}
