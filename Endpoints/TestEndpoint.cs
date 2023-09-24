using FastEndpoints;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Endpoints;

internal class TestEndpoint : EndpointWithoutRequest
{
    private readonly IApplicationTokenService _tokenService;

    public TestEndpoint(IApplicationTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("api/healthz");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new
        {
            Status = "healthy",
            ApplicationCredentials = await _tokenService.GetApplicationCredentialsAsync()
        }, cancellation: ct);
    }
}