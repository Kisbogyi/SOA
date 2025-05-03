using MoviePi_JJF6NO;
using Streaming;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
var app = builder.Build();
app.MapGet("/", () => "PI Back-End!");
app.MapGrpcService<PiMicroservice>();
app.Run();