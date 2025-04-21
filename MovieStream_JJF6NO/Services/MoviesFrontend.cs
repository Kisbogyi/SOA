using Grpc.Core;
using Grpc.Net.ClientFactory;
using Streaming;

namespace MovieStream_JJF6NO.Services;

public class MovieFrontendService : MovieStream.MovieStreamBase
{
    private readonly IEnumerable<SingleMovie.SingleMovieClient> _clients = [];

    public MovieFrontendService(GrpcClientFactory grpcClientFactory)
    {
        for (var i = 0; i < 10; i++)
        {
            try
            {
                var clnt = grpcClientFactory.CreateClient<SingleMovie.SingleMovieClient>($"url{i}");
                _clients = _clients.Append(clnt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }

    public override async Task<GetMoviesReply> GetMovies(GetMoviesRequest request, ServerCallContext context)
    {
        var movieInfos = _clients.Select(backend => new MovieInfo
        {
            Title = backend.GetTitle(new GetTitleRequest()).Title,
            Length = backend.GetLength(new GetLengthRequest()).Length
        }).ToList();
        return new GetMoviesReply { Movies = { movieInfos } };
    }

    public override async Task Watch(WatchRequest request, IServerStreamWriter<WatchReply> responseStream,
        ServerCallContext context)
    {
        var client = _clients.Where(client => client.GetTitle(new GetTitleRequest()).Title == request.Title);
        var movie = client.First().GetFrames(new GetFramesRequest()).Frame;
        for (var i = request.StartPosition; i < movie.Count; i++)
            await responseStream.WriteAsync(new WatchReply { Frame = movie[i] });
    }
}