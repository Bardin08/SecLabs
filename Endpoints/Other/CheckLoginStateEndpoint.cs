using FastEndpoints;

namespace SecurityLabs.Endpoints.Other;

public class CheckLoginStateEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("/api/login/status");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new
        {
            IsLogin = true,
        }, cancellation: ct);
    }
}