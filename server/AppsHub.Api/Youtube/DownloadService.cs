using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Extensions.Options;

namespace AppsHub.Api.Youtube;

public class DownloadService : IDownloadService
{
    private readonly DownloadSettings _settings;
    private readonly ILogger<DownloadService> _logger;

    private static readonly HashSet<string> AuthorizedSchemes = ["http", "https"];
    private static readonly HashSet<string> AuthorizedHosts = ["www.youtube.com", "youtube.com", "youtu.be", "m.youtube.com", "music.youtube.com"];

    public DownloadService(IOptions<DownloadSettings> options, ILogger<DownloadService> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<DownloadResult> DownloadFileAsync(string url, DownloadFormat format, CancellationToken cancellationToken)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult))
        {
            return new DownloadResult(DownloadOutcome.Refused, null, "Provide a real URL.");
        }

        string host = uriResult.Host;
        string scheme = uriResult.Scheme;

        if (!AuthorizedSchemes.Contains(scheme))
        {
            return new DownloadResult(DownloadOutcome.Refused, null, "Only http/https URLs are accepted.");
        }

        if (!AuthorizedHosts.Contains(host))
        {
            return new DownloadResult(DownloadOutcome.Refused, null, "Only YouTube URLs are accepted.");
        }

        ProcessStartInfo processStartInfo = new ProcessStartInfo(_settings.Ytdlp)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        string[] arguments = [ "-f", "ba", "-x",
                            "--audio-format", "mp3",
                            "--audio-quality", "0",
                            "-o", Path.Combine(_settings.DownloadFolder, "%(id)s.%(ext)s"),
                            "--print", "after_move:filepath",
                            "--ffmpeg-location",  _settings.Ffmpeg,
                            "--js-runtimes", ("deno:" + _settings.Deno),
                            uriResult.OriginalString ];
        foreach (string argument in arguments)
        {
            processStartInfo.ArgumentList.Add(argument);
        }

        Process? process;

        try
        {
            process = Process.Start(processStartInfo);
        }
        catch (Win32Exception exception)
        {
            _logger.LogError(exception, "Unable to start yt-dlp at {YtdlpPath}", _settings.Ytdlp);
            return new DownloadResult(DownloadOutcome.Failed, null, "Failed to start download.");
        }

        if (process is null)
        {
            return new DownloadResult(DownloadOutcome.Failed, null, "Failed to start download.");
        }

        using (process)
        {
            if (process is null) return new DownloadResult(DownloadOutcome.Failed, null, "Failed to start download.");

            Task<string> outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
            Task<string> errorTask = process.StandardError.ReadToEndAsync(cancellationToken);

            await process.WaitForExitAsync(cancellationToken);

            string output = await outputTask;
            string error = await errorTask;

            if (process.ExitCode != 0)
            {
                _logger.LogWarning("yt-dlp exited with code {ExitCode}. Error output: {Error}", process.ExitCode, error);
                return ClassifyError(error);
            }

            if (string.IsNullOrWhiteSpace(output)) return new DownloadResult(DownloadOutcome.Failed, null, "Download has failed: Output is empty.");

            return new DownloadResult(DownloadOutcome.Success, output.Trim(), "Download was successful.");
        }
    }

    private static DownloadResult ClassifyError(string error)
    {
        if (error.Contains("unavailable", StringComparison.OrdinalIgnoreCase))
        {
            return new DownloadResult(DownloadOutcome.NotFound, null, "Video was not found: Exit code ");
        }
        if (error.Contains("incomplete", StringComparison.OrdinalIgnoreCase))
        {
            return new DownloadResult(DownloadOutcome.NotFound, null, "Incomplete youtube url: Exit code ");
        }

        return new DownloadResult(DownloadOutcome.Failed, null, "Download has failed: Exit code ");
    }
}