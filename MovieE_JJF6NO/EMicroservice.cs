using Grpc.Core;
using Streaming;

namespace MovieE_JJF6NO;

public class EMicroservice : SingleMovie.SingleMovieBase
{
    private const string E =
        "27182818284590452353602874713526624977572470936999595749669676277240766303535475945713821785251664274274663919320030599218174135966290435729003342952605956307381323286279434907632338298807531952510190115738341879307021540891499348841675092447614606680822648001684774118537423454424371075390777449920695517027618386062613313845830007520449338265602976067371132007093287091274437470472306969772093101416928368190255151086574637721112523897844250569536967707854499699679468644549059879316368892300987931";

    public override Task<GetTitleReply> GetTitle(GetTitleRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GetTitleReply { Title = "e" });
    }

    public override Task<GetLengthReply> GetLength(GetLengthRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GetLengthReply { Length = 500 });
    }

    public override async Task<GetFramesReply> GetFrames(GetFramesRequest request, ServerCallContext context)
    {
        return new GetFramesReply { Frame = { E.Select(c => int.Parse(c.ToString())) } };
    }
}
