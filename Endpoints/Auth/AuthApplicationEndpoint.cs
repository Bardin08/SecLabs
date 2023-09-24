using FastEndpoints;
using SecurityLabs.Contracts.Api;
using SecurityLabs.Contracts.Api.Models;
using SecurityLabs.Services.Interfaces;

namespace SecurityLabs.Endpoints.Auth;

internal class AuthApplicationEndpoint
    : EndpointWithoutRequest<AuthInfoResponse>
{
    private readonly IApplicationTokenService _applicationTokenService;

    public AuthApplicationEndpoint(IApplicationTokenService applicationTokenService)
    {
        _applicationTokenService = applicationTokenService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("internal/application/token");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var token = await _applicationTokenService.GetApplicationCredentialsAsync();
        await SendAsync(token!, cancellation: ct);
    }
}