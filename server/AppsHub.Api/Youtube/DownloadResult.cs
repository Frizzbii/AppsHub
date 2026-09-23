namespace AppsHub.Api.Youtube;

public enum DownloadOutcome { Undefined, Success, Refused, NotFound, Failed }

public record DownloadResult(DownloadOutcome Outcome, string? Filepath, string? Message);