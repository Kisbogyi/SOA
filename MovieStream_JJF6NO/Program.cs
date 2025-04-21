using MovieStream_JJF6NO.Services;
using Streaming;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
var sections = builder.Configuration.GetSection("SingleMovies").GetSection("Url");

var backendUrls = sections.AsEnumerable().Select(x => x.Value).Where(x => !string.IsNullOrEmpty(x)).ToArray();

builder.Services.AddGrpcClient<SingleMovie.SingleMovieClient>("url0", o =>
{
    o.Address = new Uri(backendUrls[0]!);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return handler;
});

builder.Services.AddGrpcClient<SingleMovie.SingleMovieClient>("url1", o =>
{
    o.Address = new Uri(backendUrls[1]!);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return handler;
});

var app = builder.Build();
app.UseStaticFiles();
app.MapGet("/", () => "Hello World Front-End!");
app.MapGrpcService<MovieFrontendService>();
app.Run();