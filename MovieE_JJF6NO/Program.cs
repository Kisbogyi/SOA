using MovieE_JJF6NO;
using Streaming;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
var app = builder.Build();
app.MapGet("/", () => "E Back-End!");
app.MapGrpcService<EMicroservice>();
app.Run();