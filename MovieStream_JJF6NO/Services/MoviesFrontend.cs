using Grpc.Core;
using Grpc.Net.ClientFactory;
using Streaming;

namespace MovieStream_JJF6NO.Services;

public class MovieFrontendService : MovieStream.MovieStreamBase
{
    private readonly IEnumerable<SingleMovie.SingleMovieClient> _clients = [];

    public MovieFrontendService(GrpcClientFactory grpcClientFactory)
    {
        _clients = _clients.Append(grpcClientFactory.CreateClient<SingleMovie.SingleMovieClient>("url0"));
        _clients = _clients.Append(grpcClientFactory.CreateClient<SingleMovie.SingleMovieClient>("url1"));
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