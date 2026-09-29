using System.Text.Json;
using System.Text.Json.Serialization;
using AppsHub.Api.Youtube;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Makes the enums always string, forbids integers and makes required parameters mandatory.
        // => Hardens JSON input validation for the whole API.
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false));
        options.JsonSerializerOptions.RespectRequiredConstructorParameters = true;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddOptions<DownloadSettings>()
    .BindConfiguration("YoutubeDownloadSettings")
    .Validate(settings => File.Exists(settings.Ytdlp),
        "yt-dlp not found. Check YoutubeDownloadSettings:Ytdlp.")
    .Validate(settings => File.Exists(settings.Ffmpeg),
        "ffmpeg not found. Check YoutubeDownloadSettings:Ffmpeg.")
    .Validate(settings => File.Exists(settings.Deno),
        "deno not found. Check YoutubeDownloadSettings:Deno.")
    .Validate(settings => Directory.Exists(settings.DownloadFolder),
        "Download folder not found. Check YoutubeDownloadSettings:DownloadFolder.")
    .ValidateOnStart();
builder.Services.AddScoped<IDownloadService, DownloadService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
